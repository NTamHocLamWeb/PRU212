using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float jumpSpeed = 5f;
	private Rigidbody2D myRigidbody;
	public float moveSpeed = 3f;
	Vector2 moveInput;
	Animator myAnimator;
	public LayerMask groundLayer;
	private bool isGrounded;
	PolygonCollider2D myBodyCollider;
	BoxCollider2D myFeetCollider;
	public GameObject bulletPrefab;
	public Transform firePoint;
	bool isAlive = true;
	public float fireRate = 0.5f;
	private float nextFireTime = 0f;
	public float bulletValue = 20f;
	int health = 10;

	void Start()
	{
		myAnimator = GetComponent<Animator>();
		myBodyCollider = GetComponent<PolygonCollider2D>();
		myRigidbody = GetComponent<Rigidbody2D>();
		myFeetCollider = GetComponent<BoxCollider2D>();
	}

	void Update()
	{
		if (!isAlive) { return; }
		if (Input.GetKey(KeyCode.J) && Time.time >= nextFireTime)
		{
			if (Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon) 
			{ 
				myAnimator.SetBool("isRunningAndShooting", true); 
			} else 
			{ 
				myAnimator.SetBool("isRunningAndShooting", false);
				myAnimator.SetTrigger("Shoot");
			}
			nextFireTime = Time.time + fireRate;
			StartCoroutine(Fire());
		}

		

		FlipSprite();
		Run();
	}
	void OnJump(InputValue value)
	{
		if (!isAlive) { return; }

		if (value.isPressed)
		{
			// do stuff
			myRigidbody.velocity += new Vector2(0f, jumpSpeed);
			myAnimator.SetTrigger("Jump");
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Trap"))
		{
			health--;
			if (health > 0)
			{
				myAnimator.SetTrigger("isHit");
			}
			else
			{
				myAnimator.SetTrigger("isDie");
				isAlive = false;
			}
		}
	}

	void OnMove(InputValue value)
	{
		if (!isAlive) { return; }
		moveInput = value.Get<Vector2>();
		Debug.Log("OnMove called with value: " + moveInput);
	}

	void Run()
	{
		Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.velocity.y);
		myRigidbody.velocity = playerVelocity;

		bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
		myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);

		Debug.Log("Player velocity: " + myRigidbody.velocity);
	}

	void FlipSprite()
	{
		if (moveInput.x > 0)
		{
			transform.localScale = new Vector2(4, 4);
			bulletValue = 20f;
		}
		else if (moveInput.x < 0)
		{
			transform.localScale = new Vector2(-4, 4);
			bulletValue = -20f;
		}
	}

	IEnumerator Fire()
	{
		yield return new WaitForSeconds(0.9f);
		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet script = bullet.GetComponent<Bullet>();
		if (script != null)
		{
			script.SetBulletValue(bulletValue);
		}
	}
}
