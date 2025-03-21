using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
	private EnemyMovement enemy;

	void Start()
	{
		enemy = GetComponentInParent<EnemyMovement>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			enemy.SetPlayerInRange(true);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			enemy.SetPlayerInRange(false);
		}
	}
}
