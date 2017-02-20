using UnityEngine;
using System.Collections;
[System.Serializable]
public class attackSpeedItem {
#pragma warning disable 649
    private float attackSpeed;
    private float durationToLast;
    [SerializeField]
    private string name;
#pragma warning restore 649

    /// <summary>
    /// Uses the attack speed pickup/item 
    /// </summary>
    public void usePickup()
    {
        if (Toolbox.player.currentEquipped != null)
        {
            Toolbox.player.gameObject.GetComponent<buffScript>().startAttackSpeedBuff(durationToLast, Toolbox.player.currentEquipped.getAttackSpeed(), Toolbox.player.currentEquipped.getAttackSpeed() + attackSpeed);
        }

    }

    /// <summary>
    /// Gets the name of the attack speed item
    /// </summary>
    /// <returns>The name of the attack speed item</returns>
    public string getItemName()
    {
        return name;
    }


    /// <summary>
    /// Sets the attack speed item's information/data
    /// </summary>
    /// <param name="speed">The amount this item increases the player's currently equipped weapon's attack speed by</param>
    /// <param name="duration">The duration this buff lasts</param>
    /// <param name="itemName">The item's name</param>
    public void setData(float speed, float duration, string itemName)
    {
        this.attackSpeed = speed;
        this.durationToLast = duration;
        this.name = itemName;

    }



}
