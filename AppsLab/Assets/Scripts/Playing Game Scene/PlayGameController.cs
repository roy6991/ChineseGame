using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class PlayGameController : MonoBehaviour {

	public GameObject instructionPanel;
	public GameObject pausePanel;
	public GameObject playPanel;
	public GameObject btnLeave;
	public Image blackBackground;
	public Image timeBar;
	public GameObject btntTargetWord;
	public GameObject btnCharPrefab;
	private float decreaseAmount, userReactTime, roundStartTime, ansHiddenTime;
	private bool isShowingInstru, showCounter, isNextRoundConstructed, triggerNextRound, roundStart, isAnsHidden, isSecondClick, isPaused;
	private int isUserClickCorrect;
	public GameObject startGameCounterGo, buttonMatrix;
	private Text startGameCountertxt;
	private const int btnWidth  = 150;
	private const int btnHeight = 150;
	private int currentRow, currentCol, currentRound, totalRound, totalWords, matrixRowPos, matrixColPos, clickedRowPos, clickedColPos, ansPos, ansRow, ansCol;
	private const int spaceSize = 30;
	private List<string> fakeDataCharList;
	private List<string> roundWordList;
	private List<UserPerformance> userPerformanceList;
	private ScreenManager sm;
	private AudioClip clearClip, failClip, correctClip, wrongClip;
	private string strTargetWord, strUserChoosen;
	public Sprite imageHints, imageRetryBg, imageRetryBtn;
	public Image resultBackground;
	public GameObject resultPanel;
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
			Debug.Log ("user react time : " + userReactTime);
			roundStart = true;

		}
		timeControl ();
		if (roundStart && !isSecondClick) {
			countUserPerformTime ();
		}
		if (!isAnsHidden) {
			controlAnsHidden ();
		}
	}

	private void initFakeData(){
		fakeDataCharList = new List<string>();
		roundWordList = new List<string> ();
		userPerformanceList = new List<UserPerformance> ();

		// TODO - get words 
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
		//TODO - get col and row
		currentCol = 4;
		currentRow = 2;
		float tempTotalTime = 5f;
		showCounter = false;
		decreaseAmount = 1/tempTotalTime;
		ansHiddenTime = 1.4f;
		totalRound = 1 ;
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
		showCounter = false;
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
	private void controlAnsHidden(){
		if((Time.time - roundStartTime) >= ansHiddenTime){
			btntTargetWord.SetActive (false);
		}
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
		Debug.Log("Current Round " + currentRound);
		for (int i = 0 ; i < currentCol * currentRow ; i++) {

			GameObject charBtn = Instantiate (btnCharPrefab) as GameObject;
			charBtn.GetComponentInChildren<Text> ().text = fakeDataCharList[i];
			charBtn.transform.SetParent (buttonMatrix.transform, false);
			string btnString = fakeDataCharList [i];
			calculateCurrentRow (i + 1);
			charBtn.name = matrixRowPos + "," + matrixColPos; 

			charBtn.GetComponent<Button> ().onClick.AddListener (() => {
				onUserClick (btnString);
			});
			roundWordList.Add(fakeDataCharList [i]);

		}
		strTargetWord = getTargetWord ();
		btntTargetWord.GetComponentInChildren<Text> ().text = strTargetWord;


	}
	private void gameStart(){
		buttonMatrix.SetActive (true);
		btntTargetWord.SetActive (true);
		roundStartTime = Time.time;
		roundStart = true;

	}
	private void shuffleList(){
		for (int i = 0; i < fakeDataCharList.Count; i++) {
			string temp = fakeDataCharList[i];
			int randomIndex = Random.Range(i, fakeDataCharList.Count);
			fakeDataCharList[i] = fakeDataCharList[randomIndex];
			fakeDataCharList[randomIndex] = temp;
		}
	}
	private void printList(){
		foreach (string value in fakeDataCharList) {
			Debug.Log (value);
		}
	}
	private string getTargetWord(){
		ansPos = Random.Range (0, (currentCol * currentRow - 1));
		findAnsPos ();
		return roundWordList [ansPos];
	}
	private void findAnsPos(){
		ansRow = GameUtils.findRowPosition (currentRow, currentCol, ansPos + 1);
		ansCol = GameUtils.findColPosition (currentRow, currentCol, ansPos + 1); 
	}
	private void onUserClick(string clickedWord){


		if (clickedWord.Equals (strTargetWord)) {
			if (!isSecondClick) {
				string strClickedPos = EventSystem.current.currentSelectedGameObject.name;
				int seperatePos = strClickedPos.IndexOf (',');
				clickedRowPos = int.Parse (strClickedPos.Substring (0, seperatePos));
				clickedColPos = int.Parse (strClickedPos.Substring (seperatePos + 1, 1));
				isUserClickCorrect = 1;
				strUserChoosen = clickedWord;
				audioSource.PlayOneShot (correctClip);
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
		up.setFirstTime (1);
		up.setAnsWord (strTargetWord);
		up.setTrialOutcome (isUserClickCorrect);
		up.setUserChoosenRow (clickedRowPos);
		up.setUserChoosenCol (clickedColPos);
		up.setUserChoosenChar (strUserChoosen);
		up.setUserReactTime (userReactTime);
		up.setAnsRow (ansRow);
		up.setAnsCol (ansCol);

		userPerformanceList.Add (up);

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
		roundWordList.Clear();
		foreach (Transform child in buttonMatrix.transform) {
			GameObject.Destroy(child.gameObject);
		}
		btntTargetWord.SetActive (true);


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
