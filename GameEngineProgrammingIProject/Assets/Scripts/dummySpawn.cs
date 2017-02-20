using UnityEngine;
using System.Collections;

public class dummySpawn : MonoBehaviour {
	[Header("Spawn settings")]
	[SerializeField]
    #pragma warning disable 649
    private float respawnTimer;
	[SerializeField]
	private GameObject objectToSpawn;
    #pragma warning restore 649

    private float timeToSpawn;
	private GameObject spawnedObject;

	public void spawnAfterTime(){
		timeToSpawn=Time.time+respawnTimer;
	}

	private void Awake(){
		timeToSpawn = Time.time;
	}
	private void Update(){
		if (spawnedObject == null && Time.time > timeToSpawn) {

			spawnedObject = Instantiate (objectToSpawn, objectToSpawn.transform.position, objectToSpawn.transform.rotation) as GameObject;
			spawnedObject.transform.parent = this.gameObject.transform;


		} 
	}


}
