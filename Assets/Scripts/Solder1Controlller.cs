using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Solder1Controlller : MonoBehaviour
{
	[SerializeField] float jumpSpeed = 5f;
	private Rigidbody2D myRigidbody;
	public float moveSpeed = 3f;
	Vector2 moveInput;
	Animator solder1Animtor;
	public LayerMask groundLayer;
	private bool isGrounded;
	PolygonCollider2D myBodyCollider;
	BoxCollider2D myFeetCollider;
	public GameObject bombPrefab;
	public Transform throwPoint;
	public GameObject bulletPrefab;
	public Transform firePoint;
	float gravityScaleAtStart;
	bool isAlive = true;

	void Start()
	{
		solder1Animtor = GetComponent<Animator>();
		myBodyCollider = GetComponent<PolygonCollider2D>();
		myRigidbody = GetComponent<Rigidbody2D>();
		myFeetCollider = GetComponent<BoxCollider2D>();
		gravityScaleAtStart = myRigidbody.gravityScale;
	}

	void Update()
	{
		if (!isAlive) { return; }
		Walk();
		if (Input.GetKey(KeyCode.J))
		{
			solder1Animtor.SetTrigger("Shoot");
			StartCoroutine(ShootContinuously());
		}

		if (Input.GetKey(KeyCode.R))
		{
			solder1Animtor.SetTrigger("Reload");
		}

		if (Input.GetKeyDown(KeyCode.B)) { ThrowBomb(); }
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
			solder1Animtor.SetTrigger("Jump");
		}
	}



	void ThrowBomb()
	{
		solder1Animtor.SetTrigger("Grenade");
		Instantiate(bombPrefab, throwPoint.position, throwPoint.rotation);
		SpriteRenderer bombSpriteRenderer = bombPrefab.GetComponent<SpriteRenderer>();
		if (bombSpriteRenderer != null)
		{
			bombSpriteRenderer.sortingOrder = 1;
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
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			solder1Animtor.SetBool("isRunning", true);
			moveSpeed = 7.5f;
		}

		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			solder1Animtor.SetBool("isRunning", false);
			moveSpeed = 5f;
		}
	}

	void Walk()
	{
		Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.velocity.y);
		myRigidbody.velocity = playerVelocity;

		bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
		solder1Animtor.SetBool("isWalking", playerHasHorizontalSpeed);

		Debug.Log("Player velocity: " + myRigidbody.velocity);
	}

	void FlipSprite()
	{
		if (moveInput.x > 0)
		{
			transform.localScale = new Vector2(4, 4);
		}
		else if (moveInput.x < 0)
		{
			transform.localScale = new Vector2(-4, 4);
		}
	}

	IEnumerator ShootContinuously()
	{
		yield return new WaitForSeconds(0.9f);
		Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
	}
}