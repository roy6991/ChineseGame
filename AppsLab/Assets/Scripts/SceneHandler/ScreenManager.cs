using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ScreenManager : MonoBehaviour {

	[SerializeField]
	private FadeSprite m_blackScreenCover;
	[SerializeField]
	private float m_minDuration = 1.5f;
	public Image fadingImage;
	private const float FADE_IN_OUT_TIME = 1f;
	private FadingManger fManager;
	private Communication communication;
	private LoginForm loginForm;
	private GameStageForm gameStageDataForm; 
	private GameStageData gameStageData;
	private bool canLogin, started, levelSelected, startedPlaying, isEndedGame, logouted;
	private string playSceneName;

	public string playerName = "";

	void Update()
	{
		if (canLogin && !started)
		{
			started = true;
			StartCoroutine(loadAsyncSelectGameScene("SelectGameScene"));

		}
		if (levelSelected && !startedPlaying) {
			startedPlaying = true;
			StartCoroutine (LoadAsyncPlayingScene (playSceneName));
			playSceneName = "";
			startedPlaying = false;
			levelSelected = false;
		}
		if (isEndedGame) {

			levelSelected = false;
			isEndedGame =false;

			StartCoroutine (BackToHomeSelectScene ("SelectGameScene"));

		}

        if (logouted)
        {
            logouted = false;
            started = false;
            canLogin = false;
            StartCoroutine(loadAsyncSelectGameScene("LoginScene"));
        }
	}

	void Awake (){
		isEndedGame = false;
		canLogin = false;
		started = false;
		levelSelected = false;
		startedPlaying = false;
        logouted = false;
		loginForm = FindObjectOfType<LoginForm> ();
		gameStageDataForm = FindObjectOfType<GameStageForm> ();
	}

	void Start(){
//		fadingImage.IsActive();
	}

	public void loginValid(string email, string password){
		loginForm.submitForm (email, password, false);
	}

	private void loginResult (bool result){
		if (result == true){
			gameStageDataForm.gameStageDataRequest ();
		}
	}

	private void getStageDataResult (GameStageData data){
		gameStageData = data;
		canLogin = true;
		print ("Result: " + gameStageData.printAllLevels ());
	}

    public void logout()
    {
        logouted = true;
    }

	public void AssignPlayerName (string name){
		playerName = name;
	}

	public void levelBtnSelected(string sceneName){
		playSceneName = sceneName;
		isEndedGame = false;
		levelSelected = true;
	}
	public void endGame(){
		isEndedGame = true;
	}
	// load to game select scene
	public IEnumerator loadAsyncSelectGameScene(string sceneName)
	{

		// Load loading screen
		yield return Application.LoadLevelAsync("LoadingScene");
		if(fManager == null){
			Debug.Log ("is null");
			fManager = GameObject.FindObjectOfType(typeof(FadingManger)) as FadingManger;
			fManager.processFadeIn ();

		}

		// Fade to black
		yield return new WaitForSeconds(FADE_IN_OUT_TIME);

		if (communication == null) {
			communication = GameObject.FindObjectOfType(typeof(Communication)) as Communication;
			yield return new WaitUntil (() => communication.isDownloadFinished () == true);

		}
		fManager.processFadeOut ();

		yield return new WaitForSeconds(FADE_IN_OUT_TIME);

		yield return Application.LoadLevelAdditiveAsync(sceneName);
		
		// !!! unload loading screen
		LoadingSceneManager.UnloadLoadingScene();


		
	}
	//LOAD game one scene
	public IEnumerator LoadAsyncPlayingScene(string sceneName){
		
		yield return Application.LoadLevelAsync("LoadingScene");
		if(fManager == null){
			Debug.Log ("is null");
			fManager = GameObject.FindObjectOfType(typeof(FadingManger)) as FadingManger;
			fManager.processFadeIn ();

		}

		// Fade to black
		yield return new WaitForSeconds(FADE_IN_OUT_TIME);


		fManager.processFadeOut ();

		yield return new WaitForSeconds(FADE_IN_OUT_TIME);
		yield return Application.LoadLevelAdditiveAsync(sceneName);


		// !!! unload loading screen
		LoadingSceneManager.UnloadLoadingScene();
	}



	//Back to home select game scene
	public IEnumerator BackToHomeSelectScene(string sceneName){

		yield return Application.LoadLevelAsync("LoadingScene");
		if(fManager == null){
			Debug.Log ("is null");
			fManager = GameObject.FindObjectOfType(typeof(FadingManger)) as FadingManger;
			fManager.processFadeIn ();

		}

		// Fade to black
		yield return new WaitForSeconds(FADE_IN_OUT_TIME);


		fManager.processFadeOut ();

		yield return new WaitForSeconds(FADE_IN_OUT_TIME);
		yield return Application.LoadLevelAdditiveAsync(sceneName);


		// !!! unload loading screen
//		LoadingSceneManager.UnloadLoadingScene();
	}


 	public  Communication getCommunication(){
		return communication;
	}
}
