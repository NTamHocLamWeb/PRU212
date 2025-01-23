using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
	public GameObject enemy;
	// Start is called before the first frame update
	void OnTriggerEnter2D(Collider2D other)
	{ 
		if (other.CompareTag("Player")) {
			enemy.GetComponent<EnemyMovement>().Attack(); 
		} 
	}
}
