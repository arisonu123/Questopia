using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cats_Demo : MonoBehaviour {

	public Text camRotateCheck;
	public Text catText;

	public GameObject[] cats;
	public GameObject currCat;

	bool camRotate = true;
	int i = 0;


	// Use this for initialization
	void Start () {
		currCat = Instantiate (cats[0], new Vector3(0, 0, 0), new Quaternion(0, 120, 0, 0)) as GameObject;
		catText.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(camRotate)
			Camera.main.transform.RotateAround (Vector3.zero, Vector3.up, 40 * Time.deltaTime);
	}

	public void CamRotate(){
		if (camRotate) {
			camRotate = false;
			camRotateCheck.gameObject.SetActive (false);
		} 
		else {
			camRotate = true;
			camRotateCheck.gameObject.SetActive (true);
		}

	}

	void changeText(){
		switch (i) {
		case 0: catText.text = "Cat A"; break;
		case 1: catText.text = "Cat B"; break;
		case 2: catText.text = "Cat C"; break;
		case 3: catText.text = "Cat D"; break;
		case 4: catText.text = "Cat E"; break;
		case 5: catText.text = "Cat F"; break;
		case 6: catText.text = "Cat G"; break;
		}
	}

	public void NextChar(){
		Destroy (currCat);

		if (i == cats.Length-1)
			i = -1;
		i++;
		currCat = Instantiate (cats[i], new Vector3(0, 0, 0), new Quaternion(0, 120, 0, 0)) as GameObject;
		changeText ();
	}

	public void PrevChar(){
		Destroy (currCat);

		if (i == 0)
			i = cats.Length;
		i--;
		currCat = Instantiate (cats[i], new Vector3(0, 0, 0), new Quaternion(0, 120, 0, 0)) as GameObject;
		changeText ();
	}
}