using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class pickup:MonoBehaviour{
#pragma warning disable 649
    [SerializeField]
    [Tooltip("The duration that this pickup lasts in the game world after being spawned")]
    public float lifeSpan;
    [SerializeField]
    [Tooltip("The image representing this item in the inventory")]
    private Sprite pickupImage;
    [SerializeField]
    public List<itemInSlot> inventorySlots;
    
    #pragma warning restore 649
    private const float bobHeight=0.03f;//bob height in meters
    private const float bobSpeed=0.1f;//bob speed multiplier
    private const float spinSpeed = 45f;//spin speed in degrees per a second


	private spawnItem spawner=null;
    [SerializeField]
    private string pickupName;

    [SerializeField]
    [Tooltip("If this is a weapon, set to true so the proper pickup name is not overwritten")]
    private bool isWeapon;

    protected bool invItem;



    /// <summary>
    /// Returns the image this pickup will be in inventory
    /// </summary>
    /// <value>The sprite image of this pickup, used in inventory</value>
    public Sprite image
    {
        get { return pickupImage;  }
    }

    

	/// <summary>
	/// Called when pickups are instantiated to keep track of spawn information. This is for the purpose of resetting the spawn timer upon destruction of current pickup
	/// </summary>
	/// <param name="spawner">The spawnItem script attached to the spawner that instantiates this gameObject</param>
	public void setSpawner(spawnItem spawnTransform){
		spawner=spawnTransform;
	}

    /// <summary>
    /// Called to compare inventory items with newly picked up items
    /// </summary>
    /// <returns>The name of the pickup</returns>
    public string getPickupName()
    {
        
        return pickupName;
    }

    protected void Awake()
    {
        
        if (invItem == false)
        {
            Invoke("destroy", lifeSpan);
        }

        if (isWeapon != true)
        {
            if (gameObject.name.IndexOf("(") != -1)
            {
                pickupName = gameObject.name.Substring(0, gameObject.name.IndexOf("("));
            }
            else
            {
                pickupName = gameObject.name;
            }
        }
        
        
       
    }

    protected void Start()
    {
        
        inventorySlots = Toolbox.invManager.inventorySpots;
     
    }

	// Update is called once per frame
	private void Update () {
        transform.Translate(Vector3.up * bobHeight * Mathf.Sin(Time.timeSinceLevelLoad * bobSpeed) * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider col)
    {
        playerScript player = col.GetComponent<playerScript>();
        if (player)
        {
            onPickUp();
        }
    }

    /// <summary>
    /// When pickup is picked up/ obtained put it into inventory and deal with any spawner timers
    /// </summary>
    protected virtual void onPickUp()
    {
        if (spawner != null)
        {
            spawner.setSpawnTime();
        }
        Destroy(gameObject);
    }

	private void destroy(){

        if (spawner != null)
        {
            spawner.setSpawnTime();
            Destroy(gameObject);
        }

	}


 

   
}
