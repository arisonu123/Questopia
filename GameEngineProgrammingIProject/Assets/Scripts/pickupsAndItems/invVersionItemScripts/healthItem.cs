using UnityEngine;
using System.Collections;

[System.Serializable]
public class healthItem {

#pragma warning disable 649
    private int healthAmount;
    private int healthTick;
    private float timeBetweenTicks;
    [SerializeField]
    private string name;
#pragma warning restore 649

    /// <summary>
    /// Uses the health pickup/item 
    /// </summary>
    public void usePickup()
    {
        if (timeBetweenTicks == 0)
        {
            Toolbox.player.healthScript.modify(healthAmount);
        }
        else
        {
            Toolbox.player.gameObject.GetComponent<buffScript>().startHealthBuff(timeBetweenTicks, healthTick, healthAmount);
        }
    }
 
    /// <summary>
    /// Gets the name of the health item
    /// </summary>
    /// <returns>The name of the health item</returns>
    public string getItemName()
    {
        return name;
    }

    /// <summary>
    /// Sets the health item's information/data
    /// </summary>
    /// <param name="healthToHeal">The amount this item increases the player's health by</param>
    /// <param name="amountPerTick">The amount of health healed per a tick, should be 0 if heal is instance</param>
    /// <param name="itemName">The item's name</param>
    public void setData(int healthToHeal,int amountPerTick, float tickTimer,string itemName)
    {
        this.healthAmount = healthToHeal;
        this.healthTick = amountPerTick;
        this.timeBetweenTicks = tickTimer;
        this.name = itemName;

    }
}
