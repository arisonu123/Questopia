using UnityEngine;
using System.Collections;

/**
 * Makes the transform always face the camera.
 * Used for certain spells such as MassTeleportation, StarFall and DeathAura.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_Billboard : MonoBehaviour {
	
	private Transform myTransform;
	private Transform target;

	void Awake () {
		myTransform = this.transform;
		target = Camera.main.transform;
	}

	void LateUpdate () {
		myTransform.LookAt (target);
	}
}
