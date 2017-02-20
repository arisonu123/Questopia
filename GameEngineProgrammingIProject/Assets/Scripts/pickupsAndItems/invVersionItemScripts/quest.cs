using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class quest {
#pragma warning disable 649
    [Header("Quest name")]
    [SerializeField]
    private string questName;
    [Header("Quest requirements")]
    [SerializeField]
    private List<pickup> items;

    [Header("Quest Rewards")]
    [SerializeField]
    private pickup[] rewards;
#pragma warning restore 649
    /// <summary>
    /// Gets the quest's name and requirements as a string
    /// </summary>
    /// <returns>The quest's name and requirements</returns>
    public string getRequirements()
    {
        string requirements = questName;
       
        if (items.Count > 0)
        {
            requirements = requirements + " Items needed : ";
            Dictionary<string, int> numItems = new Dictionary<string, int>();
            for (int i = 0; i < items.Count; i++)//get count of each item
            {
                if (numItems.ContainsKey(items[i].name))
                {
               
                    if (numItems[items[i].name] == items.Count)//only continue if all items have not been checked/accounted for
                    {
                   
                        break;
                    }
                }
                if (i >= 1)//check for duplicates
                {
                    for (int c = 0; c < items.Count; c++)
                    {

                        if (items[i].name == items[c].name)//increment number of times
                        {

                            if (numItems[items[i].name] != items.Count)//only continue if all items have not been checked/accounted for
                            {
                                numItems[items[i].name] += 1;
                            }
                            else
                            {
                                break;
                            }




                        }
                        else
                        {
                            if (c == items.Count)//add item to dictionary if no duplicates were found
                            {
                                numItems.Add(items[i].name, 1);
                            }
                        }
                        
                    }
                }
                else
                {
                    numItems.Add(items[i].name,1);
                }
            }

            foreach(KeyValuePair<string,int> itemCount in numItems)
            {
                requirements = requirements + itemCount.Key+" x"+ itemCount.Value;
            }
        }
        return requirements;
    }

    /// <summary>
    /// Returns the name of the quest
    /// </summary>
    /// <value>The name of the quest</value>
    public string getName
    {
        get { return questName; }
    }

    /// <summary>
    /// Gets the pickup rewards array
    /// </summary>
    /// <value>the pickup rewards array</value>
    public pickup[] rewardsList
    {
        get { return rewards; }
    }
    


    /// <summary>
    /// Gets the list of required items
    /// </summary>
    public List<pickup> requiredItems
    {
        get { return items; }
    }
}
