using UnityEngine;
using System.Collections;

/**
 * Destroy game object after duration.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_SpellDuration : MonoBehaviour {

	[Header("Config")]
	public float spellDuration;

	void Start () {
		Destroy (gameObject, spellDuration);
	}
}
