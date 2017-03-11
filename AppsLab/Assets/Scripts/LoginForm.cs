using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.Networking;

public class LoginForm : MonoBehaviour {

	private ScreenManager sm;
	public string url= "http://chinesecharactergame.cloudapp.net/";

	private bool error = false;
	private string username = "";
	private string sessionId;
	private Dictionary<string,string> headers = new Dictionary<string, string>();

	// Use this for initialization
	void Awake () {
		sm = FindObjectOfType<ScreenManager> ();
	}

	// for login

	public void submitForm (string email, string password, bool remember) {
		StartCoroutine(submitLoginForm(email, password, remember.ToString()));
	}

	IEnumerator submitLoginForm(string email, string password, string remember) {
		// Create a Web Form
		WWWForm form = new WWWForm();
		form.AddField("email", email);
		form.AddField ("password", password);
		form.AddField ("remember", remember);
		form.AddField ("api", "");


		WWW w = new WWW(url + "login", form);

		yield return w;
		//this.GetSessionId (w.responseHeaders);

		//Account.instance.setCookie(w.responseHeaders["Set-Cookie"]);
		//Account.instance.setCookie(w.responseHeaders["Set-Cookie"]);

		if (!string.IsNullOrEmpty(w.error)) {
			print(w.error);
			onFail ();
		}
		else {
			var N = JSONNode.Parse (w.text);
			error = N ["error"].AsBool;
			if (error){
				onFail ();
			} else {
				username = N["user"] ["name"].Value;

				onSuccess ();

				using (UnityWebRequest request = new UnityWebRequest(url+"stage","Get")) {
					yield return request.Send ();
					print (request.downloadHandler.text);
				}

			}
		}
	}

	private void onSuccess (){
		sm.SendMessage ("loginResult", true);
		sm.AssignPlayerName (username);


	}

	private void onFail (){
		sm.SendMessage ("loginResult", false);
	}

	// for logout

    public void LogoutRequest()
    {
        StartCoroutine(logout());
    }

    private IEnumerator logout()
    {
        WWW w = new WWW(url + "logout");
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            print(w.error);
        }
        else
        {
            print(w.text);
            var N = JSONNode.Parse(w.text);
            error = N["error"].AsBool;
            if (error)
            {
                //fail
            }
            else
            {
                // success
                sm.logout();
            }
        }
    }
	private void GetSessionId(Dictionary<string , string> responseHeaders){

        foreach(KeyValuePair<string , string> header in responseHeaders){

            Debug.Log(string.Format("{0} : {1}" , header.Key , header.Value));

            if(header.Key == "SET-COOKIE"){

                string[] cookies = header.Value.Split(';');
                for(int i = 0 ; i < cookies.Length ; i++){

                    if(cookies[i].Split('=')[0] == "PHPSESSID" && !this.headers.ContainsKey("COOKIE")){
                        this.sessionId = cookies[i];
                        this.headers.Add("COOKIE" , this.sessionId);
                        break;
                    }
                }
            }
        }
    }
}