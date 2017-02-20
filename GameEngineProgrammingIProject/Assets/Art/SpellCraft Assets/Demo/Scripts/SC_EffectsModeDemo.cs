using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/**
 * Demo script for the Effects Mode.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_EffectsModeDemo : MonoBehaviour {

	[Header("Prefabs")]
	public GUIText spellLabel;
	public GUIText spellIndexLabel;
	public Renderer groundRenderer;
	public Collider groundCollider;
	public GameObject cameraContainerPrefab;
	public GameObject uiObject;

	protected List<GameObject> spellExamples;
	protected int exampleIndex;
	protected GameObject currentSpellObject;
	protected Vector3 defaultCamPosition;
	protected Quaternion defaultCamRotation;
	protected bool slowMotion;
	protected bool uiActive = true;

	// OVERRIDE --------------------------------------------------

	void Awake () {
		spellExamples = new List<GameObject> ();
		int children = transform.childCount;

		for (int i = 0; i < children; i++) {
			GameObject child = transform.GetChild(i).gameObject;
			spellExamples.Add(child);
		}

		spellExamples.Sort (delegate(GameObject spell1, GameObject spell2) {
			return spell1.name.CompareTo(spell2.name);
		});

		defaultCamPosition = Camera.main.transform.position;
		defaultCamRotation = Camera.main.transform.rotation;

		UpdateUI ();
	}

	void Start () {
		// Spawn the first spell
		SpawnSpell ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			PreviousSpell();
			SpawnSpell ();
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			NextSpell();
			SpawnSpell ();
		}

		// Mouse scroll / Zoom In & Out
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll != 0.0f) {
			Camera.main.transform.Translate(Vector3.forward * (scroll < 0.0f ? -1.0f : 1.0f), Space.Self);
		}

		// Right click / Reset camera
		if (Input.GetMouseButtonDown(1)) {
			Camera.main.transform.position = defaultCamPosition;
			Camera.main.transform.rotation = defaultCamRotation;
		}

		// Toggle UI
		if (Input.GetKeyDown (KeyCode.H)) {
			uiActive = !uiActive;
			uiObject.SetActive(uiActive);
		}
	}

	// MESSAGES --------------------------------------------------

	void OnPreviousSpell () {
		PreviousSpell ();
		SpawnSpell ();
	}

	void OnNextSpell () {
		NextSpell ();
		SpawnSpell ();
	}

	void OnToggleGround () {
		groundRenderer.enabled = !groundRenderer.enabled;
	}

	void OnToggleRotation () {
		cameraContainerPrefab.GetComponent<SC_Rotation> ().isRotating = !cameraContainerPrefab.GetComponent<SC_Rotation> ().isRotating;
	}

	void OnToggleSlow () {
		slowMotion = !slowMotion;

		if (slowMotion) {
			Time.timeScale = 0.3f;
		} else {
			Time.timeScale = 1.0f;
		}
	}

	void OnEffectsMode () {
		Time.timeScale = 1.0f;
		Application.LoadLevel ("SpellCraft Effects Demo");
	}

	void OnExamplesMode () {
		Time.timeScale = 1.0f;
		Application.LoadLevel ("SpellCraft Examples Demo");
	}

	// UI --------------------------------------------------

	void UpdateUI () {
		spellLabel.text = spellExamples [exampleIndex].name;
		spellIndexLabel.text = string.Format ("{0}/{1}", (exampleIndex+1).ToString("00"), spellExamples.Count.ToString("00"));
	}

	// OTHER --------------------------------------------------

	void SpawnSpell () {
		Cursor.visible = true;
		Destroy (currentSpellObject);

		currentSpellObject = GameObject.Instantiate (spellExamples[exampleIndex]);
		currentSpellObject.SetActive (true);
	}

	void PreviousSpell () {
		exampleIndex--;

		if (exampleIndex < 0)
			exampleIndex = spellExamples.Count - 1;

		UpdateUI ();
	}

	void NextSpell () {
		exampleIndex++;

		if (exampleIndex >= spellExamples.Count)
			exampleIndex = 0;

		UpdateUI ();
	}
}
