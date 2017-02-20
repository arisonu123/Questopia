using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class questManager : MonoBehaviour {
#pragma warning disable 649
    [SerializeField]
    private GameObject npcConversationBox;
    [SerializeField]
    private Text conversation;
    [SerializeField]
    private GameObject choiceBox;
    [SerializeField]
    private Text questUILabel;
    [SerializeField]
    private Text questUIList;

    private List<quest> currentQuests=new List<quest>();


    private int questsCompleted;

    [SerializeField]
    [Header("Quest settings")]
    private int totalQuests;
    [SerializeField]
    [Tooltip("The quest names to keep track of, should be the same as a quest associated with an NPC")]
    private List<string> questNames;
#pragma warning restore 649
    private bool[] accepted;
    private bool[] completed;

    private List<string> questRequirements=new List<string>();

    private void Start()
    {

        accepted = new bool[questNames.Count];
        completed = new bool[questNames.Count];
    }

    /// <summary>
    /// Gets the list of all pickedup quests in playthrough
    /// </summary>
    /// <value>the list of all pickedup quests in playthrough</value>
    public List<quest> currentQuestsList
    {
        get { return currentQuests; }
    }

    /// <summary>
    /// Gets the list of all currently pickedup quest requirements
    /// </summary>
    /// <value>the list of all currently pickedup quest requirements</value>
    public List<string> questRequirementsList
    {
        get { return questRequirements; }
    }
    /// <summary>
    /// Gets/sets the array of booleans that keep track of what quests have been accepted
    /// </summary>
    /// <value>the array of booleans that keep track what quests have been accepted</value>
    public bool[] acceptedBools
    {
        get { return accepted; }
        set { accepted = value; }
    }

    /// <summary>
    /// Gets/sets the array of booleans that keep track of what quests have been accepted
    /// </summary>
    /// <value>the array of booleans that keep track of what quests have been completed</value>
    public bool[] completedBools
    {
        get { return completed; }
        set { completed = value; }
    }

    /// <summary>
    /// sets the total number of completed quests
    /// </summary>
    /// <value>the total number of completed quests</value>
    public int numCompletedQuests
    {
        set { questsCompleted = value; }
    }
    /// <summary>
    /// activates the quest list UI
    /// </summary>
    public void activateQuestListUI()
    {
        if (!Toolbox.loadManager.isLoading)
        {
            questUILabel.gameObject.SetActive(Toolbox.GameManager.gameRunning);
            questUIList.gameObject.SetActive(Toolbox.GameManager.gameRunning);
        }
        else
        {
            questUILabel.gameObject.SetActive(!Toolbox.loadManager.isLoading);
            questUIList.gameObject.SetActive(!Toolbox.loadManager.isLoading);
        }
    }

    /// <summary>
    /// activates the npc conversation box UI
    /// </summary>
    public void activateNPCConversationBox()
    {

        npcConversationBox.SetActive(Toolbox.GameManager.gameRunning);
    }

    /// <summary>
    /// activate the npc choice box UI
    /// </summary>
    public void activateChoiceBox()
    {
        choiceBox.SetActive(Toolbox.GameManager.gameRunning);
    }

    /// <summary>
    /// Gets the choice box game object
    /// </summary>
    /// <value>The choice box game object</value>
    public GameObject choiceBoxObject
    {
        get { return choiceBox; }
    }

    /// <summary>
    /// Update display text for npc conversation UI
    /// </summary>
    /// <param name="npcChat">String to replace the current conversation text with</param>
    public void npcText(string npcChat)
    {
        conversation.text = npcChat;
    }

    /// <summary>
    /// Add quest to list, update UI
    /// </summary>
    /// <param name="questAccepted">Quest to add to list</param>
    public void addQuest(quest questAccepted)
    {
        currentQuests.Add(questAccepted);
        questRequirements.Add(questAccepted.getRequirements());
        questUIList.text = questUIList.text + " " + questAccepted.getRequirements();
      
    }

    /// <summary>
    /// Removes a quest from list of current quests,updates UI
    /// </summary>
    /// <param name="questToRemove">quest to remove</param>
    public void removeQuest(quest questToRemove)
    {
        Debug.Log(Toolbox.npcBeingInteractedWith);

        int index=-1;
        for(int i = 0; i < currentQuests.Count; i++)
        {
            if (currentQuests[i].getName == questToRemove.getName)
            {
                index = i;
            }
        }
        
        Debug.Log(index);
        if (index != -1)
        {
            currentQuests.RemoveAt(index);
            questRequirements.RemoveAt(index);
        }
        questUIList.text = "";
        foreach (string requirement in questRequirements)
        {
            questUIList.text = questUIList.text + requirement;
        }
        
    }

    /// <summary>
    /// Clears out/erases all requirements text
    /// </summary>
    public void clearRequirementsText()
    {
        questUIList.text = "";
    }
   

    /// <summary>
    /// Used when player hits the yes button to accept or hand in a quest
    /// </summary>
    public void accept()
    {
        int questNumber=-1;
        for (int i = 0; i < questNames.Count; i++)
        {
            if (Toolbox.npcBeingInteractedWith.getNPCQuest.getName == questNames[i])
            {
                questNumber = i;
                break;
            }
        }

        if (questNumber!=-1&&accepted[questNumber] == false)
        {
          
            addQuest(Toolbox.npcBeingInteractedWith.getNPCQuest);
            accepted[questNumber] = true;
            Toolbox.npcBeingInteractedWith.questTellObject.SetActive(false);
            choiceBox.SetActive(false);
            npcConversationBox.SetActive(false);
      
            Toolbox.npcBeingInteractedWith = null;
            
        }
        else if(questNumber != -1 && accepted[questNumber] == true)
        {
        
            //search through inventory, find items, remove items,make sure player has enough items
            bool[] hasNeededItem = new bool[Toolbox.npcBeingInteractedWith.getNPCQuest.requiredItems.Count];//number of items needed/being removed
            
            int numToRemoveStill = hasNeededItem.Length;
            while (numToRemoveStill != 0)
            {
                for (int i = 0; i < Toolbox.invManager.inventorySpots.Count; i++)
                {
                    for (int c = 0; c < Toolbox.npcBeingInteractedWith.getNPCQuest.requiredItems.Count; c++)
                    {
                        if (Toolbox.invManager.inventorySpots[i].getCurrentItemName() == Toolbox.npcBeingInteractedWith.getNPCQuest.requiredItems[c].name)
                        {
                            Toolbox.invManager.inventorySpots[i].handItemIn();
                            numToRemoveStill--;
                        }
                    }
                }
            }
            
            choiceBox.SetActive(false);
            npcText(Toolbox.npcBeingInteractedWith.completedQuestConvo);
            completed[questNumber] = true;
            questsCompleted += 1;
            Toolbox.npcBeingInteractedWith.obtainRewards();
            removeQuest(Toolbox.npcBeingInteractedWith.getNPCQuest);
            if (questsCompleted == totalQuests)
            {
                npcConversationBox.SetActive(false);
                Toolbox.npcBeingInteractedWith = null;
                Toolbox.GameManager.gameWon();
            }
          
        }
       
    }

    /// <summary>
    /// Cancel out of npc conversation/do not accept quest or hand in
    /// </summary>
    public void cancel()
    {
       
        if (choiceBox != null)
        {
            choiceBox.SetActive(false);
        }
        npcConversationBox.SetActive(false);
        Toolbox.player.healthScript.isDamageable = true;//make player able to take damage again
        Toolbox.npcBeingInteractedWith= null;

    }

    /// <summary>
    /// Interact with npc
    /// </summary>
    public void interact(GameObject npc)
    {
        
    
        Toolbox.npcBeingInteractedWith = npc.GetComponent<npcScript>();
        int questNumber = -1;
        for (int i = 0; i < questNames.Count; i++)
        {
            
            if (Toolbox.npcBeingInteractedWith.getNPCQuest.getName == questNames[i])
            {
                questNumber = i;
                break;
            }
        }

        if (questNumber!=-1&&accepted[questNumber] == false)
        {
        
            //display UI that has the give quest text,give option to accept
            Toolbox.player.healthScript.isDamageable = false;//make player invincible while talking to npc
            npcText(Toolbox.npcBeingInteractedWith.giveQuestConvo);//set npcText
            activateNPCConversationBox();//activate appropiate boxes
            activateChoiceBox();
            
        }
        else if (accepted[questNumber] == true && completed[questNumber]== false)
        {
        
            Toolbox.player.healthScript.isDamageable = false;//make player invincible while talking to npc

            npcText(Toolbox.npcBeingInteractedWith.notCompletedConvo);//set npcText
            activateNPCConversationBox();//activate appropiate boxes


            //check if player has required items to hand in
            bool[] hasNeededItem = new bool[Toolbox.npcBeingInteractedWith.getNPCQuest.requiredItems.Count];

            for (int i = 0; i < Toolbox.invManager.inventorySpots.Count; i++)
            {
                for (int c = 0; c < Toolbox.npcBeingInteractedWith.getNPCQuest.requiredItems.Count; c++)
                {
                   
                    if (Toolbox.invManager.inventorySpots[i].getCurrentItemName() == Toolbox.npcBeingInteractedWith.getNPCQuest.requiredItems[c].name)
                    {
                        int totalOfThisItemNeeded = 0 ;
                        for(int j = 0; j < Toolbox.npcBeingInteractedWith.getNPCQuest.requiredItems.Count; j++)//check for duplicates and make sure the player has enough
                        {
                            if (Toolbox.invManager.inventorySpots[i].getCurrentItemName() == Toolbox.npcBeingInteractedWith.getNPCQuest.requiredItems[j].name)
                            {
                                totalOfThisItemNeeded++;
                                
                            }
                            

                        }
                        if (totalOfThisItemNeeded <= Toolbox.invManager.inventorySpots[i].stackNum)
                        {
                            hasNeededItem[c] = true;
                        }
                    }
                }
            }
            bool allNeededItems = true;
            for (int j = 0; j < hasNeededItem.Length; j++)
            {
                if (hasNeededItem[j] == false)
                {
                    allNeededItems = false;
                    break;
                }
            }
            if (allNeededItems == true)
            {
                activateChoiceBox();

            }
            //display UI text, check if player has completed it or has the means to complete it, turn in/complete quest
        }
        else if (accepted[questNumber] == true && completed[questNumber] == true)
        {

            Toolbox.player.healthScript.isDamageable = false;//make player invincible while talking to npc

            npcText(Toolbox.npcBeingInteractedWith.questFinishedConvo);//set npcText
            activateNPCConversationBox();//activate appropiate boxes

            //display UI that gives text for completed
        }

    }

}
