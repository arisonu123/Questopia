using UnityEngine;
using System.Collections;

public class magicWeapon : weapon {
#pragma warning disable 649
    [Header("Attack settings")]
    [SerializeField]
    [Tooltip("The number of attack spells available to the player per a minute")]
    private float attacksPerMinute;
    [SerializeField]
    [Tooltip("The spell to be used for attacking")]
    private spell spell;

    private Vector3 spawnPos;

 

    private void Awake()
    {
        timeNextSpellIsReady = Time.time;
    }

    private void Update()
    {
        if (Time.time > timeNextSpellIsReady)
        {
            if (attacking)
            {
                var spellAttack = Instantiate(spell, spawnPos, spell.transform.rotation)as spell;
                spellAttack.setDamage(damage);
                spellAttack.gameObject.layer = gameObject.layer;
                timeNextSpellIsReady += 60 / attacksPerMinute;
            }
            else
            {
                timeNextSpellIsReady = Time.time;
            }
        }
    }
    private bool attacking = false;
    private float timeNextSpellIsReady;


    #region implemented members of weapon
    /// <summary>
    /// Starts the attack for the magic weapon if the player clicked within its attack range. Plays the appropiate animations
    /// </summary>
    public override void startAttack()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, range))
        {
                
                attacking = true;
                gameObject.GetComponent<Animator>().SetTrigger("attack");
                spawnPos = hit.point;
            
            

        }
       

    }

    /// <summary>
    /// Ends the attack for the magic weapon
    /// </summary>
    public override void endAttack()
    {
        attacking = false;
    }

    /// <summary>
    /// Sets the attack speed for the currently equipped magic weapon
    /// </summary>
    /// <param name="speed">The new attack speed for the currently equipped magic weapon</param>
    public override void setAttackSpeed(float speed)
    {
        attacksPerMinute = speed;

    }

    /// <summary>
    /// Gets the attack speed for the currently equipped magic weapon
    /// </summary>
    /// <returns>The attack speed for the currently equipped magic weapon.</returns>
    public override float getAttackSpeed()
    {
        return attacksPerMinute;
    }

    /// <summary>
    /// Sets the attack damage for the currently equipped magic weapon
    /// </summary>
    /// <param name="damage">The new attack damage for the currently equipped magic weapon</param>
    public override void setAttackDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    /// <summary>
    /// Gets the attack damage for the currently equipped magic weapon
    /// </summary>
    /// <returns>The attack damage for the currently equipped magic weapon</returns>
    public override float getAttackDamage()
    {
        return damage;
    }


    /// <summary>
    /// Returns whether or not the enemy should attack with this magic weapon
    /// </summary>
    /// <param name="target">Enemy target's transform positon</param>
    public override bool aiShouldAttack(Vector3 target)
    {
        /*var weaponToTarget = target - barrel.position;
        if (weaponToTarget.sqrMagnitude < Mathf.Pow(aiAttackRange, 2f))
        {
            return false;
        }
        return Vector3.Angle(barrel.forward, weaponToTarget) < aiAttackAngle;*/
        return false;
    }
    #endregion
}
