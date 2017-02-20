using UnityEngine;
using System.Collections;

/**
 * SpellCraft examples helper class.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_Helper : MonoBehaviour {

	/**
	 * Tries to find the parent game object of the current
	 * game object using the specified tag.
	 * Returns null if not found.
	 * 
	 * @author Thanks to Friduric for this code block.
	 * @references http://answers.unity3d.com/questions/28581/traverse-up-the-hierarchy-to-find-first-parent-wit.html
	 */
	public static GameObject FindParentWithTag (GameObject childObject, string tag) {
		Transform t = childObject.transform;
		while (t.parent != null) {
			if (t.parent.tag == tag) {
				return t.parent.gameObject;
			}
			t = t.parent.transform;
		}
		return null; // Could not find a parent with given tag.
	}

	/**
	 * Tries to find the child game object of the current
	 * game object using the specified tag.
	 * Returns null if not found.
	 */
	public static GameObject FindChildWithTag (GameObject currentObject, string tag) {
		foreach (Transform child in currentObject.transform) {
			if (child.name == tag) {
				return child.gameObject;
			}
		}

		return null;
	}
}
