using UnityEngine;
using System.Collections;

[System.Serializable]
public class damageIncreaseItem{
#pragma warning disable 649
    private float attackDamage;
    private float durationToLast;
#pragma warning restore 649
    [SerializeField]
    private string name;

    /// <summary>
    /// Uses the damage increase pickup/item 
    /// </summary>
    public void usePickup()
    {
        if (Toolbox.player.currentEquipped != null)
        {
            Toolbox.player.gameObject.GetComponent<buffScript>().startAttackDamageBuff(durationToLast, Toolbox.player.currentEquipped.getAttackDamage(), Toolbox.player.currentEquipped.getAttackDamage() + attackDamage);
        }

    }

    /// <summary>
    /// Gets the damage increase item's name
    /// </summary>
    /// <returns>The name of the damage increase item</returns>
    public string getItemName()
    {
        return name;
    }


    /// <summary>
    /// Sets the  damage increase item's information/data
    /// </summary>
    /// <param name="damage">The amount this item increases the player's currently equipped weapon's damage by</param>
    /// <param name="duration">The duration this buff lasts</param>
    /// <param name="itemName">The item's name</param>
    public void setData(float damage,float duration,string itemName)
    {
        this.attackDamage = damage;
        this.durationToLast = duration;
        this.name = itemName;

    }

}
