using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadingManger : MonoBehaviour {
	public Image fadingImage;
	private const float FADE_IN_OUT_TIME = 1f;
	public bool startFadeIn, startFadeOut;
	// Use this for initialization
	void Start () {
//		fadingImage.gameObject.SetActive (true);
	}

	
	void Update () {
		if (startFadeIn) {
			Debug.Log ("start Fade In");
			fadingImage.CrossFadeAlpha (0, FADE_IN_OUT_TIME, false);

		}
		if(startFadeOut)
			fadingImage.CrossFadeAlpha (1, FADE_IN_OUT_TIME, false);
	
	}
	public void processFadeIn(){
		fadingImage.gameObject.SetActive (true);
		startFadeIn = true;

	}

	public void processFadeOut(){
		startFadeOut = true;
	}
}
