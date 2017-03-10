using UnityEngine;
using System.Collections;
using SimpleJSON;

public class LoginForm : MonoBehaviour {

	private ScreenManager sm;
	public string url= "http://chinesecharactergame.cloudapp.net/";

	private bool error = false;
	private string username = "";

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
		// Upload to a cgi script
		WWW w = new WWW(url + "login", form);
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			print(w.error);
			onFail ();
		}
		else {
			print(w.text);
			var N = JSONNode.Parse (w.text);
			error = N ["error"].AsBool;
			if (error){
				onFail ();
			} else {
				username = N["user"] ["name"].Value;
				onSuccess ();
			}
		}
	}

	private void onSuccess (){
		sm.SendMessage ("loginResult", true);
		sm.AssignPlayerName (username);
		print (username);
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
}