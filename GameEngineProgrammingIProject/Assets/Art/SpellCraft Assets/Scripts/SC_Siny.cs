using UnityEngine;
using System.Collections;

/**
 * Scale object in a sin wave.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_Siny : MonoBehaviour {
	
	public float rate;
	public float midScale;
	public float ratio;
	private Vector3 scale;

	void Update () {
		float scaleComponent = midScale * Mathf.Pow (ratio, Mathf.Sin(Time.time * rate));
		for (int i = 0; i < 3; i++)
		{
			scale = scaleComponent * Vector3.one;
			transform.localScale = scale;
		}
	}
}
