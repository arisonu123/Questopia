using UnityEngine;
using System.Collections;
using System;

public class projectileWeapon : weapon {

#pragma warning disable 649
    [Header("Shoot settings")]
    [SerializeField]
    [Tooltip("The front position of the projectile weapon. It is the location ammo shoots from")]
    private Transform barrel;
    [SerializeField]
    [Tooltip("The projectile to be shot")]
    private projectile projectile;
    [SerializeField]
    [Tooltip("The velocity at which ammos leaves the barrel of the projectile weapon")]
    private float muzzleVelocity;
    [SerializeField]
    [Tooltip("The number of shots available to the player per a minute")]
    private float shotsPerMinute;

   
    private void Awake()
    {
        timeNextShotIsReady = Time.time;
    }

    private void Update()
    {
        if (Time.time > timeNextShotIsReady)
        {
            if (shooting)
            {
                var spawnedProjectile = Instantiate(projectile, barrel.position, barrel.rotation) as projectile;
                spawnedProjectile.setDamage(damage);
                spawnedProjectile.gameObject.layer = gameObject.layer;
                spawnedProjectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * muzzleVelocity, ForceMode.VelocityChange);
                timeNextShotIsReady += 60/shotsPerMinute;
            }
            else
            {
                timeNextShotIsReady = Time.time;
          
            }
        }
    }

    private bool shooting = false;
    private float timeNextShotIsReady;
 
    

    #region implemented members of weapon
	/// <summary>
	/// Starts the projectile weapon attack and plays appropiate animations
	/// </summary>
    public override void startAttack()
    {
        shooting = true;
	
    }

	/// <summary>
	/// Ends the projectile weapon attack.
	/// </summary>
    public override void endAttack()
    {
        shooting = false;
    }

	/// <summary>
	/// Sets the shooting speed for the currently equipped projectile weapon
	/// </summary>
	/// <param name="speed">The new shooting speed for the currently equipped projectile weapon</param>
	public override void setAttackSpeed(float speed){
		shotsPerMinute = speed;

	}

	/// <summary>
	/// Gets the shooting speed for the currently equipped projectile weapon
	/// </summary>
	/// <returns>The shooting speed for the currently equipped projectile weapon.</returns>
	public override float getAttackSpeed(){
		return shotsPerMinute;
	}

	/// <summary>
	/// Sets the attack damage for the currently equipped projectile weapon
	/// </summary>
	/// <param name="damage">The new attack damage for the currently equipped projectile weapon</param>
	public override void setAttackDamage(float damageAmount){
		damage = damageAmount;
	}

	/// <summary>
	/// Gets the attack damage for the currently equipped projectile weapon
	/// </summary>
	/// <returns>The attack damage for the currently equipped projectile weapon</returns>
	public override float getAttackDamage(){
		return damage;
	}


    /// <summary>
    /// Returns whether or not the enemy should attack with this projectile weapon
    /// </summary>
    /// <param name="target">Enemy target's transform positon</param>
    /// <returns></returns>
    public override bool aiShouldAttack(Vector3 target)
    {
        var weaponToTarget = target - barrel.position;

        if (weaponToTarget.sqrMagnitude > Mathf.Pow(range, 2f)){

            return false;
        }

        
        return Vector3.Angle(barrel.forward, weaponToTarget) < aiAttackAngle;

    }
    #endregion
}
