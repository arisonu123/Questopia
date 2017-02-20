using UnityEngine;
using System.Collections;

/**
 * UI button script for toggly type buttons.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_ToggleButton : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject callbackReceiver;

	[Header("Config")]
	public string callbackName;
	public Texture normalTexture;
	public Color normalColor = new Color32(128, 128, 128, 128);
	public Color hoverColor = new Color32(128, 128, 128, 128);
	public Color disabledColor = new Color32(128, 128, 128, 48);

	private bool isToggled = true;
	private bool isHovered;
	private Rect collisionRect;
	private GUIText label;

	void Start () {
		collisionRect = GetComponent<GUITexture> ().GetScreenRect (Camera.main);
		label = GetComponentInChildren<GUIText> ();

		UpdateTexture ();
	}

	void Update () {
		if (collisionRect.Contains (Input.mousePosition)) {
			isHovered = true;
			if (Input.GetMouseButtonDown (0)) {
				OnClick ();
			}
		} else {
			isHovered = false;
			GetComponent<GUITexture>().color = normalColor;
		}

		UpdateTexture ();
	}

	private void OnClick () {
		isToggled = !isToggled;
		callbackReceiver.SendMessage (callbackName);
	}

	private void UpdateTexture () {
		Color newColor = isToggled ? normalColor : disabledColor;

		if (isHovered) {
			GetComponent<GUITexture>().color = hoverColor;
		} else {
			GetComponent<GUITexture>().texture = normalTexture;
			GetComponent<GUITexture> ().color = newColor;
		}

		if (label != null) {
			label.color = newColor * 1.75f;
		}
	}
}
