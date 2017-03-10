using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonColorChange : MonoBehaviour{
	private Color colorPressed;

	public void onButtonPressedColorChange(){
		colorPressed =  Color.black;
		Debug.Log ("find game object" +EventSystem.current.currentSelectedGameObject.name);
		GameObject.Find (EventSystem.current.currentSelectedGameObject.name).GetComponent<Image> ().color = new Color32(213,213,213,255);

	}
	public static void resetColorButton(GameObject buttonMatrix){
		foreach (Transform child in buttonMatrix.transform) {
			child.GetComponent<Image> ().color = new Color32(255,255,255,255);
		}
	}
} 
