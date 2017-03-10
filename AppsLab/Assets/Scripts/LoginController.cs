using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour {

	public Camera mainCamera;
	public Transform panelParent;
	public Transform[] panels;
	private float originPPX;
	public GameObject UIContainer;
	private Transform cameraTransform;
	private Transform cameraDesiredLookAt;
	private const float CAMERA_TRANSITION_SPEED = 0.5f;
	private bool moveLeft = false;
	private bool moveRight = false;
	private bool backMoveLeft = false;
	private bool backMoveRight = false;
	private ScreenManager sm;

	private string username = "";
	private string password = "";
	private bool remember = false;

	private  void Start () {
		Application.targetFrameRate = 30;
		backMoveRight = false;
		backMoveLeft = false;
		if (sm == null) {
			sm = GameObject.FindObjectOfType(typeof(ScreenManager)) as ScreenManager;
		}
	}

	// Update is called once per frame
	private void Update () {
		detectBackBtn ();
	}
		
	public void loginPage (){
		iTween.MoveBy (UIContainer, new Vector3 (Screen.width,0,0), 1f);
		backMoveRight = true;
	}

	public void registerPage(){
		iTween.MoveBy (UIContainer, new Vector3 (-Screen.width,0,0), 1f);
		backMoveLeft = true;
	}

	public void enterGame(string sceneName) { 
		Debug.Log(sceneName);
		username = panels [0].FindChild ("Account Name").GetComponent<InputField> ().text;
		password = panels [0].FindChild ("Account Password").GetComponent<InputField> ().text;
		sm.loginValid (username, password);
	}

	public void detectBackBtn(){

		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKey (KeyCode.Escape)) {
				Debug.Log ("back button clicked");
				if (backMoveRight) {
					iTween.MoveBy (UIContainer, new Vector3 (-Screen.width,0,0), 1f);
					backMoveRight = false;

				} 
				if(backMoveLeft){
					iTween.MoveBy (UIContainer, new Vector3 (Screen.width,0,0), 1f);
					backMoveLeft = false;
				}

			}
		}

	}
}

