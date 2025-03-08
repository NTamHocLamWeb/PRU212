using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float jumpSpeed = 5f;
	[SerializeField] private float moveSpeed = 3f;
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform firePoint;
	[SerializeField] private float fireRate = 5f;
	[SerializeField] private float bulletValue = 20f;
	[SerializeField] public int health = 5;
	private Rigidbody2D myRigidbody;
	Vector2 moveInput;
	Animator myAnimator;
	BoxCollider2D myFeetCollider;
	bool isJump = false;
	bool isAlive = true;
	private float nextFireTime = 5f;
	public GameEvent playerDamagedEvent;
	private bool isInvincible = false;
	private float invincibilityDuration = 1.0f;

	void Start()
	{
		myAnimator = GetComponent<Animator>();
		myRigidbody = GetComponent<Rigidbody2D>();
		myFeetCollider = GetComponent<BoxCollider2D>();
	}

	void Update()
	{
		if (!isAlive) { return; }
		FlipSprite();
		
	}
	void OnJump(InputValue value)
	{
		if (!isAlive) { return; }

		if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Mainground"))) { return; }

		if (value.isPressed)
		{
			myAnimator.SetBool("isJumping", true);
			myRigidbody.velocity += new Vector2(0f, jumpSpeed);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Mainground")))
		{
			myAnimator.SetBool("isJumping", false);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Damage") && !isInvincible)
		{
			StartCoroutine(TakeDamage());
		}
	}

	private IEnumerator TakeDamage()
	{
		isInvincible = true;
		health--;

		playerDamagedEvent.RaiseEvent(health);
		myAnimator.SetTrigger("isHit");

		if (health <= 0)
		{
			myAnimator.SetTrigger("isDie");
		}

		yield return new WaitForSeconds(invincibilityDuration);
		isInvincible = false;
	}

	void OnMove(InputValue value)
	{
		if (!isAlive) { return; }
		moveInput = value.Get<Vector2>();
		Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.velocity.y);
		myRigidbody.velocity = playerVelocity;
		if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Mainground"))) { return; }
		bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
		myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
	}

	void OnFire(InputValue value)
	{
		if (!isAlive) { return; }
		if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Mainground"))) { return; }
		if (value.isPressed && Time.time >= nextFireTime)
		{
			if (Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon)
			{
				myAnimator.SetTrigger("RunningAndShooting");
			}
			else
			{
				myAnimator.SetTrigger("Shoot");
			}
			nextFireTime = Time.time + fireRate;
			StartCoroutine(Fire());
		}
	}

	void FlipSprite()
	{
		if (moveInput.x > 0)
		{
			transform.localScale = new Vector2(1, 1);
			bulletValue = 20f;
		}
		else if (moveInput.x < 0)
		{
			transform.localScale = new Vector2(-1, 1);
			bulletValue = -20f;
		}
	}

	IEnumerator Fire()
	{
		yield return new WaitForSeconds(0.5f);
		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet script = bullet.GetComponent<Bullet>();
		if (script != null)
		{
			script.SetBulletValue(bulletValue);
		}
	}
}
