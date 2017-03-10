using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using System;


public class Communication : MonoBehaviour
{
	public List<Dictionary<int, GameLevel>> games;
	public Dictionary<int, CharacterSet> characterSets;
	private bool downloaded;
    // Use this for initialization

    void Start()
    {
		downloaded = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

	public bool isDownloadFinished(){
		downloaded =  getGameData(new Action<string>(processGameData));
		return downloaded;
	}
    public void processGameData(string result)
    {
        var N = JSONNode.Parse(result);
        //GameLevls with their level id
      	games = new List<Dictionary<int, GameLevel>>();
        characterSets = new Dictionary<int, CharacterSet>();


        var i = 0;
        foreach (var game in N["games"].Childs)
        {
            //foreach game
            games.Add(new Dictionary<int, GameLevel>());

            foreach (var gl in game["GameLevel"].Childs)
            {
                //foreach gamelevel
                try
                {
                    int gamelevel_id = Int32.Parse((string)gl["gamelevel_id"]);

                    //Skip Duplicate GameLevel
                    if (games[i].ContainsKey(gamelevel_id))
                        continue;

                    int level = Int32.Parse((string)gl["level"]);
                    double duration = Double.Parse((string)gl["duration"]);
                    int characterSet_id = Int32.Parse((string)gl["characterSet_id"]);
                    int numOfCol = Int32.Parse((string)gl["numOfCol"]);
                    int numOfRow = Int32.Parse((string)gl["numOfRow"]);
                    double text_disappear_time = Double.Parse((string)gl["text_disappear_time"]);
                    int decrease_percentage = Int32.Parse((string)gl["decrease_percentage"]);

                    GameLevel gamelevel = new GameLevel();
                    gamelevel.gamelevel_id = gamelevel_id;
                    gamelevel.level = level;
                    gamelevel.duration = duration;
                    gamelevel.characterSet_id = characterSet_id;
                    gamelevel.numOfCol = numOfCol;
                    gamelevel.numOfRow = numOfRow;
                    gamelevel.text_disappear_time = text_disappear_time;
                    gamelevel.decrease_percentage = decrease_percentage;

                    games[i][gamelevel_id] = gamelevel;
                }
                catch (Exception e)
                {
                    Debug.Log("Int parse failed in the following node:\n" + (string)gl);
                    continue;
                }
            }

            var j = 0;
            foreach (var cs in game["characterSet"].Childs)
            {
                //Each Set
                int setID = Int32.Parse((string)cs["characterSet_id"]);

                //Skip Set that has been read
                if (characterSets.ContainsKey(setID))
                    continue;

                CharacterSet charSet = new CharacterSet();
                charSet.setID = setID;

                foreach (var key in cs.Keys)
                {
                    if (key == "characterSet_id")
                        continue;
                    charSet.char_audioSet.Add((string)cs[key]["oneCharacter"], (string)cs[key]["audio_url"]);
                }

                characterSets.Add(setID, charSet);

            }


            i++;
        }

        var a = 333;

    }

    IEnumerator WaitForRequest(WWW www, Action<string> callback)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            if (callback != null)
                callback(www.text);
        }
        else
        {
            //http error handling
            Debug.LogError("WWW Error: " + www.error + "\n" + new System.Diagnostics.StackTrace().ToString());
            callback("Error");
        }
        Debug.Log(www.text);
    }

	public bool getGameData(Action<string> callback)
    {
		downloaded = false;
        string url = "http://chinesecharactergame.cloudapp.net/game";

        /*
      3  Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("email", loginEmail);
        data.Add("account_id", requestToken);
        data.Add("UDID", SystemInfo.deviceUniqueIdentifier);
        data.Add("time", DataStructure.getCurrentUTCString());
        

        fsData jsonData;
        new fsSerializer().TrySerialize<Dictionary<string, string>>(data, out jsonData);

        var encoding = new System.Text.UTF8Encoding();
        WWW www = new WWW(url, encoding.GetBytes(fsJsonPrinter.PrettyJson(jsonData)));
        */

        WWW www = new WWW(url);

        StartCoroutine(WaitForRequest(www, callback));
		return true;
    }

	public List<Dictionary<int, GameLevel>> getGames(){
		return games;
	}

	public Dictionary<int, CharacterSet> getCharacterSets(){
		return characterSets;
	}

}
