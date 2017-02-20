using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(lifespan))]
public class spell : MonoBehaviour {
    private float damage;

    /// <summary>
    /// Sets the damage for the spell
    /// </summary>
    /// <param name="damage">Damage dealt on collision with a health.Use a positive number to deal damage.</param>
    public void setDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider collision)
    {
        var health = collision.transform.GetComponent<health>();

        if (health)
        {
            health.modify(-damage);
        }
       
    }
}

