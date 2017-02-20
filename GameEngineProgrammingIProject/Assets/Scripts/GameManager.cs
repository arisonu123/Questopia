using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {
#pragma warning disable 649
    [Header("Player Settings")]
    [SerializeField]
    private playerScript playerPrefab;
    [SerializeField]
    private Transform playerSpawnPoint;

    [SerializeField]
    private float playerRespawnDelay = 3f;
    [SerializeField]
    private int playerLives = 3;
    private int maxLives;
    [SerializeField]
    [Header("Loading Settings")]
    [Tooltip("The game object to display while loading")]
    private GameObject loadBg;
    [SerializeField]
    private string mainMenuSceneName= "mainMenu";
    [SerializeField]
    private string victoryMenuSceneName = "victoryScreen";

    [Header("Camera Controller")]
    [SerializeField]
    private cameraController camController;

    private bool isGameRunning = false;

#pragma warning disable 414
    private bool talkingToNPC = false;
#pragma warning restore 414

#pragma warning restore 649

    private bool paused = false;

    private void Start()
    {
        maxLives = playerLives;
    }
    /// <summary>
    /// Returns whether or not the game is paused
    /// </summary>
    /// <value>Whether or not the game is paused</value>
    public bool Paused { get { return paused; } }

    /// <summary>
    /// Returns whether or not the actual game is in progress
    /// </summary>
    /// <value>whether or not the actual game is in progress</value>
    public bool gameRunning
    {
        get { return isGameRunning; }
    }




    /// <summary>
    /// Returns the loadScreen gameObject
    /// </summary>
    /// <value>The loadScreen gameObjet</value>
    public GameObject loadScreen
    {
        get { return loadBg; }
    }

    /// <summary>
    /// Gets/Sets the player's spawn point. This is the point the player returns to upon death, if they have lives remaining
    /// </summary>
    /// <value>The player spawn point</value>
    public Transform spawnPoint
    {
        get { return playerSpawnPoint; }

        set { playerSpawnPoint = value; }
    }

    /// <summary>
    /// Returns whether or not the game is over
    /// </summary>
    /// <value>Whether or not the game is over</value>
    public bool isGameOver
    {
        get; private set;
    }

    /// <summary>
    /// Gets the player's current lives count
    /// </summary>
    /// <value>The player's lives count</value>
    public int lives
    {
        get { return playerLives; }
    }

    /// <summary>
    /// Does all new game functions
    /// </summary>
    public void startGame()
    {
        isGameOver = false;
        isGameRunning = true;
        Toolbox.UI.activateGameOverWindow();
        Toolbox.UI.setDefaultWepImage();
        Toolbox.UI.clearHealthBars();
        spawnPlayer();
        playerLives = maxLives;
        Toolbox.UI.turnOnPlayerStats();
        Toolbox.QuestManager.activateQuestListUI();
        camController.gameStart();
    }

    private void spawnPlayer()
    {
        if (isGameRunning) { 
        Toolbox.player = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation) as playerScript;
        Toolbox.player.healthScript.eventsList.onDie.AddListener(HandlePlayerDeath);
        }
    }

    private void HandlePlayerDeath()
    {
        if (playerLives > 0)
        {
            Invoke("spawnPlayer", playerRespawnDelay);
            Toolbox.UI.setDefaultWepImage();
            playerLives--;
        }
        else
        {
            isGameOver = true;
            isGameRunning = false;
            bool[] acceptedTracking = Toolbox.QuestManager.acceptedBools;
            bool[] completedTracking = Toolbox.QuestManager.completedBools;
            for (int i = 0; i < acceptedTracking.Length; i++)
            {
                acceptedTracking[i] = false;
            }
            for (int i = 0; i < completedTracking.Length; i++)
            {
                completedTracking[i] = false;
            }
            Toolbox.QuestManager.acceptedBools = acceptedTracking;
            Toolbox.QuestManager.completedBools = completedTracking;
            Toolbox.QuestManager.numCompletedQuests = 0;
            Toolbox.QuestManager.currentQuestsList.Clear();
            Toolbox.QuestManager.questRequirementsList.Clear();
            Toolbox.QuestManager.clearRequirementsText();
            Toolbox.UI.setDefaultWepImage();
            foreach (itemInSlot item in Toolbox.invManager.inventorySpots)
            {
                item.clearSlot();
            }

            if (paused)
            {
                pause(false);
            }
            Toolbox.UI.activateGameOverWindow();
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && !isGameOver&&isGameRunning)
        {
            pause(!paused);
        }
    }

    /// <summary>
    /// Pauses/Unpauses the game
    /// </summary>
    /// <param name="paused">Whether or not to pause the game</param>
    public void pause(bool paused)
    {
        this.paused = paused;
        Toolbox.UI.activatePauseWindow();
        Time.timeScale= paused ? 0: 1f;
    }

    /// <summary>
    /// Quits the game and loads the main menu scene
    /// </summary>
    public void quitToMainMenu()
    {
        if (Toolbox.player)
        {
            Debug.Log("In here");
            Toolbox.player.gameObject.SetActive(false);
            Toolbox.player.CancelInvoke();
            Destroy(Toolbox.player.gameObject);
            Toolbox.player = null;
        }
        pause(false);
        isGameRunning = false;
        isGameOver = true;
        
      
        bool[] acceptedTracking = Toolbox.QuestManager.acceptedBools;
        bool[] completedTracking = Toolbox.QuestManager.completedBools;
        for (int i = 0; i < acceptedTracking.Length; i++)
        {
            acceptedTracking[i] = false;
        }
        for (int i = 0; i < completedTracking.Length; i++)
        {
            completedTracking[i] = false;
        }
        Toolbox.QuestManager.acceptedBools = acceptedTracking;
        Toolbox.QuestManager.completedBools = completedTracking;
        Toolbox.QuestManager.numCompletedQuests = 0;
        Toolbox.QuestManager.currentQuestsList.Clear();
        Toolbox.QuestManager.questRequirementsList.Clear();
        Toolbox.QuestManager.clearRequirementsText();
        Toolbox.UI.setDefaultWepImage();
        foreach (itemInSlot item in Toolbox.invManager.inventorySpots)
        {
            item.clearSlot();
        }
        Toolbox.loadManager.load(mainMenuSceneName);
        Toolbox.UI.turnOnPlayerStats();
        Toolbox.QuestManager.activateQuestListUI();
        Toolbox.QuestManager.activateChoiceBox();
        Toolbox.QuestManager.activateNPCConversationBox();
        foreach(itemInSlot item in Toolbox.invManager.inventorySpots)
        {
            item.clearSlot();
        }

    }


    /// <summary>
    /// Loads the victory screen when game is won
    /// </summary>
    public void gameWon()
    {
        pause(false);
        isGameRunning = false;
        isGameOver = true;
        bool[] acceptedTracking = Toolbox.QuestManager.acceptedBools;
        bool[] completedTracking = Toolbox.QuestManager.completedBools;
        for(int i = 0; i < acceptedTracking.Length; i++)
        {
            acceptedTracking[i] = false;
        }
        for(int i = 0; i < completedTracking.Length; i++)
        {
            completedTracking[i] = false;
        }
        Toolbox.QuestManager.acceptedBools = acceptedTracking;
        Toolbox.QuestManager.completedBools = completedTracking;
        Toolbox.QuestManager.numCompletedQuests = 0;
        Toolbox.QuestManager.currentQuestsList.Clear();
        Toolbox.QuestManager.questRequirementsList.Clear();
        Toolbox.QuestManager.clearRequirementsText();
        Toolbox.UI.setDefaultWepImage();
        Toolbox.QuestManager.activateQuestListUI();
        Toolbox.UI.turnOnPlayerStats();
        foreach (itemInSlot item in Toolbox.invManager.inventorySpots)
        {
            item.clearSlot();
        }

        Destroy(Toolbox.player.gameObject);
        Toolbox.loadManager.load(victoryMenuSceneName);
    }
}
