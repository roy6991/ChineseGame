using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class SelectGameUIController : MonoBehaviour {

	public GameObject gameOneStagebtn1, gameOneStagebtn2, gameOneStagebtn3;

	public GameObject gameTwoStagebtn1, gameTwoStagebtn2, gameTwoStagebtn3;

	public GameObject gameThreeStagebtn1, gameThreeStagebtn2, gameThreeStagebtn3;

	public GameObject gameFourStagebtn1, gameFourStagebtn2, gameFourStagebtn3;

	private Sprite[] allStageButtonSprite;
	private const int INDEX_GAME_1 = 1;
	private const int INDEX_GAME_2 = 2;
	private const int INDEX_GAME_3 = 3;
	private const int INDEX_GAME_4 = 4;

	public GameObject lvBtnContainerOne, lvBtnContainerTwo, lvBtnContainerThree, lvBtnContainerFour;
	public GameObject btnPrefabLockG1, btnPrefabLockG2, btnPrefabLockG3, btnPrefabLockG4;
	public GameObject lvBtnPrefabs,lvBtnPrefabsG2,lvBtnPrefabsG3,lvBtnPrefabsG4;
	public GameObject topStageContainer;

	public Text playerNameText;
	public Image playerLevelImage;

	private Communication mComm;
	private List<Dictionary<int, GameLevel>> games;
	private Dictionary<int, CharacterSet> characterSets;
	private int[] numLvCleared = new int[4];
	private float topStageContainerSizeX,topStageContainerSizeY;
	private int btnWidth, btnHeight;
	private int gameOneClearedNum;
	private const int btnWidthFullHD = 384;
	private const int btnHeightFullHD = 230;
	private bool connected = false;
	private ScreenManager sm;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 30;

		initParams ();
		initGame ();

	}

	// Update is called once per frame
	void Update () {
//		handleGamesData ();
	}

	private void initParams(){
		if (sm == null) {
			sm = GameObject.FindObjectOfType(typeof(ScreenManager)) as ScreenManager;
		}
		allStageButtonSprite = Resources.LoadAll<Sprite> ("StageButton");
		topStageContainerSizeX = topStageContainer.GetComponent<RectTransform> ().rect.width;
		topStageContainerSizeY = topStageContainer.GetComponent<RectTransform> ().rect.height;	
		btnWidth = btnWidthFullHD;
		btnHeight = btnHeightFullHD;

		lvBtnContainerOne.GetComponent<GridLayoutGroup>().cellSize = new Vector2 (btnWidth, btnHeight);
		lvBtnContainerTwo.GetComponent<GridLayoutGroup>().cellSize = new Vector2 (btnWidth, btnHeight);
		lvBtnContainerThree.GetComponent<GridLayoutGroup> ().cellSize = new Vector2 (btnWidth, btnHeight);
		lvBtnContainerFour.GetComponent<GridLayoutGroup> ().cellSize = new Vector2 (btnWidth, btnHeight);
		gameOneClearedNum = lvOneClearStages ();

		// init player name
		playerNameText.text = sm.playerName;
	}
	private void initGame(){
		onClickGame1Stage1 ();
		onClickGame3Stage1 ();
		onClickGame4Stage1 ();
		onClickGame2Stage1 ();
	}

	/* 
	* This function is going to initialize the game data which download form class Communication
	* include : Game One to Four cleared level num
	* 			User Name , User Level
	* 			Unlock new game level
	*/			 
	private void handleGamesData(){
		if (games == null ) {
			GameObject goScreenManager = GameObject.Find ("ScreenManager");
			mComm = goScreenManager.GetComponent<Communication> ();
			ScreenManager sm = goScreenManager.GetComponent<ScreenManager> ();
			Debug.Log ("sm " + sm);
			Debug.Log ("mComm " + mComm);
	
			games = mComm.getGames ();
			characterSets = mComm.getCharacterSets ();
			if (games != null) {
				Debug.Log ("successe");
				int i = 0;
				foreach (Dictionary<int, GameLevel> game in games) {
//					GameLevel currentGl = game[i];
					Debug.Log("game size" + game.Count);
					GameLevel currentGL;

					Debug.Log ("hiiiiiiiiiiiiii" +game [0].characterSet_id);

					if (game.TryGetValue (i, out currentGL)) {
						Debug.Log ("success to get value ");
					} else
						Debug.Log ("fail to get value ");
					i++;
				}

//				foreach (CharacterSet cs in characterSets) {
//				}
//				Dictionary<int, String> testDict = new Dictionary<int, String>();
//				testDict.Add (1, "abc");
//				testDict.Add (2, "cde");
//				String temp;
//				if (testDict.TryGetValue (1, out temp)) {
//					Debug.Log (temp);
//				} else {
//					Debug.Log ("fail to test dict");
//				}

			}
		} 

	}
	private void onClickGame1Stage1(){
			Debug.Log ("called onClickGame1Stage1");
		gameOneStagebtn1.GetComponent<Image> ().sprite = allStageButtonSprite [1];
		gameOneStagebtn2.GetComponent<Image> ().sprite = allStageButtonSprite [2];
		gameOneStagebtn3.GetComponent<Image> ().sprite = allStageButtonSprite [4];

		stageBtnInit(lvBtnPrefabs, lvBtnContainerOne, btnPrefabLockG1, 1 , gameOneClearedNum, 1);

	}

	private void onClickGame1Stage2(){
		gameOneStagebtn1.GetComponent<Image> ().sprite = allStageButtonSprite [0];
		gameOneStagebtn2.GetComponent<Image> ().sprite = allStageButtonSprite [3];
		gameOneStagebtn3.GetComponent<Image> ().sprite = allStageButtonSprite [4];
		stageBtnInit(lvBtnPrefabs, lvBtnContainerOne, btnPrefabLockG1, 1  , gameOneClearedNum,2);

	}
	private void onClickGame1Stage3(){
		gameOneStagebtn1.GetComponent<Image> ().sprite = allStageButtonSprite [0];
		gameOneStagebtn2.GetComponent<Image> ().sprite = allStageButtonSprite [2];
		gameOneStagebtn3.GetComponent<Image> ().sprite = allStageButtonSprite [5];

		stageBtnInit(lvBtnPrefabs, lvBtnContainerOne,btnPrefabLockG1 , 1 , gameOneClearedNum, 3);

	}

	private void onClickGame2Stage1(){
		gameTwoStagebtn1.GetComponent<Image> ().sprite = allStageButtonSprite [7];
		gameTwoStagebtn2.GetComponent<Image> ().sprite = allStageButtonSprite [8];
		gameTwoStagebtn3.GetComponent<Image> ().sprite = allStageButtonSprite [10];
		stageBtnInit(lvBtnPrefabsG2, lvBtnContainerTwo, btnPrefabLockG2, 2 , lvTwoClearStages(), 1);


	}
	private void onClickGame2Stage2(){
		gameTwoStagebtn1.GetComponent<Image> ().sprite = allStageButtonSprite [6];
		gameTwoStagebtn2.GetComponent<Image> ().sprite = allStageButtonSprite [9];
		gameTwoStagebtn3.GetComponent<Image> ().sprite = allStageButtonSprite [10];
		stageBtnInit(lvBtnPrefabsG2, lvBtnContainerTwo, btnPrefabLockG2, 2 , lvTwoClearStages(), 2);


	}
	private void onClickGame2Stage3(){
		gameTwoStagebtn1.GetComponent<Image> ().sprite = allStageButtonSprite [6];
		gameTwoStagebtn2.GetComponent<Image> ().sprite = allStageButtonSprite [8];
		gameTwoStagebtn3.GetComponent<Image> ().sprite = allStageButtonSprite [11];
		stageBtnInit(lvBtnPrefabsG2, lvBtnContainerTwo, btnPrefabLockG2, 2 , lvTwoClearStages(), 3);

	}

	private void onClickGame3Stage1(){
		gameThreeStagebtn1.GetComponent<Image> ().sprite = allStageButtonSprite [13];
		gameThreeStagebtn2.GetComponent<Image> ().sprite = allStageButtonSprite [14];
		gameThreeStagebtn3.GetComponent<Image> ().sprite = allStageButtonSprite [16];
		stageBtnInit(lvBtnPrefabsG3, lvBtnContainerThree, btnPrefabLockG3, 3 , lvThreeClearStages(), 1);
	}

	private void onClickGame3Stage2(){
		gameThreeStagebtn1.GetComponent<Image> ().sprite = allStageButtonSprite [12];
		gameThreeStagebtn2.GetComponent<Image> ().sprite = allStageButtonSprite [15];
		gameThreeStagebtn3.GetComponent<Image> ().sprite = allStageButtonSprite [16];
		stageBtnInit(lvBtnPrefabsG3, lvBtnContainerThree, btnPrefabLockG3, 3 ,  lvThreeClearStages(), 2);

	}
	private void onClickGame3Stage3(){
		gameThreeStagebtn1.GetComponent<Image> ().sprite = allStageButtonSprite [12];
		gameThreeStagebtn2.GetComponent<Image> ().sprite = allStageButtonSprite [14];
		gameThreeStagebtn3.GetComponent<Image> ().sprite = allStageButtonSprite [17];
		stageBtnInit(lvBtnPrefabsG3, lvBtnContainerThree, btnPrefabLockG3, 3 , lvThreeClearStages(), 3);
	}

	private void onClickGame4Stage1(){
		gameFourStagebtn1.GetComponent<Image> ().sprite = allStageButtonSprite [19];
		gameFourStagebtn2.GetComponent<Image> ().sprite = allStageButtonSprite [20];
		gameFourStagebtn3.GetComponent<Image> ().sprite = allStageButtonSprite [22];
		stageBtnInit(lvBtnPrefabsG4, lvBtnContainerFour, btnPrefabLockG4, 4 ,lvfourClearStages(), 1);
	}
	private void onClickGame4Stage2(){
		gameFourStagebtn1.GetComponent<Image> ().sprite = allStageButtonSprite [18];
		gameFourStagebtn2.GetComponent<Image> ().sprite = allStageButtonSprite [21];
		gameFourStagebtn3.GetComponent<Image> ().sprite = allStageButtonSprite [22];
		stageBtnInit(lvBtnPrefabsG4, lvBtnContainerFour, btnPrefabLockG4, 4 ,lvfourClearStages(), 2);
	}
	private void onClickGame4Stage3(){
		gameFourStagebtn1.GetComponent<Image> ().sprite = allStageButtonSprite [18];
		gameFourStagebtn2.GetComponent<Image> ().sprite = allStageButtonSprite [20];
		gameFourStagebtn3.GetComponent<Image> ().sprite = allStageButtonSprite [23];
		stageBtnInit(lvBtnPrefabsG4, lvBtnContainerFour, btnPrefabLockG4, 4 , lvfourClearStages() , 3);
	}

	private void stageBtnInit(GameObject btnPrefab, GameObject lvBtnContainer ,GameObject btnLockPrefab,int game, int clearedNum, int stage){
		Debug.Log ("stage btn init");
		int startStageNum, endStageNum;
		foreach (Transform child in lvBtnContainer.transform) {
			GameObject.Destroy(child.gameObject);
		}
		switch (stage) {
		case 1:
			startStageNum = 1;
			endStageNum = 20;
			break;
		case 2:
			startStageNum = 21;
			endStageNum = 40;
			break;
		case 3:
			startStageNum = 41;
			endStageNum = 60;
			break;
		default:
			startStageNum = 0;
			endStageNum = 0;
			break;
		}

		for (int i = startStageNum; i<= endStageNum; i++) {
			if (i <= clearedNum) {
				GameObject newLevelButton = Instantiate (btnPrefab) as GameObject;
				newLevelButton.GetComponent<LayoutElement> ().minHeight = btnHeight;
				newLevelButton.GetComponent<LayoutElement> ().minWidth = btnWidth;
				newLevelButton.GetComponentInChildren<Text> ().text = (i).ToString ();
				newLevelButton.name = i.ToString();
				newLevelButton.transform.SetParent (lvBtnContainer.transform, false);
				newLevelButton.GetComponent<Button> ().onClick.AddListener (() => {
					onClickStageButton (game);
				});
			} else {
				GameObject newLevelButton = Instantiate (btnLockPrefab) as GameObject;
				newLevelButton.GetComponent<LayoutElement> ().minHeight = btnHeight;
				newLevelButton.GetComponent<LayoutElement> ().minWidth = btnWidth;
				newLevelButton.transform.SetParent (lvBtnContainer.transform, false);
			}
		}

	}
	private int lvOneClearStages(){
		return 34;
	}

	private int lvTwoClearStages(){
//		return numLvCleared[1];
		return 10;
	}
	private int lvThreeClearStages(){
//		return numLvCleared[2];
		return 11;
	}
	private int lvfourClearStages(){
		return 9;
//		return numLvCleared[3];
	}

	//initialize playerprefs
	private void onClickStageButton(int game){
		Debug.Log ("onClickStageButton");
		Debug.Log ("game " + game);

		PlayerPrefs.SetInt ("GameType", game);
		PlayerPrefs.SetInt ("GameLevel", Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name));
		switch (game) {

		case INDEX_GAME_1:
			sm.levelBtnSelected("PlayingScene");
			break;
		case INDEX_GAME_2:
			sm.levelBtnSelected("PlayingScene2");
			break;
		case INDEX_GAME_3:
			sm.levelBtnSelected ("PlayingScene3");
			break;
		case INDEX_GAME_4:
			sm.levelBtnSelected ("PlayingScene4");
			break;
		default:
			break;

		}
	}
}
