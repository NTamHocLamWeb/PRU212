using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : EnemyAttack
{
	public GameObject hitbox;
	public float attackDuration = 0.5f;

	void Start()
	{
		hitbox.SetActive(false);
	}

	public override void PerformAttack()
	{
		StartCoroutine(EnableHitbox());
	}

	IEnumerator EnableHitbox()
	{
		hitbox.SetActive(true);
		yield return new WaitForSeconds(attackDuration);
		hitbox.SetActive(false);
	}
}
