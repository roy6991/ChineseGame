using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectGamesController : MonoBehaviour {
	public GameObject gamePanelGroup;

	public GameObject optionPanel;
	public GameObject selectGamesPanel;
	public GameObject MaskPanel;
	public Camera MainCamera;

	private const int NUM_LV = 60;
	private const int TAG_GAME_1 = 1;
	private const int TAG_GAME_2 = 2;
	private const int TAG_GAME_3 = 3;
	private const int TAG_GAME_4 = 4;
	private const int TAG_SELECT_GAMES = 5;
	private const int TAG_OPTION_PANEL  = 6;
	private const int btnWidthFullHD = 384;
	private const int btnHeightFullHD = 230;
	private int currentFocus, game1CurrentStageNum;
	private SelectGameUIController uiController;
    private LoginForm loginForm;

    void Awake()
    {
        loginForm = FindObjectOfType<LoginForm>();
    }

	// Use this for initialization
	void Start () {
		initGameParams();	
	}
	
	// Update is called once per frame
	void Update () {
		detectBackBtn();

	}
	public void onClickBtnOpt(){
		optionPanel.SetActive (true);
		iTween.MoveBy (optionPanel, new Vector3 (0, -Screen.height,0), 1f);

	}

	public void onClickBtnOptYes(){
        if (loginForm == null)
            loginForm = FindObjectOfType<LoginForm>();
        loginForm.LogoutRequest();
	}

	public void onClickBtnNo(){
		iTween.MoveBy (optionPanel, new Vector3 (0, Screen.height,0), 0.2f);
		Invoke ("disableOptionPanel", 0.2f);
			
	}

	private void disableOptionPanel (){
		optionPanel.SetActive (false);
	}

	private int lvOneClearStages(){
		return 28;
	}

	private void initGameParams(){
		currentFocus = TAG_SELECT_GAMES;
		game1CurrentStageNum = 1;
//		uiController = new SelectGameUIController ();
	}



	private void loadGameStage1(string lvClicked){
	
	}
	public void onClickGame1(){
		iTween.MoveBy (gamePanelGroup, new Vector3 (Screen.width,0,0), 1f);
		currentFocus = TAG_GAME_1;
//		uiController.onClickGame1Stage1 ();
	}

	public void onClickGame2(){
		iTween.MoveBy (gamePanelGroup, new Vector3 (0,-Screen.height,0), 1f);
		currentFocus = TAG_GAME_2;

	}
	public void onClickGame3(){
		iTween.MoveBy (gamePanelGroup, new Vector3 (-Screen.width,0,0), 1f);
		currentFocus = TAG_GAME_3;
	}
	public void onClickGame4(){
		iTween.MoveBy (gamePanelGroup, new Vector3 (0,Screen.height,0), 1f);
		currentFocus = TAG_GAME_4;
//		uiController.onClickGame4Stage1 ();

		
	}



	public void detectBackBtn(){
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKey (KeyCode.Escape)) {
				switch (currentFocus) {
				case TAG_GAME_1:
					iTween.MoveBy (gamePanelGroup, new Vector3 (-Screen.width, 0, 0), 1f);
					currentFocus = TAG_SELECT_GAMES;
					break;
				case TAG_GAME_2:
					iTween.MoveBy (gamePanelGroup, new Vector3 (0, Screen.height, 0), 1f);
					currentFocus = TAG_SELECT_GAMES;
					break;
				case TAG_GAME_3:
					iTween.MoveBy (gamePanelGroup, new Vector3 (Screen.width, 0, 0), 1f);
					currentFocus = TAG_SELECT_GAMES;
					break;
				case TAG_GAME_4:
					currentFocus = TAG_SELECT_GAMES;
					iTween.MoveBy (gamePanelGroup, new Vector3 (0, -Screen.height, 0), 1f);
					break;
				}

			}
		}
	}

}
