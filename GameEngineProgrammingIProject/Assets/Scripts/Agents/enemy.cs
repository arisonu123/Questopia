using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent (typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class enemy : agent {



    #pragma warning disable 649
    [SerializeField]
    private GameObject defaultWeapon;



    [Header("Pickups to choose from to spawn on enemy death")]
    [SerializeField]
    [Tooltip("List of pickups that may be spawned")]
    private weightedObject[] pickups;
#pragma warning restore 649

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Vector3 desiredVelocity;

    private Coroutine traverseOffMeshLink;

    private const float closeEnoughSquared = 0.06f;


    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        equipWeapon(defaultWeapon);

       
    }


    private void Start()
    {
        Toolbox.UI.registerEnemy(this);
    }

    protected override void  HandleOnDie()
    {
        navMeshAgent.enabled = false;
        base.HandleOnDie();
      

    }

    protected override void onDieEnd()
    {
        if (pickups.Length > 0)
        {
            var randPickup = weightedObject.select(pickups);
            // int randPickup = UnityEngine.Random.Range(0, pickups.Length - 1);
            if (randPickup != null)
            {
                Vector3 spawnPos = new Vector3(transform.position.x, randPickup.transform.position.y + transform.position.y, transform.position.z);
                #pragma warning disable 168
                Instantiate(randPickup, spawnPos, randPickup.transform.rotation);
                #pragma warning restore 168
            }
        }
        StopAllCoroutines();
        base.onDieEnd();
    }

    protected override void Update()
    {
        base.Update();



        if (traverseOffMeshLink != null)
            return;

    
        if (Toolbox.player != null)
        {
            if (navMeshAgent.enabled)
            {
                if (traverseOffMeshLink == null)
                {
                    navMeshAgent.Resume();
                }
                navMeshAgent.SetDestination(Toolbox.player.transform.position);

            }

            if (navMeshAgent.isOnOffMeshLink)
            {

                traverseOffMeshLink = StartCoroutine(TraverseOffMeshLink());
            }

            else
            {
                desiredVelocity = Vector3.MoveTowards(desiredVelocity, navMeshAgent.desiredVelocity, navMeshAgent.acceleration * Time.deltaTime);
                var input = transform.InverseTransformDirection(desiredVelocity);
                animator.SetFloat("Horizontal", input.x);
                animator.SetFloat("Vertical", input.z);
                if (weaponEquipped != null)
                {
                    if (weaponEquipped.aiShouldAttack(Toolbox.player.transform.position) == true)
                    {


                        weaponEquipped.startAttack();

                    }
                    else
                    {
          
                        weaponEquipped.endAttack();
                    }
                }
            }
        }
        else
        {
            navMeshAgent.Stop();
          
        }

     
        

    }

    private void OnAnimatorMove()
    {
        if (navMeshAgent.updatePosition)
        {
            navMeshAgent.velocity = animator.velocity;
        }
        else
        {
            animator.ApplyBuiltinRootMotion();
        }
    }

    private IEnumerator TraverseOffMeshLink()
    {


        navMeshAgent.Stop();
        navMeshAgent.ActivateCurrentOffMeshLink(false);
        UnityEngine.AI.OffMeshLinkData link = navMeshAgent.currentOffMeshLinkData;
        int linkType = navMeshAgent.currentOffMeshLinkData.offMeshLink.area;

        navMeshAgent.updatePosition = false;
        int oldAvoidancePriority = navMeshAgent.avoidancePriority;
        navMeshAgent.avoidancePriority = 99;


        // Move to start node with Root Motion
        Vector3 offset;

        do
        {


            offset = link.startPos - transform.position;

            // Stay Grounded
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo, 2f, 1, QueryTriggerInteraction.Ignore))
            {
                transform.position = hitInfo.point;
            }
            // Line up with start node
            transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(offset, Vector3.up));

            // Move
            Vector3 input = transform.InverseTransformDirection(offset.normalized * navMeshAgent.speed);
            animator.SetFloat("Horizontal", input.x);
            animator.SetFloat("Vertical", input.z);


            yield return null;
        }
        while (offset.sqrMagnitude > closeEnoughSquared);



        animator.SetTrigger("off-mesh link");

        do
        {

            // Update Animator
            offset = link.endPos - transform.position;
            if (linkType == 3)
            {
                animator.SetBool("drop", true);

            }


            animator.SetFloat("off-mesh link height", offset.y);
            animator.SetFloat("off-mesh link distance", Vector3.ProjectOnPlane(offset, Vector3.up).magnitude);

            if ((animator.GetFloat("off-mesh link height") <= 1.1f && this.gameObject.name == "GoblinRanger Enemy(Clone)") || (animator.GetFloat("off-mesh link height") <= 1.8f && this.gameObject.name == "Girl_elf_Enemy(Clone)"))
            {
                animator.SetBool("top", true);
            }

            animator.applyRootMotion = (animator.GetFloat("off-mesh link root motion") == 1f);

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("climb start"))
            {
                setWeaponVisible(false);

            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("climb end"))
            {
                setWeaponVisible(true);
            }

            // Move and Rotate to face target

            transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(link.endPos - transform.position, Vector3.up));
            transform.position = Vector3.MoveTowards(transform.position, link.endPos, animator.GetFloat("off-mesh link speed") * Time.deltaTime);


            yield return null;
        }
        while (offset.sqrMagnitude > closeEnoughSquared);





        transform.position = link.endPos;
        navMeshAgent.avoidancePriority = oldAvoidancePriority;
        navMeshAgent.updatePosition = true;

        if (linkType == 3)
        {
            animator.SetBool("drop", false);

        }


        if (animator.GetBool("top") == true)
        {
            animator.SetBool("top", false);
        }

        if (navMeshAgent!=null) { 
        navMeshAgent.ActivateCurrentOffMeshLink(true);

        navMeshAgent.CompleteOffMeshLink();
        navMeshAgent.Resume();
        }
        animator.applyRootMotion = true;
       
        traverseOffMeshLink = null;

        yield return null;
    }

    
}
