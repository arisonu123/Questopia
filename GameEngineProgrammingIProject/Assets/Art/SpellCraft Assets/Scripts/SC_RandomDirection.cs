using UnityEngine;
using System.Collections;

/**
 * Random direction script.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_RandomDirection : MonoBehaviour {

	public Vector3 rotation = Vector3.forward;
	private Vector3 direction;
	
	void Start () {
		direction = new Vector3 (Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
		direction.Scale (rotation);
		transform.localEulerAngles = direction;
	}
}
