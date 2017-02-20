using UnityEngine;
using System.Collections;

public class npcScript : MonoBehaviour {


#pragma warning disable 649
    [Header("Has Quest Marker Gameobject")]
    [Tooltip("This is the gameObject with the sprite on it that only appears if an NPC has a quest the player has yet to accept")]
    [SerializeField]
    private GameObject questTell;
     
    [Header("Quest Settings")]
    [SerializeField]
    [TextArea(5, 10)]
    private string giveQuestText;

    [SerializeField]
    [TextArea(5, 10)]
    private string notCompletedQuestText;

    [SerializeField]
    [TextArea(5, 10)]
    private string completedQuestText;

    [SerializeField]
    [TextArea(5, 10)]
    private string questFinishedText;


    [SerializeField]
    [Header("Quest Information")]
    private quest questInfo;
#pragma warning restore 649
    /// <summary>
    /// Gets the current quest/quest requirements and rewards associated with this npc
    /// </summary>
    /// <value>the current quest associated with this npc</value>
    public quest getNPCQuest
    {
        get { return questInfo; }
    }

    /// <summary>
    /// Returns the give quest conversation string
    /// </summary>
    /// <value> the give quest conversation string</value>
    public string giveQuestConvo
    {
        get { return giveQuestText; }
    }

    /// <summary>
    /// Returns the not completed quest conversation string
    /// </summary>
    /// <value>the not completed quest conversation string</value>
    public string notCompletedConvo
    {
        get { return notCompletedQuestText; }
    }

    /// <summary>
    /// Returns the completed quest conversation string
    /// </summary>
    /// <value> the quest completed conversation string</value>
    public string completedQuestConvo
    {
        get { return completedQuestText; }
    }

    /// <summary>
    /// Returns the quest finished conversation string
    /// </summary>
    /// <value> the quest finished conversation string</value>
    public string questFinishedConvo
    {
        get { return questFinishedText; }
    }

    /// <summary>
    /// Gets the questTell gameobject
    /// </summary>
    /// <value>The questTell gameobject</value>
    public GameObject questTellObject
    {
        get { return questTell;}
    }

    private void Awake()
    {
        foreach(quest npcQuest in Toolbox.QuestManager.currentQuestsList)
        {
            if (npcQuest.getName == questInfo.getName)
            {
                questTell.SetActive(false);
            }
        }
      
    }


    /// <summary>
    /// Gives the rewards
    /// </summary>
    public void obtainRewards()
    {
        foreach (pickup item in questInfo.rewardsList)
        {
            Vector3 spawnPos = new Vector3(transform.position.x, item.transform.position.y + transform.position.y, transform.position.z);
            var spawnedPickup = Instantiate(item, spawnPos, item.transform.rotation) as pickup;
            spawnedPickup.transform.parent = gameObject.transform;
        }
    }
}
