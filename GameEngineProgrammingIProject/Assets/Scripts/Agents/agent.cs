using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(health))]
public class agent : MonoBehaviour {
#pragma warning disable 649
    [Header("Projectile weapon settings")]
    [SerializeField]
    private Transform projectileWeaponContainer;
    [SerializeField]
    private float projectileWeaponAimSpeed = 1f;

    [Header("Melee weapon settings")]
    [SerializeField]
    private Transform meleeWeaponContainer;
    [SerializeField]
    private float meleeWeaponAimSpeed = 1f;

    [Header("Magic weapon settings")]
    [SerializeField]
    private Transform magicWeaponContainer;
    [SerializeField]
    private float magicWeaponAimSpeed = 1f;

    [Header("Blood Effect Settings")]
    [SerializeField]
    private Transform bloodContainer;
    [SerializeField]
    private GameObject leftBloodEff;
    [SerializeField]
    private GameObject rightBloodEff;

    [SerializeField]
    [Tooltip("The time after death in which the agent will despawn")]
    protected float despawnDelay = 3f;

    protected Animator ani;
    protected health Health;

    protected weapon weaponEquipped = null;


    /// <summary>
    /// Gets the animator.
    /// </summary>
    /// <value>Returns the animator attached to this agent.</value>
    public Animator animator
    {
        get { return ani; }
    }

    /// <summary>
	/// Gets the health script.
	/// </summary>
	/// <value>Returns the health script attached to this agent.</value>
    public health healthScript
    {
        get { return Health; }
    }

    /// <summary>
    /// Gets the currently equipped weapon
    /// </summary>
    /// <value>returns the currently equipped weapon</value>
    public weapon currentEquipped
    {
        get { return weaponEquipped; }
    }

    protected virtual void Awake()
    {
        ani = GetComponent<Animator>();
        Health = GetComponent<health>();

        //Events
        Health.eventsList.onDie.AddListener(HandleOnDie);
    }

    protected virtual void HandleOnDie()
    {
        unequipWeapon(weaponEquipped);
        Invoke("onDieEnd", despawnDelay);

    }

    protected virtual void onDieEnd()
    {
        if (this.gameObject == Toolbox.player)
        {
            Toolbox.player = null;
        }
        Destroy(gameObject);
    }
    protected virtual void Update()
    {
        if (projectileWeaponContainer)
        {
            projectileWeaponContainer.rotation = Quaternion.Slerp(projectileWeaponContainer.rotation, transform.rotation, projectileWeaponAimSpeed * Time.deltaTime);
        }

        if (meleeWeaponContainer)
        {
            meleeWeaponContainer.rotation = Quaternion.Slerp(meleeWeaponContainer.rotation, transform.rotation, meleeWeaponAimSpeed * Time.deltaTime);
        }

        if (magicWeaponContainer)
        {
            magicWeaponContainer.rotation = Quaternion.Slerp(magicWeaponContainer.rotation, transform.rotation, magicWeaponAimSpeed * Time.deltaTime);
        }
    }

    protected void setWeaponVisible(bool visible)
    {
        if (weaponEquipped)
        {
            weaponEquipped.gameObject.SetActive(visible);
            animator.SetInteger("weapon type", visible ? (int)weaponEquipped.animationKind : 0);
        }
    }

    private void OnAnimatorIK()
    {
        if (!weaponEquipped)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 0);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 0);
            animator.SetLookAtWeight(0);
            return;
        }

        if (weaponEquipped.isActiveAndEnabled && weaponEquipped.leftHandTarget)//left hand ik
        {

            animator.SetIKPosition(AvatarIKGoal.LeftHand, weaponEquipped.leftHandTarget.position);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, weaponEquipped.leftHandTarget.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }

        if(weaponEquipped.isActiveAndEnabled && weaponEquipped.leftElbowHint)//left elbow IK
        {

            animator.SetIKHintPosition(AvatarIKHint.LeftElbow, weaponEquipped.leftElbowHint.position);
            animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1f);

        }
        else
        {
            animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 0);
        }

        if(weaponEquipped.isActiveAndEnabled && weaponEquipped.rightHandTarget)//right hand ik
        {
            animator.SetIKPosition(AvatarIKGoal.RightHand, weaponEquipped.rightHandTarget.position);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotation(AvatarIKGoal.RightHand, weaponEquipped.rightHandTarget.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);

        }

        if (weaponEquipped.isActiveAndEnabled && weaponEquipped.rightElbowHint)//right elbow IK
        {

            animator.SetIKHintPosition(AvatarIKHint.LeftElbow, weaponEquipped.rightElbowHint.position);
            animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1f);

        }
        else
        {
            animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 0);
        }

        if(weaponEquipped.isActiveAndEnabled && weaponEquipped.headLookPos)//head ik
        {
            animator.SetLookAtPosition(weaponEquipped.headLookPos.position);
            animator.SetLookAtWeight(1f);
        }

        else
        {
            animator.SetLookAtWeight(0);
        }

     
    }

    /// <summary>
    /// unequips the weapon to the agent
    /// </summary>
    /// <param name="weapon">The weapon object to unequip</param>
    public void unequipWeapon(weapon weapon)
    {
        
        if (weapon != null)
        {
            Destroy(weapon.gameObject);
        }

        if (animator)
        {
            animator.SetInteger("weapon type", 0);
        }
    }

    /// <summary>
    /// Equips the weapon to the agent
    /// </summary>
    /// <param name="weaponToEquip">The weapon item's name to equip</param>
    public void equipWeapon(GameObject weaponToEquip)
    {

        var wepToEquip = weaponToEquip.GetComponent<weapon>();

        if (weaponEquipped != null)
        {
            unequipWeapon(weaponEquipped);

        }
        if (wepToEquip is projectileWeapon)
        {
            var wep = Instantiate(weaponToEquip, projectileWeaponContainer, false) as GameObject;
            weaponEquipped = wep.GetComponent<weapon>();
            wep.gameObject.layer = gameObject.layer;

        }
        else if (wepToEquip is magicWeapon)
        {
            var wep = Instantiate(weaponToEquip, magicWeaponContainer, false) as GameObject;
            weaponEquipped = wep.GetComponent<weapon>();
            wep.gameObject.layer = gameObject.layer;

        }

        else
        {
            //do nothing atm , may try to add more weapons later
        }

        if (weaponEquipped){
            animator.SetInteger("weapon type", (int)weaponEquipped.animationKind);
        }
    }


    /// <summary>
    /// Instantiates blood effects
    /// </summary>
    public void spawnBlood()
    {
        #pragma warning disable 168
        var bloodEffLeft = Instantiate(leftBloodEff, bloodContainer, false) as GameObject;
        var bloodEffRight = Instantiate(rightBloodEff, bloodContainer, false) as GameObject;
        #pragma warning restore 168
    }

}
