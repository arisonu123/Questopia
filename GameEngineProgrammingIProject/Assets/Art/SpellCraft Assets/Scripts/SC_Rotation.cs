using UnityEngine;
using System.Collections;

/**
 * Rotate the game object at a set speed.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_Rotation : MonoBehaviour {
	
	public Vector3 speed = Vector3.up;
	public bool isRotating = true;

	void FixedUpdate () {
		if (isRotating) {
			transform.Rotate (speed);
		} else {
			transform.Rotate (new Vector3(0.0f, 0.0f, 0.0f));
		}
	}
}
