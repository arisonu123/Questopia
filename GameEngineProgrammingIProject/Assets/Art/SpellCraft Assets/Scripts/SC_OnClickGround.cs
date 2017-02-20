using UnityEngine;
using System.Collections;

/**
 * Mouse Click on ground sends callback.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_OnClickGround : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject callbackReceiver;
	
	[Header("Config")]
	public string callbackName;

	private MeshCollider groundCollider;

	void Awake () {
		groundCollider = GameObject.FindWithTag ("Ground").GetComponent<MeshCollider> ();
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit = new RaycastHit ();
			if (groundCollider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 9999.0f)) {
				SendMessage (hit);
			}
		}
	}

	private void SendMessage (RaycastHit target) {
		callbackReceiver.SendMessage (callbackName, target);
	}
}
