using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class playerScript : agent
{
    private static playerScript instance;

	private float fallTime;

    #pragma warning disable 649
   


    private bool[] filledInvSlots=new bool[6];

	[Header("Fall damage settings")]
	[SerializeField]
	[Tooltip("Determines at what rigidbody y velocity point the player is considered to be falling.")]
	[Range(-10,-1)]
	private float fallVelocity=-3.5f;
	[SerializeField]
	[Tooltip("The amount of damage taken for each frame spent falling. Must Be Positive")]
	private float damagePerFallingFrame=100;

    private bool canTalkToNpc=false;
    private GameObject npcBeingTalkedTo;
	
    /// <summary>
    /// Gets info on what slots in inventory are filled/empty
    /// </summary>
    /// <value>returns the current information on whether inventory slots are full or not</value>
    public bool[] slotsFilled
    {
        get { return filledInvSlots; }
    }

    
    [Header("Ladder Triggers")]
    [SerializeField, Tooltip("The top point of the ladder,a trigger that gets the player off the ladder")]
    private GameObject climbTop;
    [SerializeField, Tooltip("The bottom point of the ladder,a trigger that gets the player off the ladder")]
    private GameObject climbBot;
    #pragma warning restore 649

    public GameObject topLadder
    {
        set { climbTop = value; }

        get { return climbTop; }
    }

    public GameObject botLadder
    {
        set { climbBot = value; }

        get { return climbBot; }
    }

    private void Start()
    {   if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (SceneManager.GetActiveScene().name == "gameScene")
        {
            climbTop = GameObject.Find("LadderCombinationSample Long").transform.Find("topReached").transform.gameObject;
            climbTop.SetActive(false);
            climbBot = GameObject.Find("LadderCombinationSample Long").transform.Find("bottomReached").transform.gameObject;
            climbBot.SetActive(false);
        }
        Toolbox.UI.registerPlayer(this);
    }

    protected override void Awake()
    {
        base.Awake();
        Toolbox.player= this;
    }

    protected override void HandleOnDie()
    {
        base.HandleOnDie();

        instance = null;
        this.gameObject.GetComponent<buffScript>().StopAllCoroutines();
    }
    /// <summary>
    /// Runs every frame, getting player input and determining animations to be played
    /// </summary>
    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Interact")&&canTalkToNpc==true)
        {
            if (Toolbox.npcBeingInteractedWith != null)
            {

                Toolbox.QuestManager.cancel();

            }
            else if (Toolbox.npcBeingInteractedWith == null)
            {
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", 0);
                Toolbox.QuestManager.interact(npcBeingTalkedTo);
            }

        }

        if (Toolbox.GameManager.Paused||Toolbox.npcBeingInteractedWith !=null)
        {
            return;
        }
           
        //Movement
        if (animator.GetBool("climb") != true)
        {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            input = transform.InverseTransformDirection(input);
            animator.SetFloat("Horizontal", input.x);
            animator.SetFloat("Vertical", input.z);

           
            //Rotation
            Plane plane = new Plane(Vector3.up, transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                transform.LookAt(ray.GetPoint(distance), Vector3.up);
            }
            
        
			detectFalling ();
        }

        //actions
        if (weaponEquipped)
        {
            if (Input.GetButtonDown("Attack"))
            {
                weaponEquipped.startAttack();
            }
            if (Input.GetButtonUp("Attack"))
            {
                weaponEquipped.endAttack();
            }
        }
        //climbing feature
        if (animator.GetBool("climb") == true)
        {
            if (Input.GetButtonDown("Vertical"))
            {
                animator.SetFloat("Vertical", 0.1f);
            }
            if (Input.GetButtonDown("Horizontal"))
            {
                animator.SetFloat("Vertical", -0.1f);
            }

        }

        //inventory controls
        if (Input.GetButtonDown("Inventory"))
        {
            if (Toolbox.UI.inv.activeInHierarchy == true)
            {
                Toolbox.UI.inv.SetActive(false);
            }
            else
            {
                Toolbox.UI.inv.SetActive(true);
            }
        }

        


    }

    /// <summary>
    /// if player is in ladder collider and presses the 1 key they beging climbing and proper animations are played
    /// </summary>
    /// <param name="col">Collider that the player is currently in</param>
    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "ladder")
        {
            float inFrontOfDot = Vector3.Dot(transform.forward, (col.gameObject.transform.position - transform.position).normalized);//make sure player is facing the ladder
              if (Input.GetButton("Interact")&&inFrontOfDot>0.8)
            {
                Invoke("climbChecksOn", 1.5f);
                animator.SetBool("climb", true);
                animator.SetFloat("Vertical", 0);
                animator.SetFloat("Horizontal", 0);
                setWeaponVisible(false);
            }
          
        }
        if(col.gameObject.tag == "topRung")
        {
            float behindDot = Vector3.Dot(( transform.position-col.gameObject.transform.position).normalized,transform.forward);//make sure player is facing away from the ladder
            float underDot = Vector3.Dot(col.gameObject.transform.position.normalized, Vector3.up);
            if (Input.GetButton("Interact")&&underDot>0.5&&behindDot<=0)
             {
               Invoke("climbChecksOn", 1.5f);
               animator.SetBool("climb", true);
               animator.SetFloat("Vertical", 0);
               animator.SetFloat("Horizontal", 0);
               setWeaponVisible(false);
            }
                

        }


        //scene switch colliders
        if (col.gameObject.tag == "caveEntrance")
        {

            if (Input.GetButton("Interact")&& Toolbox.loadManager.isLoading==false)
            {
                Toolbox.loadManager.load("caveScene");
            }
        }
        if (col.gameObject.tag == "exit")
        {
            if (Input.GetButton("Interact") && Toolbox.loadManager.isLoading == false)
            {
                Toolbox.loadManager.load("gameScene");
            }
        }

      


    }

    /// <summary>
    /// when player enters special trigger, disable climbing and return to proper animations
    /// </summary>
    /// <param name="col">Collider being entered</param>
    private void OnTriggerEnter(Collider col)
    {
       
       if (animator.GetBool("climb") == true)
       {
         animator.SetBool("climb", false);
         setWeaponVisible(true);
         Invoke("climbChecksOff", 0.2f);
               
       }





    }

    private void OnTriggerStay(Collider col)
    {
        //npc interactions checks
        if (col.gameObject.tag == "NPC")
        {
            canTalkToNpc = true;
            npcBeingTalkedTo = col.gameObject;
           
        }
    }

    private void OnTriggerExit(Collider col)
    {
        //npc interactions checks
        if (col.gameObject.tag == "NPC")
        {
            canTalkToNpc = false;
            npcBeingTalkedTo = null;

        }
    }

    /// <summary>
    /// Turns climb checks on
    /// </summary>
    private void climbChecksOn()
    {
        climbTop.SetActive(true);
        climbBot.SetActive(true);
    }

    /// <summary>
    /// Turns climb checks off
    /// </summary>
    private void climbChecksOff()
    {
        climbTop.SetActive(false);
        climbBot.SetActive(false);
    }

   

  

	/// <summary>
	/// Ups the light intensity on the point light. Should be called by the onChange event in health
	/// </summary>
	public void upLightIntensity(){
		Light light = gameObject.GetComponentInChildren<Light> ();
		light.intensity = 3f;
		Invoke("downLightIntensity",0.5f);

	}

	private void downLightIntensity(){
		Light light = gameObject.GetComponentInChildren<Light> ();
		light.intensity = 0f;
	}

	private void detectFalling(){
		if (GetComponent<Rigidbody>().velocity.y <= fallVelocity) {

			fallTime += Time.deltaTime;
		}
		else{
			if (fallTime > 0) {//if fall time is greater than 0 player takes fall damage equal to fallTime/60
                if (healthScript.percent != 50 && healthScript.percent != 25 && healthScript.percent != 10)
                {
                    healthScript.eventsList.onLowPercentHealth.Invoke(healthScript.currentVal);
                }
				healthScript.modify ((int)(-(fallTime*damagePerFallingFrame)));
				fallTime = 0;
			}

		}
	}

   
}