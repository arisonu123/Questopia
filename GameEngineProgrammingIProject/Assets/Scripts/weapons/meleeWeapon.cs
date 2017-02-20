using UnityEngine;
using System.Collections;

public class meleeWeapon : weapon {
	#pragma warning disable 649
	[Header("Attack settings")]
	[SerializeField]
    [Tooltip("The number of attacks available to the player per a minute")]
    private float attacksPerMinute;

	private void Awake()
	{
		timeNextAttackIsReady = Time.time;
	}

	private void Update()
	{
		if (Time.time > timeNextAttackIsReady)
		{
			if (attacking)
			{
				//var spawnedProjectile = Instantiate(projectile, barrel.position, barrel.rotation) as projectile;
				//spawnedProjectile.setDamage(damage);
				//spawnedProjectile.gameObject.layer = gameObject.layer;
				//spawnedProjectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * muzzleVelocity, ForceMode.VelocityChange);
				timeNextAttackIsReady += 60/attacksPerMinute;
			}
			else
			{
				timeNextAttackIsReady= Time.time;
			}
		}
	}
	private bool attacking = false;
	private float timeNextAttackIsReady;


	#region implemented members of weapon
	/// <summary>
	/// Starts the attack for the melee weapon and plays the appropiate animations
	/// </summary>
	public override void startAttack()
	{
	  attacking = true;
	  gameObject.GetComponent<Animator> ().SetTrigger ("attack");
	  //gameObject.transform.Find ("meleeWeaponLocation").gameObject.GetComponent<Animator>().SetTrigger("attack");
	 // gameObject.transform.Find("default").gameObject.GetComponent<Animator>().SetTrigger ("attack");
	  //Debug.Log (gameObject.GetComponentInParent<Animator> ().gameObject.name);

	}

	/// <summary>
	/// Ends the attack for the melee weapon
	/// </summary>
	public override void endAttack()
	{
		attacking = false;
	}

	/// <summary>
	/// Sets the attack speed for the currently equipped melee weapon
	/// </summary>
	/// <param name="speed">The new attack speed for the currently equipped melee weapon</param>
	public override void setAttackSpeed(float speed){
		attacksPerMinute = speed;

	}

	/// <summary>
	/// Gets the attack speed for the currently equipped melee weapon
	/// </summary>
	/// <returns>The attack speed for the currently equipped melee weapon.</returns>
	public override float getAttackSpeed(){
		return attacksPerMinute;
	}

	/// <summary>
	/// Sets the attack damage for the currently equipped melee weapon
	/// </summary>
	/// <param name="damage">The new attack damage for the currently equipped melee weapon</param>
	public override void setAttackDamage(float damageAmount){
		damage = damageAmount;
	}

	/// <summary>
	/// Gets the attack damage for the currently equipped melee weapon
	/// </summary>
	/// <returns>The attack damage for the currently equipped melee weapon</returns>
	public override float getAttackDamage(){
		return damage;
	}

    /// <summary>
    /// Returns whether or not the enemy should attack with this melee weapon
    /// </summary>
    /// <param name="target">Enemy target's transform positon</param>
    public override bool aiShouldAttack(Vector3 target)
    {
       
       return false;
        
        
    }
    #endregion
}
