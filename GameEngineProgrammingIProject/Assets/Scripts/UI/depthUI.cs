using UnityEngine;
using System.Collections;

public class depthUI : MonoBehaviour {
#pragma warning disable 649
    [SerializeField]
    private float depth;
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private float alphaValue=1f;

    private bool justInstantiated = true;

#pragma warning restore 649
    /// <summary>
    /// Gets/sets the depth
    /// </summary>
    /// <value>The depth</value>
    public float depthAmount
    {
        get { return depth; }

        set { depth = value; }
    }

    /// <summary>
    /// Gets whether or not this health bar was just instantiated
    /// </summary>
    /// <value>whether or not this health bar was just instantiated</value>
    public bool justCreated
    {
        get { return justInstantiated; }

        set { justInstantiated = value; }
    }

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Toolbox.UI.addToCanvas(this);
    }

    /// <summary>
    /// Gets the current alpha value of the health bar UI
    /// </summary>
    /// <value>the current alpha value of the health bar UI. Affects whether or not the health bar displays and is solid</value>
    public float getAlpha
    {
        get { return alphaValue; }
    }

    /// <summary>
    /// Sets the alpha value of the health bar game object
    /// </summary>
    /// <param name="alpha">The alpha value of the health bar game object</param>
    public void SetAlpha(float alpha)
    {//modify alpha values, disable if object is not visable
        Debug.Log("Alpha in setAlpha is: " + alpha);
        alphaValue = alpha;
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        canvasGroup.alpha = alpha;

      
        //  Toolbox.UI.activateHealthBars();

    }
}
