using UnityEngine;
using System.Collections;

/**
 * Handles mouse clicks in the Demo scenes.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_ClickButton : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject callbackReceiver;

	[Header("Config")]
	public string callbackName;
	public Color normalColor = new Color32(128, 128, 128, 128);
	public Color hoverColor = new Color32(128, 128, 128, 128);

	private Rect collisionRect;

	void Start () {
		collisionRect = GetComponent<GUITexture> ().GetScreenRect (Camera.main);
	}

	void Update () {
		if (collisionRect.Contains (Input.mousePosition)) {
			GetComponent<GUITexture> ().color = hoverColor;

			if (Input.GetMouseButtonDown (0)) {
				OnClick ();
			}
		} else {
			GetComponent<GUITexture>().color = normalColor;
		}
	}

	private void OnClick() {
		callbackReceiver.SendMessage (callbackName);
	}
}
