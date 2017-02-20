using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class healthUI : MonoBehaviour
{
    #pragma warning disable 649
    [Header("Health bar parts")]
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Text healthText;
    #pragma warning restore 649

    private health Health;
   
    [Header("Health bar display, movement, and destroy settings")]
    [SerializeField]
    private bool trackTarget = false;
    [SerializeField]
    private Vector3 offset = Vector3.up * 2f;
    [SerializeField]
    private bool destroyWithTarget = false;

    [SerializeField]
    [Tooltip("This script should be on all enemy healthUI only, it adjusts the depth of the healthbar to avoid overlap and decides when a health bar should display")]
    private depthUI depthUIScript;
    [SerializeField]
    [Tooltip("This is the max distance away from the camera that healthBars will show up at")]
    private float maxDisplayDis;

    public Renderer selfRenderer;

    /// <summary>
    /// Gets the depthUIScript component attached to this healthUI
    /// </summary>
    /// <value>The depthUIScript component attached to this healthUI</value>
    public depthUI DepthUIScipt
    {
        get { return depthUIScript; }
    }

    /// <summary>
    /// Sets the Renderer that this healthUI script should keep track of for the purpose of deciding whether or not to render an enemy health bar
    /// </summary>
    public Renderer enemyRenderer
    {
        set { selfRenderer = value; }

        get { return selfRenderer; }
    }


    // Update is called once per frame
    private void Update()
    {
        if (Health)
        {
            //updating display
            if (healthSlider)
            {
                healthSlider.value = Health.percent;
            }
            if (healthText)
            {
                healthText.text = Health.currentVal.ToString() + "/" + Health.maxVal.ToString();
            }

            //Check whether or not health bar should be visibile and enable/disable the object and set the position if needed
            if (trackTarget)
            {

                Vector3 worldPos = selfRenderer.transform.parent.position + offset;
                var position = Camera.main.WorldToScreenPoint(worldPos);


                //get distance of UI from camera
                float distance = Vector3.Distance(worldPos, Camera.main.transform.position);

                depthUIScript.depthAmount = -distance;

                float alpha = maxDisplayDis - distance;
                Debug.Log("The alpha at this point: " + alpha);

                depthUIScript.SetAlpha(alpha);

                position.z = 0;
                transform.position = position;
              //  Toolbox.UI.activateHealthBars();


            }
        }

     
       
    }

    /// <summary>
    /// Register's this health script to the health bar/healthUI, makes sure it gets destroyed if destroyWithTarget is true
    /// </summary>
    /// <param name="health">The health script to register to this healthUI</param>
    public void register(health health)
    {
        this.Health = health;
        if(destroyWithTarget)
        {
            Health.eventsList.onDie.AddListener(HandleTargetDied);
        }
    }

    private void HandleTargetDied()
    {
        Toolbox.UI.enemyBars.Remove(gameObject.GetComponent<depthUI>());
        Destroy(gameObject);
    }
 
  
}
