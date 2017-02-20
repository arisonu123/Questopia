using UnityEngine;
using System.Collections;

/**
 * Scales the game object over time from
 * a start scale to end scale.
 * Used in FrostBlast and Skewer.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_ScaleOverTime : MonoBehaviour {
	
	public float startSize = 1.0f;
	public float minSize = 1.0f;
	public float maxSize = 3.0f;
	
	public float speed = 2.0f;
	
	private Vector3 targetScale;
	private Vector3 baseScale;
	private float currentScale;
	
	void Start () {
		baseScale = transform.localScale;
		transform.localScale = baseScale * startSize;
		currentScale = startSize;
		targetScale = baseScale * startSize;
	}
	
	void Update () {
		transform.localScale = Vector3.Lerp (transform.localScale, targetScale, speed * Time.deltaTime);
		
		ChangeSize ();
	}
	
	public void ChangeSize() {
		currentScale++;
		
		currentScale = Mathf.Clamp (currentScale, minSize, maxSize + 1);
		
		targetScale = baseScale * currentScale;
	}
}
