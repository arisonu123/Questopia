using UnityEngine;
using System.Collections;

//from http://wiki.unity3d.com/index.php/Toolbox
public class Toolbox : Singleton<Toolbox>{

    protected Toolbox() { } // guarantee this will be always a singleton only - can't use the constructor!

    
    [Header("Managers")]
    [SerializeField]
    #pragma warning disable 649
    private GameManager gameManager;
    /// <summary>
    /// Returns the GameManager Instance
    /// </summary>
    public static GameManager GameManager { get { return Instance.gameManager; } }

    [SerializeField]
    private UIManager UIManager;
    /// <summary>
    /// Returns the UIManager Instance
    /// </summary>
    public static UIManager UI { get { return Instance.UIManager; } }

    [SerializeField]
    private inventoryManager inventoryManager;
    /// <summary>
    /// Returns the inventoryManager
    /// </summary>
    public static inventoryManager invManager { get { return Instance.inventoryManager; } }

    [SerializeField]
    private loadScenes loadingManager;
    /// <summary>
    /// Returns the loadingManager
    /// </summary>
    public static loadScenes loadManager { get { return Instance.loadingManager; } }

    [SerializeField]
    private questManager questManager;
    /// <summary>
    /// Returns the questManager
    /// </summary>
    public static questManager QuestManager { get { return Instance.questManager; } }
#pragma warning restore 649

    /// <summary>
    /// Gets/sets the player's playerScript
    /// </summary>
    /// <value>The playerScript</value>
    public static playerScript player
    {
        get;
        set;
    }

    /// <summary>
    /// Gets the current npc being interacted with
    /// </summary>
    /// <value>The current npc being interacted with</value>
    public static npcScript npcBeingInteractedWith
    {
        get;
        set;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    // (optional) allow runtime registration of global objects
    static public T RegisterComponent<T>() where T : Component
    {
        return Instance.GetOrAddComponent<T>();
    }
}
