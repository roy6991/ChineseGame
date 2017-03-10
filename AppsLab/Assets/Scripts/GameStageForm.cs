using UnityEngine;
using System.Collections;
using SimpleJSON;

public class GameStageForm : MonoBehaviour {

	private ScreenManager sm;
	public string url= "http://chinesecharactergame.cloudapp.net/";

	private GameStageData stageData;
	private bool error = false; 

	void Awake () {
		sm = FindObjectOfType<ScreenManager> ();
	}

	public void gameStageDataRequest() {
		StartCoroutine(getStageData());
	}

	private IEnumerator getStageData(){
		WWW w = new WWW(url + "stage");
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			print(w.error);
			//onFail ();
		}
		else {
			print(w.text);
			var N = JSONNode.Parse (w.text);
			error = N ["error"].AsBool;
			if (error){
				//onFail ();
			} else {
				print (N ["currentLevel"] [0].Value);
				int game1 = N ["currentLevel"] [0] ["level"].AsInt;
				int game2 = N ["currentLevel"] [1] ["level"].AsInt;
				int game3 = N ["currentLevel"] [2] ["level"].AsInt;
				int game4 = N ["currentLevel"] [3] ["level"].AsInt;
				stageData = new GameStageData (game1, game2, game3, game4);
				sm.SendMessage ("getStageDataResult",stageData);
				//onSuccess ();
			}
		}
	
	}
}
