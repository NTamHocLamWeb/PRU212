using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	private float jumpSpeed = 3.5f;
	private float moveSpeed = 3f;
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform firePoint;
	[SerializeField] private float fireRate = 5f;
	[SerializeField] private float bulletValue = 20f;
	[SerializeField] public int health = 5;
	[SerializeField] public int energy = 6;
	private Rigidbody2D myRigidbody;
	Vector2 moveInput;
	Animator myAnimator;
	BoxCollider2D myFeetCollider;
	bool isAlive = true;
	private float nextFireTime = 5f;
	public GameEvent playerDamagedEvent;
	public GameEvent playerShotEvent;
	private bool isInvincible = false;
	private float invincibilityDuration = 1.0f;
	private SpriteRenderer spriteRenderer;
	private Color originalColor;
	private AudioManager audioManager;
	private GameSession gameSession;

	void Start()
	{
		myAnimator = GetComponent<Animator>();
		myRigidbody = GetComponent<Rigidbody2D>();
		myFeetCollider = GetComponent<BoxCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		originalColor = spriteRenderer.color;
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
		gameSession = gameSession.GetComponent<GameSession>();
	}

	void Update()
	{
		if (!isAlive) 
		{
			gameSession.GameOver();
		}
		FlipSprite();
		
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

	void OnJump(InputValue value)
	{
		if (!isAlive) { return; }

		if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Mainground"))) { return; }

		if (value.isPressed)
		{
			myAnimator.SetBool("isJumping", true);
			audioManager.PlaySFX(audioManager.jumpClip);
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
		audioManager.PlaySFX(audioManager.hurtClip);
		StartCoroutine(FlashRed());
		StartCoroutine(Stun());

		if (health <= 0)
		{
			myAnimator.SetTrigger("isDie");
		}

		yield return new WaitForSeconds(invincibilityDuration);
		isInvincible = false;
	}

	IEnumerator Stun()
	{
		float originalSpeed = moveSpeed;
		moveSpeed = 0f;
		yield return new WaitForSeconds(1f);
		moveSpeed = originalSpeed;
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
		if (energy > 0)
		{
			energy--;
			playerShotEvent.RaiseEvent(energy);
			yield return new WaitForSeconds(0.5f);
			GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
			Bullet script = bullet.GetComponent<Bullet>();
			if (script != null)
			{
				script.SetBulletValue(bulletValue);
			}
		}
		else
		{
			energy = 6;
		}
	}

	IEnumerator FlashRed()
	{
		spriteRenderer.color = Color.red;
		yield return new WaitForSeconds(0.2f);
		spriteRenderer.color = originalColor;
	}
}
