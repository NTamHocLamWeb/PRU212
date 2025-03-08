using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
	public GameObject enemy;
	// Start is called before the first frame update

	private void Update()
	{
		if (enemy.transform.localScale.x < 0)
		{
			transform.localScale = new Vector2(-1f, 1f);
		}
		else
		{
			transform.localScale = new Vector2(1f, 1f);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{ 
		if (other.CompareTag("Player")) {
			enemy.GetComponent<EnemyMovement>().Attack(); 
		} 
	}
}
