using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

	public float projectileSpeed;

	float damageCaused;

	public void SetDamage(float damage)
	{
		damageCaused = damage;
	}

    void OnTriggerEnter(Collider other)
    {
		Component damageableComponent = other.gameObject.GetComponent(typeof(IDamageable));
		
		if(damageableComponent) 
		{
			(damageableComponent as IDamageable).TakeDamage(damageCaused);
		}
    }
}
