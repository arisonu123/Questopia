using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class inventoryManager : MonoBehaviour {

    [SerializeField]
    [Tooltip("The inventory slots in player inventory. Must be gameObjects with the itemInSlot script attached")]
    #pragma warning disable 649
    private List<itemInSlot> inventorySlots;
    [SerializeField]
    [Tooltip("The list of all weapons the player may have in the inventory, used to swap weapons in the inventory properly")]
    private List<GameObject> possibleWeapons;
    #pragma warning restore 649 

    /// <summary>
    /// Gets the inventorySlots information
    /// </summary>
    /// <value>The list of inventory slots/gameObjects with the itemInSlot script attached</value>
    public List<itemInSlot> inventorySpots
    {
        get { return inventorySlots; }
    }

    public List<GameObject> weaponsPossible
    {
        get { return possibleWeapons;  }
    }

  

   
   
}
