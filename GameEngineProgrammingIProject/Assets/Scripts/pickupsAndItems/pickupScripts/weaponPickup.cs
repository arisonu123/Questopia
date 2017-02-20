using UnityEngine;
using System.Collections;
using System;

public class weaponPickup : pickup{
    [SerializeField]
    [Tooltip("The weapon prefab of the weapon the player will equip when this pickup is used")]
    private GameObject weaponPrefab;

    /// <summary>
    /// When weapon pickup is picked up/ obtained put it into inventory and deal with any spawner timers
    /// </summary>
    protected override void onPickUp()
    {
       
       if (Toolbox.player.currentEquipped == null || !Toolbox.player.currentEquipped.weaponPickup.Equals(getPickupName()))//prevent duplicate weapons
       {
           bool alreadyHas = false;
           for (int i = 0; i < inventorySlots.Count; i++)//check to see if item exists in inventory already
           {
               if (inventorySlots[i].hasItem != false)
               {
                   if (inventorySlots[i].slotWeaponItem != null)
                   {
                       if (inventorySlots[i].slotWeaponItem.getItemName() == getPickupName())
                       {
                           alreadyHas = true;
                           break;

                       }
                   }

               }
           }

           if (alreadyHas == false)
           {
               for (int i = 0; i < inventorySlots.Count; i++)//check to see if empty slot is available for weapon
                {
                   if (inventorySlots[i].hasItem == false)
                   {

                       inventorySlots[i].slotWeaponItem = new weaponItem();
                       inventorySlots[i].slotWeaponItem.setData(getPickupName(),weaponPrefab);
                       inventorySlots[i].stackNum = 1;
                       inventorySlots[i].setImage(image);
                       inventorySlots[i].hasItem = true;
                       break;
                   }


                }
            }

        }
        

        base.onPickUp();
    }


   


}
