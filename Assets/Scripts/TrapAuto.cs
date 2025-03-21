using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAuto : MonoBehaviour
{
	Animator animator;
	public PolygonCollider2D attackArea;
	public float attackDelay = 0.5f;
	public float attackDuration = 0.5f;

	void Start()
	{
		animator = GetComponent<Animator>();
		attackArea = GetComponent<PolygonCollider2D>();
		attackArea.enabled = false;
	}

	public void Attack()
	{
		StartCoroutine(EnableHitbox());
	}
	IEnumerator EnableHitbox()
	{
		attackArea.enabled = true;
		yield return new WaitForSeconds(attackDuration);
		attackArea.enabled = false;
	}
}
