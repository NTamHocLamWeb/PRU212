using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
	[SerializeField] float moveSpeed = 1f;
	public float maxHealth = 100f;
	private float currentHealth;
	Rigidbody2D myRigidbody;
	Animator myAnimator;
	bool isAlive = true;
	public float attackRange = 1.5f;
	public int damage = 10;
	private EnemyAttack attackScript;
	public float attackCooldown = 2f;
	private float lastAttackTime;
	private bool playerInRange = false;
	bool isStop = false;
	public Transform healthFill;
	private Vector3 healthFillOriginalPos;
    [SerializeField] private GameObject[] dropItems;
    [SerializeField] private float dropChance = 0.5f;

    void Start()
	{
		currentHealth = maxHealth;
		healthFillOriginalPos = healthFill.localPosition;
		attackScript = GetComponent<EnemyAttack>();
		myRigidbody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
	}

	void Update()
	{
		if (isAlive && !isStop)
		{
			myRigidbody.velocity = new Vector2(moveSpeed, 0f);

			if (moveSpeed != 0)
			{
				FlipEnemyFacing();
			}
		}
		else if (!isAlive)
		{
			myRigidbody.velocity = Vector2.zero;
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
	public void SetPlayerInRange(bool inRange)
	{
		playerInRange = inRange;
		if (playerInRange)
		{
			TryAttack();
		}
	}

	void TryAttack()
	{
		if (playerInRange && Time.time > lastAttackTime + attackCooldown)
		{
			isStop = true;
			myAnimator.SetTrigger("isAttack");
			myRigidbody.velocity = Vector2.zero;
			lastAttackTime = Time.time;
			StartCoroutine(Stun());
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playerInRange = false;
		}

		if (!isAlive || isStop)
		{
			return;
		}

		moveSpeed = -moveSpeed;
		FlipEnemyFacing();
	}

	void TakeDamage(int damage)
	{
		currentHealth -= damage;
		float healthPercent = Mathf.Clamp(currentHealth / maxHealth, 0, 1);

		healthFill.localScale = new Vector3(healthPercent, 0.1f, 1);
		healthFill.localPosition = new Vector3(
			healthFillOriginalPos.x - (1 - healthPercent) * 0.5f,
			healthFillOriginalPos.y,
			healthFillOriginalPos.z
		);

		if (currentHealth > 0)
		{
			isStop = true;
			myAnimator.SetTrigger("isHit");
			StartCoroutine(Stun());
		}
		else
		{
			myAnimator.SetTrigger("isDie");
            DropItem();
            isAlive = false;
		}
	}

	IEnumerator Stun()
	{
		yield return new WaitForSeconds(2f);
		isStop = false;
	}

	IEnumerator WaitToDestroy()
	{
		yield return new WaitForSeconds(2f);
		Destroy(gameObject);
	}

	void FlipEnemyFacing()
	{
		transform.localScale = new Vector2((Mathf.Sign(myRigidbody.velocity.x) * 1f), 1f);
	}

	public void Attack()
	{
		attackScript.PerformAttack();
	}

    private void DropItem()
    {
        if (dropItems.Length > 0 && Random.value < dropChance)
        {
            int randomIndex = Random.Range(0, dropItems.Length);
            GameObject droppedItem = Instantiate(dropItems[randomIndex], transform.position, Quaternion.identity);
            Debug.Log($"Quái r?t {droppedItem.name}");
        }
    }
}
