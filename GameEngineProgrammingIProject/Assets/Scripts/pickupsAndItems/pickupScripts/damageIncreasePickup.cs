using UnityEngine;
using System.Collections;
using System;


public class damageIncreasePickup : pickup {
	[Header("Damage Increase Pickup Settings")]
	[SerializeField]
	[Tooltip("The damage to add to the player's weapon's current damage")]
    #pragma warning disable 649
    private float attackDamage;
	[SerializeField]
	[Tooltip("The duration that the buff will last upon being used by the player")]
	private float durationToLast;
    [SerializeField]
    [Tooltip("The max number allowed in a stack of this item in the inventory")]
    private int numStack;
#pragma warning restore 649


    /// <summary>
    /// When  damage increase pickup is picked up/ obtained put it into inventory and deal with any spawner timers
    /// </summary>
    protected override void onPickUp()
	{
        
        bool placedItem = false;

        //try to place item in inventory
        if (placedItem == false)
        {
            for (int i = 0; i < inventorySlots.Count; i++)//check to see if item exists in inventory already
            {
                if (inventorySlots[i].hasItem ==true)
                {
                    if (inventorySlots[i].slotDamageIncreaseItem != null)
                    {
                        if (inventorySlots[i].slotDamageIncreaseItem.getItemName() == getPickupName())
                        {
                            if (inventorySlots[i].stackNum < numStack)
                            {
                                inventorySlots[i].stackNum += 1;
                                placedItem = true;
                                break;
                            }

                        }
                    }
                }
            }
        }

        if (placedItem == false)
        {
            
             for (int i = 0; i < inventorySlots.Count; i++)//check to see if empty slot is available for damage increase pickup
             {
                 if (inventorySlots[i].hasItem == false)
                 {
                    inventorySlots[i].slotDamageIncreaseItem = new damageIncreaseItem();
                    inventorySlots[i].slotDamageIncreaseItem.setData(attackDamage,durationToLast,getPickupName());
                    inventorySlots[i].stackNum = 1;
                    inventorySlots[i].setImage(image);
                    inventorySlots[i].hasItem = true;
                    break;
                }
             }

         }

          

        
        base.onPickUp();
	}

   

  
}
