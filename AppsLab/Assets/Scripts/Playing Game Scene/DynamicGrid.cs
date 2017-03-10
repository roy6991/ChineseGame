using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour {

	public int col, row;
	private const int btnWidth =  150;
	private const int btnHeight= 150;
	private const int spacingSize = 30;
	// Use this for initialization

	void Start () {
		RectTransform buttonMatrix = gameObject.GetComponent<RectTransform> ();
		GridLayoutGroup grid = gameObject.GetComponent<GridLayoutGroup> ();
		grid.cellSize = new Vector2 (btnWidth, btnHeight);
		grid.spacing = new Vector2 (spacingSize, spacingSize);
//		grid.cellSize = new Vector2 (buttonMatrix.rect.width / col, buttonMatrix.rect.height / row);
//		grid.cellSize = new Vector2 (btnWidth*col, btnWidth*row);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
