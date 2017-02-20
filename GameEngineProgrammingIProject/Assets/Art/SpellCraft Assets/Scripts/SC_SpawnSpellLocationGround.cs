using UnityEngine;
using System.Collections;

/**
 * Spawns the spell at the ground click location.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_SpawnSpellLocationGround : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject spellPrefab;

	public void SpawnSpell (RaycastHit hit) {
		Vector3 spellPosition = new Vector3 (hit.point.x, spellPrefab.transform.position.y, hit.point.z);

		// Set the position and parent of the spell
		GameObject spell = GameObject.Instantiate (spellPrefab);
		spell.transform.position = spellPosition;
		spell.transform.parent = transform;
	}
}
