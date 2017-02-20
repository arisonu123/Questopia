using UnityEngine;
using System.Collections;

public class buffScript : MonoBehaviour {
    public bool hasHealthBuff;
    public bool hasAtkSpeedBuff;
    public bool hasDamageBuff;


    /// <summary>
    /// Returns whether or not a health buff is already going
    /// </summary>
    /// <value>Whether or not a health buff is active</value>
    public bool isHealthBuffGoing
    {
        get { return hasHealthBuff; }
    }

    /// <summary>
    /// Returns whether or not a damage buff is already going
    /// </summary>
    /// <value>Whether or not an attack speed buff is active</value>
    public bool isAtkSpeedBuffGoing
    {
        get { return hasAtkSpeedBuff; }

        set { hasAtkSpeedBuff = value; }
    }

    /// <summary>
    /// Returns whether or not a damage increase buff is already going
    /// </summary>
    /// <value>//Whether or not a damage increase buff is active</value>
    public bool isDamageBuffGoing
    {
        get { return hasDamageBuff; }

        set { hasDamageBuff = value; }
    }
	/// <summary>
	/// Call this function when the player uses a health pickup that heals over time
	/// </summary>
	/// <returns>IEnumerator which is the seconds for ticks</returns>
	/// <param name="tickTime">Time between ticks</param>
	/// <param name="healthPerTick">Health healed per tick.</param>
	/// <param name="healthToHeal">Total amount to heal,it goes down until it reaches 0 at the final tick.</param>
	public IEnumerator healthBuff(float tickTime,int healthPerTick,int healthToHeal){
        hasHealthBuff = true;
        Toolbox.player.healthScript.modify(healthPerTick);
		healthToHeal -= healthPerTick;
		while (healthToHeal > 0) {
            
			yield return new WaitForSeconds(tickTime);
            while (!Toolbox.player || Toolbox.player.gameObject.activeInHierarchy == false)
            {
        
                yield return new WaitForEndOfFrame();
            }
            Toolbox.player.healthScript.modify (healthPerTick);
			healthToHeal -= healthPerTick;
		}
        hasHealthBuff = false;
		yield return null;
	}

    /// <summary>
    /// Call this function when the player uses a attack speed buff
    /// </summary>
    /// <returns>IEnumerator which is the duration in seconds</returns>
    /// <param name="duration">Duration for the buff to last</param>
    /// <param name="orgionalSpeed">player's weapon's original attack speed</param>
    /// <param name="newSpeed">player's weapon's bufffed/new attack speed</param>
    public IEnumerator attackSpeedBuff(float duration, float originalSpeed, float newSpeed) {
        hasAtkSpeedBuff = true;
        if (Toolbox.player.currentEquipped != null)
        {
           
            weapon current = Toolbox.player.currentEquipped;
            current.setAttackSpeed(newSpeed);
            yield return new WaitForSeconds(duration);
            if (current != null)
            {//check if player is still using the same weapon when buff runs out
                current.setAttackSpeed(originalSpeed);
            }
        }
        hasAtkSpeedBuff = false;
        yield return null;
	}

	public IEnumerator attackDamageBuff(float duration,float originalDamage,float newDamage){
        hasDamageBuff = true;

        if (Toolbox.player.currentEquipped != null)
        {
           
            weapon current = Toolbox.player.currentEquipped;
            current.setAttackDamage(newDamage);
            yield return new WaitForSeconds(duration);
            if (current != null)
            {//check if player is still using the same weapon when buff runs out
                current.setAttackDamage(originalDamage);
            }
        }
        hasDamageBuff = false;
        yield return null;
	}


    /// <summary>
    /// activates a over-time health buff
    /// </summary>
    /// <param name="healthPerTick">The health healed per a tick</param>
    /// <param name="healthToHeal">The total heal to heal</param>
    public void startHealthBuff(float tickTime, int healthPerTick, int healthToHeal)
    {
        StartCoroutine(healthBuff(tickTime, healthPerTick, healthToHeal));
        
    }

    /// <summary>
    /// activate a damage increase buff
    /// </summary>
    /// <param name="duration">The duration the buff lasts</param>
    /// <param name="originalDamage">The player's orgional weapon damage</param>
    /// <param name="newDamage">The player's buffed weapon damage</param>
    public void startAttackDamageBuff(float duration, float originalDamage, float newDamage)
    {
        StartCoroutine(attackDamageBuff(duration, originalDamage, newDamage));
    }


    /// <summary>
    /// activate an attack speed buff
    /// </summary>
    /// <param name="duration">The duration of the buff</param>
    /// <param name="originalSpeed">The player's origonal weapon attack speed</param>
    /// <param name="newSpeed">The player's buffed attack speed</param>
    /// <param name="player">The player's playerScript</param>
    public void startAttackSpeedBuff(float duration, float originalSpeed, float newSpeed)
    {
        StartCoroutine(attackSpeedBuff(duration, originalSpeed, newSpeed));
    }
}
