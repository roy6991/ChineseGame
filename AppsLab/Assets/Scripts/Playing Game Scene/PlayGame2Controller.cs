using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class PlayGame2Controller : MonoBehaviour {

	public GameObject instructionPanel;
	public GameObject pausePanel;
	public GameObject playPanel;
	public GameObject btnLeave;
	public Image blackBackground;
	public Image timeBar;
	public GameObject btnCharPrefab;
	private float decreaseAmount, userReactTime, roundStartTime, ansHiddenTime;
	private bool isShowingInstru, showCounter, isNextRoundConstructed, triggerNextRound, roundStart, isAnsHidden, isSecondClick, isPaused, isRoundFail;
	private int isUserClickCorrect;
	public GameObject startGameCounterGo, buttonMatrix;
	private Text startGameCountertxt;
	private const int btnWidth  = 150;
	private const int btnHeight = 150;
	private int currentRow, currentCol, currentRound, totalRound, totalWords, matrixRowPos, matrixColPos, clickedRowPos, clickedColPos, ansPos, ansRow, ansCol;
	// another ans that randomly choose from the wordList
	private int extraAnsCol, extraAnsRow;
	private const int spaceSize = 30;
	private List<string> fakeDataCharList;
	private List<string> roundWordList;
	private List<UserPerformance> userPerformanceList;
	private ScreenManager sm;
	private string strTargetWord, strRandomSameChar , strUserFirstChoosen, strUserSecChoosen;
	public Sprite imageHints, imageRetryBg, imageRetryBtn;
	public Image resultBackground;
	public GameObject resultPanel;
	private AudioClip clearClip, failClip, correctClip, wrongClip;
	public Text accuracyText;
	public Button btnContinue;
	public AudioSource audioSource;
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
			roundStart = true;

		}
		timeControl ();
		if (roundStart && isUserClickCorrect != 0) {
			countUserPerformTime ();
		}

	}

	private void initFakeData(){
		fakeDataCharList = new List<string>();
		roundWordList = new List<string> ();
		userPerformanceList = new List<UserPerformance> ();
		fakeDataCharList.Add ("你");
		fakeDataCharList.Add ("好");
		fakeDataCharList.Add ("嗎");
		fakeDataCharList.Add ("做");
		fakeDataCharList.Add ("乜");
		fakeDataCharList.Add ("事");
		fakeDataCharList.Add ("野");
		fakeDataCharList.Add ("呀");

		fakeDataCharList.Add ("爆");
		fakeDataCharList.Add ("教");
		fakeDataCharList.Add ("學");
		fakeDataCharList.Add ("範");
		fakeDataCharList.Add ("本");
		fakeDataCharList.Add ("龍");
		fakeDataCharList.Add ("習");
		fakeDataCharList.Add ("社");


	}

	private void initRoundParams(){

		correctClip =  (AudioClip)Resources.Load ("Sounds/correct");
		wrongClip =  (AudioClip)Resources.Load ("Sounds/wrong");
		failClip = (AudioClip) Resources.Load ("Sounds/fail_dialog");
		clearClip = (AudioClip) Resources.Load ("Sounds/win_dialog");


		isRoundFail = false;
		roundStart = false;
		triggerNextRound = false;
		isNextRoundConstructed = false;
		totalWords = fakeDataCharList.Count;
		isUserClickCorrect = -1;
//		totalRound = totalWords/(currentCol*currentRow);
		currentRound = 1;
		isSecondClick = false;
//		startGameCounterGo.SetActive (true);
		startGameCountertxt = startGameCounterGo.GetComponent<Text> ();
		isPaused = false;
		if (sm == null) {
			sm = GameObject.FindObjectOfType(typeof(ScreenManager)) as ScreenManager;
		}

	}

	private void initSettingFromServer(){ 
		currentCol = 3;
		currentRow = 2;
		float tempTotalTime = 5f;
		showCounter = false;
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
//		showCounter = true;
//		StartCoroutine (startGameCounter());
		gameStart();

	}
	private void timeControl(){
		if (!isShowingInstru && showCounter == false && !isPaused) {
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
	private IEnumerator startGameCounter(){
		Debug.Log ("inside show counter");
		startGameCountertxt.text = "3";    
		yield return new WaitForSeconds (1f);  

		startGameCountertxt.text = "2";    
		yield return new WaitForSeconds (1f);

		startGameCountertxt.text = "1";    
		yield return new WaitForSeconds (1f);

//	 	yield return new WaitForSeconds (0.5f);
		showCounter = false;
		startGameCounterGo.SetActive (false);
		gameStart ();
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
		strRandomSameChar = fakeDataCharList [randomSameCharIndex];
		strTargetWord = strRandomSameChar;
		int insertPos = 0;	
		Debug.Log ("random string" +strRandomSameChar ); 
		// get the insert position
		while (true) {
			insertPos = Random.Range (0, currentCol * currentRow - 1);
			if (insertPos == randomSameCharIndex)
				continue;
			else
				break;
		}

		for (int i = 0 ; i < currentCol * currentRow ; i++) {

			if (insertPos != i) {
				GameObject charBtn = Instantiate (btnCharPrefab) as GameObject;
				charBtn.GetComponentInChildren<Text> ().text = fakeDataCharList [i];
				charBtn.transform.SetParent (buttonMatrix.transform, false);
				string btnString = fakeDataCharList [i];
				calculateCurrentRow (i + 1);
				charBtn.name = matrixRowPos + "," + matrixColPos; 

				charBtn.GetComponent<Button> ().onClick.AddListener (() => {
					onUserClick (btnString, charBtn);
				});
				roundWordList.Add (fakeDataCharList [i]);
			} else {
				Debug.Log ("inside random same char index"); 

				GameObject charBtn = Instantiate (btnCharPrefab) as GameObject;
				charBtn.GetComponentInChildren<Text> ().text = strRandomSameChar;
				charBtn.transform.SetParent (buttonMatrix.transform, false);
				string btnString = strRandomSameChar;
				calculateCurrentRow (i + 1);
				charBtn.name = matrixRowPos + "," + matrixColPos; 

				charBtn.GetComponent<Button> ().onClick.AddListener (() => {
					onUserClick (btnString, charBtn);
				});
				roundWordList.Add (strRandomSameChar);
			}
		}
		findAnsPos (randomSameCharIndex);
		findExtraAnsPos (insertPos);


			
	}
	private void gameStart(){
		buttonMatrix.SetActive (true);
		roundStartTime = Time.time;
		roundStart = true;

	}
	private void shuffleList(){
		Debug.Log ("before shuffle");
		for (int i = 0; i < fakeDataCharList.Count ; i++) {
			string temp = fakeDataCharList[i];
			int randomIndex = Random.Range(i, fakeDataCharList.Count);
			fakeDataCharList[i] = fakeDataCharList[randomIndex];
			fakeDataCharList[randomIndex] = temp;
		}
		Debug.Log ("After shuffle");
	}
	private void printList(){
		foreach (string value in fakeDataCharList) {
			Debug.Log (value);
		}
	}

	private void findAnsPos(int randomSameCharIndex){
		ansRow = GameUtils.findRowPosition (currentRow, currentCol, randomSameCharIndex + 1);
		ansCol = GameUtils.findColPosition (currentRow, currentCol, randomSameCharIndex + 1); 
		Debug.Log ("ansRow " + ansRow);
		Debug.Log ("ansCol " + ansCol);


	}
	private void findExtraAnsPos(int insertPos){
		extraAnsRow = GameUtils.findRowPosition (currentRow, currentCol, insertPos + 1);
		extraAnsCol = GameUtils.findColPosition (currentRow, currentCol, insertPos + 1);
		Debug.Log ("Extra ansRow " + extraAnsRow);
		Debug.Log ("Extra ansCol " + extraAnsCol);
	}
	private void onUserClick(string clickedWord, GameObject charBtn){
		string strClickedPos = EventSystem.current.currentSelectedGameObject.name;
		int seperatePos = strClickedPos.IndexOf (',');
		clickedRowPos = int.Parse (strClickedPos.Substring (0, seperatePos));
		clickedColPos = int.Parse (strClickedPos.Substring (seperatePos+1,1));
		if (!isSecondClick) {
			strUserFirstChoosen = clickedWord;
			syncUserPerformanceData ();
			isSecondClick = true;
			charBtn.GetComponent<Button>().interactable = false;

		} else {
			strUserSecChoosen = clickedWord;
			checkResultAfterTwoClick ();

		}
	}

	private void checkResultAfterTwoClick(){
		Debug.Log ("inside checkResult" + "first " + strUserFirstChoosen + " sec " + strUserSecChoosen + " target " +  strTargetWord);

		if (strUserFirstChoosen.Equals (strUserSecChoosen) && strUserFirstChoosen.Equals (strTargetWord) && isUserClickCorrect == -1) {
			isUserClickCorrect = 1;
//			gameObject.GetComponent<Button>().interactable = false;
			audioSource.PlayOneShot (correctClip);

			syncUserPerformanceData ();
			nextRound ();
		} else if ((strUserFirstChoosen != strTargetWord || strUserSecChoosen != strTargetWord) && isUserClickCorrect == -1) {
			isUserClickCorrect = 0;
			isRoundFail = true;
			audioSource.PlayOneShot (wrongClip);

			syncUserPerformanceData ();
			strUserFirstChoosen = "";
			strUserSecChoosen = "";
			isSecondClick = false;
			ButtonColorChange.resetColorButton (buttonMatrix);
			foreach (Transform child in buttonMatrix.transform) {
				
				child.GetComponent<Button>().interactable = true;

				if (child.name.Equals (ansRow + "," + ansCol) || child.name.Equals (extraAnsRow + "," + extraAnsCol) ) {
					GameObject charBtn = Instantiate (btnCharPrefab) as GameObject;
					child.GetComponent<Button> ().image.sprite = imageHints;
				}
			}
		} else if(strUserFirstChoosen.Equals (strUserSecChoosen) && strUserFirstChoosen.Equals (strTargetWord) && isUserClickCorrect == 0){
			nextRound ();
		}
	}

	private void nextRound(){
		currentRound += 1;
		triggerNextRound = true;
		isNextRoundConstructed = false;
		roundStart = false;
		if (currentRound > totalRound) {
			endGame ();	
		}
	}

	private void syncUserPerformanceData(){
		if (!isRoundFail) {
			UserPerformance up = new UserPerformance ();
			up.setTrial (currentRound);

			up.setAnsWord (strTargetWord);
			if (!isSecondClick) {
				up.setTrialOutcome (0);
				up.setFirstTime (1);
				up.setUserChoosenChar (strUserFirstChoosen);

			} else {
				up.setTrialOutcome (isUserClickCorrect);
				up.setFirstTime (0);
				up.setUserChoosenChar (strUserSecChoosen);


			}
			up.setUserChoosenRow (clickedRowPos);
			up.setUserChoosenCol (clickedColPos);
			up.setUserReactTime (userReactTime);
			up.setAnsRow (ansRow);
			up.setAnsCol (ansCol);

			userPerformanceList.Add (up);
		}

	}

	private void resetParamsForNextRound(){
		isUserClickCorrect = -1;
		timeBar.fillAmount = 1;
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


} 
