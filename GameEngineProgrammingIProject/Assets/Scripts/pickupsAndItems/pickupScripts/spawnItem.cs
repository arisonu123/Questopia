using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spawnItem : MonoBehaviour {
	[Header("Pickups to spawn")]
    [SerializeField]
	[Tooltip("List of pickups that may be spawned")]
    #pragma warning disable 649
    private List<pickup> pickups;
	[SerializeField]
	[Tooltip("The max number of spawns per a minute, nothing will spawn if a pickup is already there")]
	private int spawnsPerMinute;
    #pragma warning restore 649

    private float spawnTime;
	
	private pickup spawnedPickup;
	private int randPickup;

	private void Awake(){
	  spawnTime = 0;
	}
	private void Update(){
	  if(spawnedPickup==null && Time.time>spawnTime){
		  randPickup=UnityEngine.Random.Range(0,pickups.Count-1);
		  Vector3 spawnPos = new Vector3 (transform.position.x, pickups [randPickup].transform.position.y+transform.position.y, transform.position.z);
		  spawnedPickup = Instantiate(pickups[randPickup],spawnPos,pickups[randPickup].transform.rotation ) as pickup;
		  spawnedPickup.transform.parent = gameObject.transform;
		  spawnedPickup.setSpawner (this.gameObject.GetComponent<spawnItem>());

		  
	  }
	}

	/// <summary>
	/// Sets the new spawn time. Call this when pickups are destroyed
	/// </summary>
	public void setSpawnTime(){
		spawnTime=(Time.time+(60/spawnsPerMinute));
	}
}
