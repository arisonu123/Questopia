using UnityEngine;
using System.Collections;

[System.Serializable]
public class attackSpeedPickup : pickup {
	[Header("Attack Speed Pickup Settings")]
	[SerializeField]
    #pragma warning disable 649
    [Tooltip("The attackSpeed to add to the player's weapon's current speed")]
	private float attackSpeed;
	[SerializeField]
	[Tooltip("The duration that the buff will last upon being used by the player")]
	private float durationToLast;
    [SerializeField]
    [Tooltip("The max number allowed in a stack of this item in the inventory")]
    private int numStack;
#pragma warning restore 649

    /// <summary>
    /// When attack speed pickup is picked up/ obtained put it into inventory and deal with any spawner timers
    /// </summary>
    protected override void onPickUp()
	{
		
        bool placedItem = false;
        if (placedItem == false)//try to place item in inventory
        {
            for (int i = 0; i < inventorySlots.Count; i++)//check to see if item exists in inventory already
            {
                if (inventorySlots[i].hasItem!= false)
                {
                    if (inventorySlots[i].slotAttackSpeedItem != null)
                    {
                        if (inventorySlots[i].slotAttackSpeedItem.getItemName() == getPickupName())
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

        if (placedItem == false) { 
            for (int i = 0; i < inventorySlots.Count; i++)//check to see if empty slot is available for the attack speed Pickup
            {
                if (inventorySlots[i].hasItem == false)
                {
                    inventorySlots[i].slotAttackSpeedItem =new attackSpeedItem();
                    inventorySlots[i].slotAttackSpeedItem.setData(attackSpeed,durationToLast,this.getPickupName());
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
