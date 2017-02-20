using UnityEngine;
using System.Collections;

/**
 * Fade the TintColour property of the material
 * set at a speed.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_FadeOut : MonoBehaviour {
	
	public float fadeSpeed = 3.0f;
	
	private bool isDone = false;
	private Color materialColor;
	private Color newColor;
	private float alphaValue = 0.0f;
	
	private Renderer rend;
	
	void Awake () {
		this.rend = this.gameObject.GetComponent<Renderer> ();
	}
	
	void Start () {
		materialColor = this.rend.material.GetColor ("_TintColor");
		alphaValue = materialColor.a;
	}
	
	void Update () {
		if (!isDone) {
			alphaValue = alphaValue - Time.deltaTime/(fadeSpeed == 0 ? 1 : fadeSpeed);
			newColor = new Color(materialColor.r, materialColor.g, materialColor.b, alphaValue);
			rend.material.SetColor("_TintColor", newColor);
			isDone = alphaValue <= 0 ? true : false;
		}
	}
}

