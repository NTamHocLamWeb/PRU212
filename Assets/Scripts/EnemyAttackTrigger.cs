using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
	public GameObject enemy;

	void OnTriggerEnter2D(Collider2D other)
	{ 
		if (other.CompareTag("Player")) {
			enemy.GetComponent<EnemyMovement>().Attack(); 
		} 
	}
}
