using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	[SerializeField] float moveSpeed = 1f;
	int health = 100;
	Rigidbody2D myRigidbody;
	Animator myAnimator;
	bool isAlive = true;
	public float attackRange = 1.5f;
	public int damage = 10;

	void Start()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
	}

	void Update()
	{
		if (isAlive)
		{
			myRigidbody.velocity = new Vector2(moveSpeed, 0f);
		}
		else
		{
			StartCoroutine(WaitToDestroy());
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Bullet") && isAlive)
		{
			TakeDamage(10);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (!isAlive)
		{
			moveSpeed = 0f;
		}
		else
		{
			moveSpeed = -moveSpeed;
			FlipEnemyFacing();
		}
	}

	void TakeDamage(int damage)
	{
		health -= damage;
		if (health > 0)
		{
			myAnimator.SetTrigger("isHit");
			StartCoroutine(Stun());
		}
		else
		{
			myAnimator.SetTrigger("isDie");
			isAlive = false;
		}
	}

	IEnumerator Stun()
	{
		float originalSpeed = moveSpeed;
		moveSpeed = 0f;
		yield return new WaitForSeconds(1f);
		moveSpeed = originalSpeed;
	}

	IEnumerator WaitToDestroy()
	{
		yield return new WaitForSeconds(2f);
		Destroy(gameObject);
	}

	void FlipEnemyFacing()
	{
		transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x) * 1f), 1f);
	}

	public void Attack()
	{
		myAnimator.SetTrigger("isAttack");
	}
}
