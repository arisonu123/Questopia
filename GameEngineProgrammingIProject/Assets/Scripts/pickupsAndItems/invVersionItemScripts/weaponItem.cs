using UnityEngine;
using System.Collections;

[System.Serializable]
public class weaponItem {
    [Header("Item information")]
    [SerializeField]
    private string name;
    [SerializeField]
    private GameObject weaponPrefab;

    /// <summary>
    /// Uses/equips the weapon item
    /// </summary>
    public void usePickup()
    {
      
        Toolbox.player.equipWeapon(weaponPrefab);
    }

    /// <summary>
    /// Gets the name of this weapon
    /// </summary>
    /// <returns>The name of this weapon item</returns>
    public string getItemName()
    {
        return name;
    }

    /// <summary>
    /// Sets info/data for this weapon item
    /// </summary>
    /// <param name="itemName">The weapon item's name</param>
    /// <param name="weapon">The weapon prefab associated with this weaponItem</param>
    public void setData(string itemName,GameObject weapon)
    {
        this.name = itemName;
        this.weaponPrefab = weapon;
    }
}
