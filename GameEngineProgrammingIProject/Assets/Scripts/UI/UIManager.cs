using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {
#pragma warning disable 649
    [SerializeField]
    [Header("Player")]
    private healthUI playerHealthBar;
    [SerializeField]
    [Tooltip("This is the weapon Icon Image on the UI")]
    private Image weaponIcon;
    [SerializeField]
    [Tooltip("This is the default weapon Icon image/sprite")]
    private Sprite defaultIcon;
    [SerializeField]
    private Text livesLabel;
    [SerializeField]
    private string livesFormat = "♥ x {0}";

    [SerializeField]
    [Header("Enemies")]
    private healthUI enemyHealthBarPrefab;
    [SerializeField]
    private Transform enemyHealthBarContainer;

    
    [Header("Windows")]
    [SerializeField]
    [Tooltip("The pause window")]
    private Transform pauseWindow;
    [SerializeField]
    [Tooltip("The game over window")]
    private Transform gameOverWindow;
    [SerializeField]
    [Tooltip("The player's inventory UI window")]
    private GameObject inventory;

    [Header("Button Sounds and Audio")]
    [Tooltip("This is the sound that plays any time a button is clicked")]
    [SerializeField]
    private AudioClip buttonSound;
    [SerializeField]
    [Tooltip("This audio source is used to get the proper output settings for the button sound")]
    private AudioSource audioSource;

#pragma warning restore 649

    [SerializeField]
    List<depthUI> enemyHealthBars = new List<depthUI>();

    /// <summary>
    /// Emptys the list of current enemy health bars
    /// </summary>
    public void clearHealthBars()
    {
        enemyHealthBars.Clear();
    }

    /// <summary>
    /// Sets the weaponIcon's image back to the default image
    /// </summary>
    public void setDefaultWepImage()
    {
        weaponIcon.sprite = defaultIcon;
    }

    /// <summary>
    /// Sets the weaponIcon image
    /// </summary>
    /// <value>The weaponIcon's Image</value>
    public Image icon
    {
        set { weaponIcon = value; }
    }

    /// <summary>
    /// Activates/Deactivates player stats
    /// </summary>
    public void turnOnPlayerStats()
    {
        if (!Toolbox.loadManager.isLoading)
        {
            playerHealthBar.gameObject.SetActive(Toolbox.GameManager.gameRunning);
            weaponIcon.gameObject.SetActive(Toolbox.GameManager.gameRunning);
            livesLabel.gameObject.SetActive(Toolbox.GameManager.gameRunning);
        }
        else
        {
            playerHealthBar.gameObject.SetActive(!Toolbox.loadManager.isLoading);
            weaponIcon.gameObject.SetActive(!Toolbox.loadManager.isLoading);
            livesLabel.gameObject.SetActive(!Toolbox.loadManager.isLoading);
        }
    }

    /// <summary>
    /// Registers the player's health for its health bar
    /// </summary>
    /// <param name="player">The player's playerScript</param>
    public void registerPlayer(playerScript player)
    {
        playerHealthBar.register(player.healthScript);
    }

    /// <summary>
    /// Registers the enemy's health for its health bar
    /// </summary>
    /// <param name="enemy">The enemy's enemy script</param>
    public void registerEnemy(enemy enemy)
    {
        var healthBar=Instantiate(enemyHealthBarPrefab, enemyHealthBarContainer) as healthUI;
        healthBar.register(enemy.healthScript);
        healthBar.enemyRenderer=enemy.gameObject.GetComponentInChildren<Renderer>();
   
    }

    /// <summary>
    /// Register's the dummy's health for its health bar
    /// </summary>
    /// <param name="dummy">The dummy's dummyBehavior script</param>
    public void registerDummy(dummyBehavior dummy)
    {
        var healthBar = Instantiate(enemyHealthBarPrefab, enemyHealthBarContainer) as healthUI;
        healthBar.register(dummy.healthScript);
        healthBar.enemyRenderer = dummy.gameObject.GetComponentInChildren<Renderer>();


    }

    /// <summary>
    /// Activates/Deacivates the pause window. Also activates/deactivates inventory usage
    /// </summary>
    public void activatePauseWindow()
    {
        pauseWindow.gameObject.SetActive(Toolbox.GameManager.Paused);
        Button[] invButtons =inventory.gameObject.GetComponentsInChildren<Button>();
        foreach(Button button in invButtons)
        {
            button.enabled = !Toolbox.GameManager.Paused;
        }
        Button[] choiceButtons = Toolbox.QuestManager.choiceBoxObject.GetComponentsInChildren<Button>();
        foreach(Button button in choiceButtons)
        {
            button.enabled = !Toolbox.GameManager.Paused;
        }
    }

    /// <summary>
    /// activates the game over window
    /// </summary>
    public void activateGameOverWindow()
    {
        gameOverWindow.gameObject.SetActive(Toolbox.GameManager.isGameOver);
    }

    /// <summary>
    /// Gets the list of enemy health bars
    /// </summary>
    /// <value> the list of enemy health bars</value>
    public List<depthUI> enemyBars
    {
        get { return enemyHealthBars; }
    }

    /// <summary>
    /// Gets the player's inventory gameObject
    /// </summary>
    /// <value>The player's inventory gameObject</value>
    public GameObject inv
    {
        get { return inventory; }
    }

    /// <summary>
    /// Plays the button click sound at location of the button
    /// </summary>
    public void playButtonSound()
    {
        GameObject tempAudio = new GameObject("One Shot Audio"); // create the temporary game object for audio
        tempAudio.transform.position = EventSystem.current.currentSelectedGameObject.transform.position; 
        AudioSource aSource = tempAudio.AddComponent<AudioSource>();
        aSource.clip = buttonSound; 
        aSource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
        aSource.Play();
        Destroy(tempAudio, buttonSound.length);
      
    }

    /// <summary>
    /// Add health bar to list of enemy health bars
    /// </summary>
    /// <param name="objectToAdd">Health bar to add</param>
    public void addToCanvas(depthUI objectToAdd)
    {
       enemyHealthBars.Add(objectToAdd);
    }

    private void sort()
    {//sort panels based on depth
        enemyHealthBars.Sort((x, y) => x.depthAmount.CompareTo(y.depthAmount));
        for (int i = 0; i < enemyHealthBars.Count; i++)
        {
            if (enemyHealthBars[i] != null)
            {
                enemyHealthBars[i].transform.SetSiblingIndex(i);
            }
        }

    }

    

    private void Update()
    {
        sort();
       

        
        if (Toolbox.player)
        {
            if (Toolbox.player.currentEquipped)
            {
                weaponIcon.sprite = Toolbox.player.currentEquipped.image;
            }
        }

        livesLabel.text = string.Format(livesFormat, Toolbox.GameManager.lives);
     
    }


}
