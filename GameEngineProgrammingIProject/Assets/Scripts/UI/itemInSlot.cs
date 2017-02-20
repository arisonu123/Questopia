using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class itemInSlot : MonoBehaviour {
    #pragma warning disable 649
    //pickups types
    [SerializeField]
    private bool isPickUpInSlot=false;
    [SerializeField]
    private healthItem healthPickupInSlot;
    [SerializeField]
    private damageIncreaseItem damageIncreasePickupInSlot;
    [SerializeField]
    private attackSpeedItem attackSpeedPickupInSlot;
    [SerializeField]
    private weaponItem weaponPickupInSlot;
    [SerializeField]
    private int numInSlot=0;

    [SerializeField]
    [Tooltip("The text object that displays the amount of this object")]
    private Text amount;


    [SerializeField]
    [Tooltip("The image object to be changed when items are picked up")]
    private Image itemImage;
    [SerializeField]
    [Tooltip("The default sprite image for this item slot. Shows when no item is in this slot")]
    private Sprite defaultImage;
    #pragma warning restore 649

    /// <summary>
    /// Gets/Sets whether or not a pickup is currently in this inventory slot
    /// </summary>
    ///<value>Whether or not a pickup is currently in this inventory slot</value>
    public bool hasItem
    {
        get { return isPickUpInSlot; }

        set { isPickUpInSlot = value; }
    }

    /// <summary>
    /// Gets/Sets the damage increase item currently in this inventory slot
    /// </summary>
    ///<value>The damage increase item in this inventory slot</value>
    public damageIncreaseItem slotDamageIncreaseItem
    {
        get { return damageIncreasePickupInSlot; }

        set { damageIncreasePickupInSlot = value; }
    }

    public healthItem slotHealthItem
    {
        get { return healthPickupInSlot; }

        set { healthPickupInSlot = value; }
    }

    /// <summary>
    /// Gets/Sets the attack speed item currently in this inventory slot
    /// </summary>
    ///<value>The attack speed item in this inventory slot</value>
    public attackSpeedItem slotAttackSpeedItem
    {
        get { return attackSpeedPickupInSlot; }

        set { attackSpeedPickupInSlot = value; }
    }

    /// <summary>
    /// Gets/Sets the weapon item currently in this inventory slot
    /// </summary>
    ///<value>The weapon item in this inventory slot</value>
    public weaponItem slotWeaponItem
    {
        get { return weaponPickupInSlot; }

        set { weaponPickupInSlot = value; }
    }
    /// <summary>
    /// Gets/Sets the number of this item in this slot. Also sets the text to the proper number
    /// </summary>
    /// <value>The number of this pickup in this slot</value>
    public int stackNum
    {
        get { return numInSlot; }

        set {
         
            numInSlot = value;

            if (amount == null)
            {
                amount = gameObject.GetComponentInChildren<Text>();
            }
       
            amount.text = numInSlot.ToString();
            
        }
    }


 


    /// <summary>
    /// Sets the image for the item in this inventory slot
    /// </summary>
    /// <param name="image">The image to set the item inventoy slot image to</param>
    public void setImage(Sprite image)
    {
        itemImage.sprite = image;
    }


    /// <summary>
    /// Uses the item currently in this inventory slot,used as OnClick for invSlot objects
    /// </summary>
    public void useItem()
    {
        if (isPickUpInSlot != false&&Toolbox.player&&Toolbox.player.enabled==true)
        {
            if(!healthPickupInSlot.getItemName().Equals(""))
            {
                if (Toolbox.player.gameObject.GetComponent<buffScript>().isHealthBuffGoing == false)
                {
                    healthPickupInSlot.usePickup();
               
                    numInSlot -= 1;
                    stackNum = numInSlot;
                    if (numInSlot == 0)
                    {
                        isPickUpInSlot = false;
                        setImage(defaultImage);
                        healthPickupInSlot.setData(0, 0, 0, "");
                     
                    }
                }
            }
            else if (!attackSpeedPickupInSlot.getItemName().Equals(""))
            {
                if (Toolbox.player.gameObject.GetComponent<buffScript>().isAtkSpeedBuffGoing == false)
                {
                    attackSpeedPickupInSlot.usePickup();
                
                    numInSlot -= 1;
                    stackNum = numInSlot;
                    if (numInSlot == 0)
                    {
                        isPickUpInSlot = false;
                        setImage(defaultImage);
                        attackSpeedPickupInSlot.setData(0, 0, "");
                        
                    }
                }
            }
            else if(!damageIncreasePickupInSlot.getItemName().Equals(""))
            {
                if (Toolbox.player.gameObject.GetComponent<buffScript>().isDamageBuffGoing == false)
                {
                    damageIncreasePickupInSlot.usePickup();


                    numInSlot -= 1;
                    stackNum = numInSlot;
                    if (numInSlot == 0)
                    {
                        isPickUpInSlot = false;
                        setImage(defaultImage);
                        damageIncreasePickupInSlot.setData(0, 0, "");

                    }

                }
            }
            else if(!weaponPickupInSlot.getItemName().Equals(""))
            {
              
                if (Toolbox.player.currentEquipped!=null&&Toolbox.player.currentEquipped.weaponPickup!="")//if player has a weapon equipped swap weapon and set proper sprite
                {
                    string oldPlayerWep = Toolbox.player.currentEquipped.weaponPickup;

                    setImage(Toolbox.player.currentEquipped.image);
                    if (Toolbox.player.gameObject.GetComponent<buffScript>().isDamageBuffGoing == true)
                    {
                        Toolbox.player.gameObject.GetComponent<buffScript>().isDamageBuffGoing = false;
                    }
                    if (Toolbox.player.gameObject.GetComponent<buffScript>().isAtkSpeedBuffGoing == true)
                    {
                        Toolbox.player.gameObject.GetComponent<buffScript>().isAtkSpeedBuffGoing = false;
                    }
                    weaponPickupInSlot.usePickup();
                    foreach (GameObject wep in Toolbox.invManager.weaponsPossible)
                    {
         
                        if (wep.gameObject.name == oldPlayerWep)
                        {
                            weaponPickupInSlot.setData(oldPlayerWep, wep);
                        }
                    }


                }
                else
                {
                    numInSlot = 0;
                    stackNum = numInSlot;
                    setImage(defaultImage);
                    isPickUpInSlot = false;
                    weaponPickupInSlot.usePickup();
                    weaponPickupInSlot.setData("",null);

                }
              
                
            }
        }
    }

    /// <summary>
    /// Returns the name of the item in this slot
    /// </summary>
    /// <returns>The name of the item in this slot</returns>
    public string getCurrentItemName()
    {
        if (isPickUpInSlot != false)
        {
            if (!healthPickupInSlot.getItemName().Equals(""))
            {
                return healthPickupInSlot.getItemName();
            }

            else if (!attackSpeedPickupInSlot.getItemName().Equals(""))
            {
                return attackSpeedPickupInSlot.getItemName();
            }
            else if (!damageIncreasePickupInSlot.getItemName().Equals(""))
            {
                return damageIncreasePickupInSlot.getItemName();
            }
            else if (!weaponPickupInSlot.getItemName().Equals(""))
            {
                return weaponPickupInSlot.getItemName();
            }
        }
        return null;
    }

    /// <summary>
    /// Called to handInItems and make sure they get properly removed from the inventory
    /// </summary>
    public void handItemIn()
    {

        numInSlot -= 1;
        if (numInSlot == 0)
        {
            if (!healthPickupInSlot.getItemName().Equals(""))
            {
                healthPickupInSlot.setData(0, 0, 0, "");
            }
            else if (!damageIncreasePickupInSlot.getItemName().Equals(""))
            {
                damageIncreasePickupInSlot.setData(0, 0, "");
            }
            else if (!attackSpeedPickupInSlot.getItemName().Equals(""))
            {
                attackSpeedPickupInSlot.setData(0, 0, "");
            }
            else if (!weaponPickupInSlot.getItemName().Equals(""))
            {
                weaponPickupInSlot.setData("",null);
            }

            setImage(defaultImage);
            isPickUpInSlot = false;
        }
    }

    /// <summary>
    /// Empty's out the slot and sets it to default
    /// </summary>
    public void clearSlot()
    {
        numInSlot = 0;
        amount.text = numInSlot.ToString();
        if (!healthPickupInSlot.getItemName().Equals(""))
        {
            healthPickupInSlot.setData(0, 0, 0, "");
        }
        else if (!damageIncreasePickupInSlot.getItemName().Equals(""))
        {
            damageIncreasePickupInSlot.setData(0, 0, "");
        }
        else if (!attackSpeedPickupInSlot.getItemName().Equals(""))
        {
            attackSpeedPickupInSlot.setData(0, 0, "");
        }
        else if (!weaponPickupInSlot.getItemName().Equals(""))
        {
            weaponPickupInSlot.setData("",null);
        }

        setImage(defaultImage);
        isPickUpInSlot = false;
    }
}
