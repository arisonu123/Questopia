using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public abstract class weapon : MonoBehaviour {
    [SerializeField]
    public enum animationType
    {
        None = 0,
        Longbow = 1,
        Melee = 2

    }

    [SerializeField]
    private animationType typeOfAnimation;

    [SerializeField]
    [Tooltip("Damage this weapon deals")]
    protected float damage;
    [SerializeField, Range(0, 180f)]
    protected float aiAttackAngle = 5f;
    [SerializeField, Range(1, 40)]
    [Tooltip("The range this weapon has")]
    protected float range = 15f;

    [SerializeField]
    [Header("Inventory settings")]
    [Tooltip("The name of the weapon,used for putting the weapon in inventory")]
    private string weaponName;
    [SerializeField]
    [Tooltip("The sprite image for this weapon to display as in the inventory")]
    private Sprite weaponInvImg;

    [Header("IK Settings")]
    [SerializeField]
    private Transform leftHandIKTarget;
    [SerializeField]
    private Transform leftElbowIKHint;
    [SerializeField]
    private Transform rightHandIKTarget;
    [SerializeField]
    private Transform rightElbowIKHint;
    [SerializeField]
    private Transform headLookAtPos;
#pragma warning restore 649

    /// <summary>
    /// Gets the left hand IK target for this weapon.
    /// </summary>
    /// <value>The left hand IK target for this weapon.</value>
    public Transform leftHandTarget
    {
        get { return leftHandIKTarget; }
    }

    /// <summary>
    /// Gets the left elbow IK hint for this weapon
    /// </summary>
    /// <value>The left elbow IK hint for this weapon.</value>
    public Transform leftElbowHint
    {
        get { return leftElbowIKHint; }
    }

    /// <summary>
    /// Gets the right hand IK target for this weapon.
    /// </summary>
    /// <value>The right hand IK target for this weapon.</value>
    public Transform rightHandTarget
    {
        get { return rightHandIKTarget; }
    }

    /// <summary>
    /// Gets the right elbow IK hint for this weapon
    /// </summary>
    /// <value>The right elbow IK hint for this weapon.</value>
    public Transform rightElbowHint
    {
        get { return rightElbowIKHint; }
    }

    /// <summary>
    /// Gets the head look at IK position for this weapon
    /// </summary>
    /// <value> The look at postion for the head's IK for this weapon</value>
    public Transform headLookPos
    {
        get { return headLookAtPos; }
    }

    /// <summary>
    /// Gets the kind of animation for this weapon
    /// </summary>
    /// <value>The kind of the animation for this weapon</value>
    public animationType animationKind
    {
        get
        {
            return (typeOfAnimation);
        }
    }

    /// <summary>
    /// Gets the image this weapon displays as when going in the inventory
    /// </summary>
    /// <value>The weapon's inventory image </value>
    public Sprite image
    {
        get
        {
            return (weaponInvImg);
        }
    }

    /// <summary>
    /// Returns the weapon's name as a string
    /// </summary>'
    /// <value>The weapon's name</value>
    public string weaponPickup
    {
        get
        {
            return weaponName;
        }
    }

	/// <summary>
	/// Called to start attack
	/// </summary>
    public abstract void startAttack();

	/// <summary>
	/// Called to end attack
	/// </summary>
    public abstract void endAttack();

	/// <summary>
	/// Sets the attack speed of the character's currently equipped weapon
	/// </summary>
	/// <param name="speed">The new attack speed for the currently equipped weapon</param>
	public abstract void setAttackSpeed(float speed);


	/// <summary>
	/// Gets the attack speed of the player's currenly equipped weapon
	/// </summary>
	/// <returns>The attack speed of the player's currently equipped weapon.</returns>
	public abstract float getAttackSpeed ();


	/// <summary>
	/// Sets the attack damage of the character's currently equipped weapon
	/// </summary>
	/// <param name="damage">The new attack damage for the currently equipped weapon</param>
	public abstract void setAttackDamage(float damageAmount);


	/// <summary>
	/// Gets the attack damage of the player's currently equipped weapon
	/// </summary>
	/// <returns>The attack damage of the player's currently equipped weapon</returns>
	public abstract float getAttackDamage ();

    public abstract bool aiShouldAttack(Vector3 target);
  
        
}
