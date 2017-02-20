using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[DisallowMultipleComponent]
public class health : MonoBehaviour {
    [System.Serializable]
    public struct events
    {
        [Tooltip("Event raised whenever the health changes. Passes the change in health as an argument")]
        public FloatUnityEvent onChange;

        [Tooltip("Event raised whenever the health goes down. Passes the change in health as an argument")]
        public FloatUnityEvent onDamage;

        [Tooltip("Event raised whenever the health goes up. Passes the change in health as an argument")]
        public FloatUnityEvent onHeal;

        [Tooltip("Event raised whenever health reaches 50,25,10 percent")]
        public FloatUnityEvent onLowPercentHealth;

        [Tooltip("Event raised whenever the health becomes equal to 0.")]
        public UnityEvent onDie;
    }


    #pragma warning disable 649
    [Header("Settings")]
    [SerializeField,Tooltip("The initial health")]
    private float initialValue;
    [SerializeField,Tooltip("The max health")]
    private float maxValue;

    [Header("Events")]
    [Tooltip("A number of events raised by this component")]
    public events eventsList;
    #pragma warning restore 649

    private float currentValue;

	private bool justSpawned;

    private bool damageable = true;


    /// <summary>
    /// Gets/sets whether or not this object can take damage
    /// </summary>
    /// <value>Whether or not this object can take damage</value>
    public bool isDamageable
    {
        get { return damageable; }

        set { damageable = value; }
    }

    
	/// <summary>
	/// Returns the current health value
	/// </summary>
	/// <value>The current health value</value>
    public float currentVal
    {
        get { return currentValue; }
    }

	/// <summary>
	/// Returns the max health value
	/// </summary>
	/// <value>The max  health value.</value>
    public float maxVal
    {
        get { return maxValue; }
    }

	/// <summary>
	/// Returns the current percentage of health
	/// </summary>
	/// <value>The percentage of health</value>
    public float percent
    {
        get { return Mathf.Clamp(currentValue / maxValue,0,100); }
    }

    private void Awake()
    {
		justSpawned = true;
        modify(initialValue);
        
    }


    /// <summary>
    /// Modify the health by the specified change
    /// </summary>
    /// <param name="change">The amount to modify the health by</param>
    public void modify(float change)
    {
        if (!enabled||(Toolbox.npcBeingInteractedWith!=null&&this.gameObject.GetComponent<enemy>()==null))
        {
            return;
        }
        float newValue = Mathf.Clamp(currentValue+change, 0, maxValue);
		float actualChange = newValue-currentValue;

		if (justSpawned == false) {
			eventsList.onChange.Invoke (actualChange);
		}
        if (actualChange < 0f)
        {
            eventsList.onDamage.Invoke(actualChange);
        }
		else if(actualChange > 0f&&justSpawned==false)
        {
            eventsList.onHeal.Invoke(actualChange);
        }
        currentValue = newValue;
		
        if (currentValue == 0)
        {
            eventsList.onDie.Invoke();
            enabled = false;
        }

        //determine if onLowPercentHealth event should occur
        if (percent == 50)
        {
            eventsList.onLowPercentHealth.Invoke(actualChange);
        }
        else if (percent == 25)
        {
            eventsList.onLowPercentHealth.Invoke(actualChange);
        }
        else if(percent == 10)
        {
            eventsList.onLowPercentHealth.Invoke(actualChange);
        }
        else
        {
            //do nothing
        }

		if (justSpawned == true) {
			justSpawned = false;
		}
    }
}
