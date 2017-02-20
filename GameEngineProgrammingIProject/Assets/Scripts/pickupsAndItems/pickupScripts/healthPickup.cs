using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


public class healthPickup : pickup {
    [SerializeField]
    #pragma warning disable 649
	[Header("Health pickup settings")]
	[Tooltip("The total amount that is healed")]
    private int healthAmount;
	[SerializeField]
	[Tooltip("The amount per a tick,if you want the full amount instantly then you can leave this at 0")]
	private int healthTick;
	[SerializeField]
	[Tooltip("The time between health amount ticks ,if you want the full amount instantly then put 0")]
	private float timeBetweenTicks;
    [SerializeField]
    [Tooltip("The max number allowed in a stack of this item in the inventory")]
    private int numStack;
#pragma warning restore 649



    /// <summary>
    /// When health pickup is picked up/ obtained put it into inventory and deal with any spawner timers
    /// </summary>
    protected override void onPickUp()
    {
       
        bool placedItem = false;

        //try to place item in inventory
        if (placedItem == false)
        {
            for (int i = 0; i < inventorySlots.Count; i++)//check to see if item exists in inventory already
            {
                if (inventorySlots[i].hasItem !=false)
                {

                    if (inventorySlots[i].slotHealthItem != null)
                    {
                        if (inventorySlots[i].slotHealthItem.getItemName() == getPickupName())
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
            for (int i = 0; i < inventorySlots.Count; i++)//check to see if empty slot is available for the health pickup
            {
                if (inventorySlots[i].hasItem == false)
                {
                    inventorySlots[i].slotHealthItem = new healthItem();
                    inventorySlots[i].slotHealthItem.setData(healthAmount,healthTick,timeBetweenTicks,getPickupName());
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
