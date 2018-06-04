using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

	[SerializeField] float maxHealthPoints = 100f;
	[SerializeField] int enemyLayer = 9;
	[SerializeField] float damagePerHit = 10f;
	[SerializeField] float minTimeBetweenHits = 0.5f;
	[SerializeField] float maxAttackRange = 2f;

	GameObject currentTarget;
	float currentHealthPoints;
	CameraRaycaster cameraRaycaster;
	float lastHitTime = 0f;

	public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

	void Start()
	{
		cameraRaycaster = FindObjectOfType<CameraRaycaster>();
		cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
		currentHealthPoints = maxHealthPoints;
	}
	
	void OnMouseClick(RaycastHit raycastHit, int layerHit) 
	{
		if(layerHit == enemyLayer)
		{
			var enemy = raycastHit.collider.gameObject;

			if((enemy.transform.position - transform.position).magnitude > maxAttackRange) 
			{
				return;
			}

			currentTarget = enemy;
			var enemyComponent = currentTarget.GetComponent<Enemy>();
			if(Time.time - lastHitTime > minTimeBetweenHits) 
			{	
				enemyComponent.TakeDamage(damagePerHit);
				lastHitTime = Time.time;
			}
		}
	}

	public void TakeDamage(float damage) 
	{
		currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
	}

	// void IDamageable.TakeDamage(float Damage) 
	// {

	// }
	
}
