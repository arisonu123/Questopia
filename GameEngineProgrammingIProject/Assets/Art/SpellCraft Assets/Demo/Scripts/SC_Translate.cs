using UnityEngine;
using System.Collections;

/**
 * Move object in a direction over time with a set speed.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_Translate : MonoBehaviour {

	[Header("Config")]
	public float speed = 30.0f;
	public Vector3 rotation = Vector3.forward;
	public Vector3 axis = Vector3.forward;
	public bool gravity;
	private Vector3 direction;
	
	void Start ()
	{
		direction = new Vector3 (Random.Range(0.0f,360.0f),Random.Range(0.0f,360.0f),Random.Range(0.0f,360.0f));
		direction.Scale (rotation);
		transform.localEulerAngles = direction;
	}

	void Update ()
	{
		transform.Translate (axis * speed * Time.deltaTime, Space.Self);
	}
}
