using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class PlayGame3Controller : MonoBehaviour {

	public GameObject instructionPanel;
	public GameObject pausePanel;
	public GameObject playPanel;
	public GameObject btnLeave , roundHintText;
	public Image blackBackground;
	public Image timeBar;
	public GameObject btnCharPrefab;
	private float decreaseAmount, userReactTime, roundStartTime, ansHiddenTime;
	private bool isShowingInstru, isNextRoundConstructed, triggerNextRound, roundStart, isAnsHidden, isSecondClick, isPaused, isBirdPressed,isRoundFail;
	private int isUserClickCorrect;
	public GameObject buttonMatrix;
	private const int btnWidth  = 150;
	private const int btnHeight = 150;
	private int currentRow, currentCol, currentRound, totalRound, totalWords, matrixRowPos, matrixColPos, clickedRowPos, clickedColPos, ansPos, ansRow, ansCol;
	// another ans that randomly choose from the wordList
	private int extraAnsCol, extraAnsRow;
	private const int spaceSize = 30;
	private const string PREFIX_AUDIO_PATH = "http://chinesecharactergame.cloudapp.net/character/audio/";
	private const int LAST_NUM_OF_AUDIO_PATH = 57;
	private List<CharacterSet> fakeDataCharList;
	public AudioSource audioSource;
	private List<CharacterSet> roundWordList;
	private AudioClip targetAudioClip, clearClip, failClip, correctClip, wrongClip;
	private List<UserPerformance> userPerformanceList;
	private ScreenManager sm;
	private string strTargetWord, strTargetAudioUrl , strUserChoosen;
	public Sprite imageHints, imageRetryBg, imageRetryBtn;
	public Image resultBackground;
	public GameObject resultPanel;
	public Text accuracyText;
	public Button btnContinue, btnBird;
	// Use this for initialization 
	void Start () {
		Application.targetFrameRate = 30;

		initFakeData ();

		initRoundParams ();
		initSettingFromServer ();
		constructureMatrix();
		showInstru ();

	}

	
	// Update is called once per frame
	void Update () {
		if (!isNextRoundConstructed && !(currentRound > totalRound) && triggerNextRound) {
			resetParamsForNextRound ();
			shuffleList ();
			constructureMatrix ();
			isNextRoundConstructed = true;

		}
		timeControl ();
		if (roundStart && isUserClickCorrect != 0) {
			countUserPerformTime ();
		}

		if (isBirdPressed) {
			roundStart = true;
			playTargetWordAudio ();
			isBirdPressed = false;
		}
	}

	private void initFakeData(){
		fakeDataCharList = new List<CharacterSet>();
		roundWordList = new List<CharacterSet>();

		userPerformanceList = new List<UserPerformance> ();
		CharacterSet charactorSet1 = new CharacterSet ();
		charactorSet1.setCharactor ("\u6068");
		charactorSet1.setAudioUrl ("http://chinesecharactergame.cloudapp.net/character/audio/D3QxulCU5FquxTrOiGdINJLzQNfqO9yNXjxjZxZR.WAV");
		fakeDataCharList.Add (charactorSet1);

		CharacterSet charactorSet2 = new CharacterSet ();
		charactorSet2.setCharactor ("\u6062");
		charactorSet2.setAudioUrl ("http://chinesecharactergame.cloudapp.net/character/audio/BWZ1rtMKaVUxajQbe1RuyGxrH8MkJQFWCSkgb1cd.WAV");
		fakeDataCharList.Add (charactorSet2);

		CharacterSet charactorSet3 = new CharacterSet ();
		charactorSet3.setCharactor ("\u6052");
		charactorSet3.setAudioUrl ("http://chinesecharactergame.cloudapp.net/character/audio/xEZuJJAmzsy7dfa6NJNNg1VgENxIePBFFKGydiY1.WAV");
		fakeDataCharList.Add (charactorSet3);

		CharacterSet charactorSet4 = new CharacterSet ();
		charactorSet4.setCharactor ("\u6064");
		charactorSet4.setAudioUrl ("http://chinesecharactergame.cloudapp.net/character/audio/M2GMWnpqgUiVNVioDOdXhPUTU7QBeBP7i0amLpcm.WAV");
		fakeDataCharList.Add (charactorSet4);

		CharacterSet charactorSet5 = new CharacterSet ();
		charactorSet5.setCharactor ("\u6065");
		charactorSet5.setAudioUrl ("http://chinesecharactergame.cloudapp.net/character/audio/z8rdzu9TEd3EpMVtSmcWYpKfoSWuFC3jLJgcj0Sa.WAV");
		fakeDataCharList.Add (charactorSet5);


		CharacterSet charactorSet6 = new CharacterSet ();
		charactorSet6.setCharactor ("\u6050");
		charactorSet6.setAudioUrl ("http://chinesecharactergame.cloudapp.net/character/audio/wdbe4rcamU9Gh7Cc4qV2WLtd5vHyAF42fvgIE95G.WAV");
		fakeDataCharList.Add (charactorSet6);



	}

	private void initRoundParams(){

		correctClip =  (AudioClip)Resources.Load ("Sounds/correct");
		wrongClip =  (AudioClip)Resources.Load ("Sounds/wrong");
		failClip = (AudioClip) Resources.Load ("Sounds/fail_dialog");
		clearClip = (AudioClip) Resources.Load ("Sounds/win_dialog");
		 
		isBirdPressed = false;
		isRoundFail = false;
		roundStart = false;
		triggerNextRound = false;
		isNextRoundConstructed = false;
		totalWords = fakeDataCharList.Count;
		isUserClickCorrect = -1;
		roundHintText.SetActive (true);
//		totalRound = totalWords/(currentCol*currentRow);
		currentRound = 1;
		isSecondClick = false;
		isPaused = false;
		if (sm == null) {
			sm = GameObject.FindObjectOfType(typeof(ScreenManager)) as ScreenManager;
		}

	}

	private void initSettingFromServer(){ 
		currentCol = 2;
		currentRow = 2;
		float tempTotalTime = 5f;
		decreaseAmount = 1/tempTotalTime;
		ansHiddenTime = 1.4f;
		totalRound = 4 ;
	}
	public void showInstru(){
		isShowingInstru = true;
		instructionPanel.SetActive (true);
		blackBackground.GetComponent<CanvasGroup> ().alpha = 0.3f;
		
	}
	public void closeInstruction(){
		blackBackground.GetComponent<CanvasGroup> ().alpha = 0f;
		instructionPanel.SetActive (false);
		isShowingInstru = false;

	}
	private void timeControl(){
		if (!isShowingInstru && !isPaused && roundStart) {
			if (!triggerNextRound && isUserClickCorrect != 0) {
				timeBar.fillAmount -= decreaseAmount * Time.deltaTime;
				if (timeBar.fillAmount <= 0f) {
					nextRound ();
				}
			}
		}
	}
		
	private void countUserPerformTime(){
		userReactTime = Time.time - roundStartTime;
	}

	private void constructureMatrix(){

		calculateMatrixSize ();
		initMatrix ();

	}
	private void calculateMatrixSize(){
		buttonMatrix.GetComponent<RectTransform>().sizeDelta = new Vector2(btnWidth*currentCol + 
							spaceSize*currentCol, btnWidth*currentRow + spaceSize*currentRow);

	}


	private void initMatrix(){
		if(currentRound == 1)
			shuffleList ();
		// randomly select which charactor is same
		int randomSameCharIndex = Random.Range (0, currentCol * currentRow - 1);
		CharacterSet randomCharactorSet;
		randomCharactorSet = fakeDataCharList [randomSameCharIndex];
		strTargetWord = randomCharactorSet.getCharactor();
		strTargetAudioUrl = randomCharactorSet.getAudioUrl();
//		int seperatePos = strTargetAudioUrl.LastIndexOfAny(PREFIX_AUDIO_PATH, 0 );
		string substrAudioName = strTargetAudioUrl.Substring (LAST_NUM_OF_AUDIO_PATH,strTargetAudioUrl.Length -LAST_NUM_OF_AUDIO_PATH-4);
		targetAudioClip = (AudioClip) Resources.Load ("Sounds/" + substrAudioName);
		Debug.Log("After substring");



		for (int i = 0 ; i < currentCol * currentRow ; i++) {
			
			GameObject charBtn = Instantiate (btnCharPrefab) as GameObject;
			charBtn.GetComponentInChildren<Text> ().text = fakeDataCharList [i].getCharactor();
			charBtn.transform.SetParent (buttonMatrix.transform, false);
			string btnString = fakeDataCharList [i].getCharactor();
			calculateCurrentRow (i + 1);
			charBtn.name = matrixRowPos + "," + matrixColPos; 

			charBtn.GetComponent<Button> ().onClick.AddListener (() => {
				onUserClick (btnString);
			});
			roundWordList.Add (fakeDataCharList [i]);

		}
		findAnsPos (randomSameCharIndex);


			
	}

	public void onClickBird(){
		// allow user to press the bird when the round is not started 
		if (!roundStart)
			gameStart ();
	}

	private void playTargetWordAudio(){
		audioSource.PlayOneShot (targetAudioClip);
	}

	private void gameStart(){
		buttonMatrix.SetActive (true);
		roundStartTime = Time.time;
		isBirdPressed = true;
		roundHintText.SetActive (false);


	}
	private void shuffleList(){
		Debug.Log ("before shuffle");
		for (int i = 0; i < fakeDataCharList.Count ; i++) {
			CharacterSet temp = fakeDataCharList[i];
			int randomIndex = Random.Range(i, fakeDataCharList.Count);
			fakeDataCharList[i] = fakeDataCharList[randomIndex];
			fakeDataCharList[randomIndex] = temp;
		}
		Debug.Log ("After shuffle");
	}
	private void printList(){
//		foreach (string value in fakeDataCharList) {
//			Debug.Log (value);
//		}
	}

	private void findAnsPos(int randomSameCharIndex){
		ansRow = GameUtils.findRowPosition (currentRow, currentCol, randomSameCharIndex + 1);
		ansCol = GameUtils.findColPosition (currentRow, currentCol, randomSameCharIndex + 1); 
		Debug.Log ("ansRow " + ansRow);
		Debug.Log ("ansCol " + ansCol);


	}

	private void onUserClick(string clickedWord){
		if (clickedWord.Equals (strTargetWord)) {
			if (!isSecondClick) {
				string strClickedPos = EventSystem.current.currentSelectedGameObject.name;
				int seperatePos = strClickedPos.IndexOf (',');
				clickedRowPos = int.Parse (strClickedPos.Substring (0, seperatePos));
				clickedColPos = int.Parse (strClickedPos.Substring (seperatePos + 1, 1));
				isUserClickCorrect = 1;
				audioSource.PlayOneShot (correctClip);
				strUserChoosen = clickedWord;
			} else {

				isUserClickCorrect = 0;
			}
			isSecondClick = true;

			nextRound ();
		} else {
			audioSource.PlayOneShot (wrongClip);
			string strClickedPos = EventSystem.current.currentSelectedGameObject.name;
			int seperatePos = strClickedPos.IndexOf (',');
			clickedRowPos = int.Parse (strClickedPos.Substring (0, seperatePos));
			clickedColPos = int.Parse (strClickedPos.Substring (seperatePos+1,1));
			strUserChoosen = clickedWord;
			isSecondClick = true;
			isUserClickCorrect = 0;

			foreach (Transform child in buttonMatrix.transform) {
				Debug.Log ("child name " + child.name);
				if (child.name.Equals (ansRow + "," + ansCol)) {
					GameObject charBtn = Instantiate (btnCharPrefab) as GameObject;
					child.GetComponent<Button> ().image.sprite = imageHints;
					break;					
				}
			}
		}
	}



	private void nextRound(){
		syncUserPerformanceData ();
		currentRound += 1;
		triggerNextRound = true;
		isNextRoundConstructed = false;
		roundStart = false;
		if (currentRound > totalRound) {
			endGame ();	
		}
	}

	private void syncUserPerformanceData(){
		UserPerformance up = new UserPerformance ();
		up.setTrial (currentRound);

		up.setAnsWord (strTargetWord);

		up.setTrialOutcome (isUserClickCorrect);
		up.setFirstTime (1);
		up.setUserChoosenChar (strUserChoosen);


		up.setUserChoosenRow (clickedRowPos);
		up.setUserChoosenCol (clickedColPos);
		up.setUserReactTime (userReactTime);
		up.setAnsRow (ansRow);
		up.setAnsCol (ansCol);

		userPerformanceList.Add (up);

	}

	private void resetParamsForNextRound(){
		roundHintText.SetActive (true);
		buttonMatrix.SetActive (false);
		isUserClickCorrect = -1;
		timeBar.fillAmount = 1;
		isBirdPressed = false;
		ansPos = 0;
		triggerNextRound = false;
		isSecondClick = false;
		roundStartTime = Time.time;
		clickedColPos = 0;
		clickedRowPos = 0;
		ansRow = 0;
		ansCol = 0;
		isRoundFail = false;
		roundWordList.Clear();
		foreach (Transform child in buttonMatrix.transform) {
			GameObject.Destroy(child.gameObject);
		}


	}
	private void calculateCurrentRow(int position){
		matrixRowPos = GameUtils.findRowPosition(currentRow, currentCol, position);
		matrixColPos = GameUtils.findColPosition(currentRow, currentCol, position);

	}
	private void endGame(){
		resultPanel.SetActive (true);
		int passRound = 0;

		foreach (UserPerformance uPerformance in userPerformanceList) {
			if (uPerformance.getTrialOutcome() == 1) {
				passRound += 1;
			}
		}

		float accuracy = ((float)passRound / (float)totalRound) * 100;
		accuracyText.text = accuracy.ToString()+"%";
		if (accuracy >= 80) {
			Debug.Log ("win");
			iTween.MoveBy (resultPanel, new Vector3 (0, -Screen.height,0), 1f);
			audioSource.PlayOneShot (clearClip);


		} else {
			resultBackground.sprite = imageRetryBg;
			btnContinue.image.sprite = imageRetryBtn;
			iTween.MoveBy (resultPanel, new Vector3 (0, -Screen.height,0), 1f);
			Debug.Log ("lose");	
			audioSource.PlayOneShot (failClip);


		}
		syncToServer();
	}

	public void onClickBackToHome(){
		sm.endGame ();
	}
	private void syncToServer(){
		
	}

	public void onClickResumeButton(){
		isPaused = false;
		iTween.MoveBy (pausePanel, new Vector3 (0, Screen.height,0), 0.1f);

	}
	public void onClickOptionPanel(){
		isPaused = true;
		iTween.MoveBy (pausePanel, new Vector3 (0, -Screen.height,0), 1f);
	}
	public void buttonText(){
		Debug.Log ("Testing text");
	}

} 
