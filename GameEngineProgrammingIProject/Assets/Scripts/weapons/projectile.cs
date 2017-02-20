using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(lifespan))]
public class projectile : MonoBehaviour {
    private float damage;
    /// <summary>
    /// Sets the damage for the projectile
    /// </summary>
    /// <param name="damage">Damage dealt on collision with a health.Use a positive number to deal damage.</param>
    public void setDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var health = collision.transform.GetComponent<health>();

        if (health)
        {
            health.modify(-damage);
        }
        Destroy(gameObject);
    }
}
