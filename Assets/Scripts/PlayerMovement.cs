using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float moveSpeed = 3f;
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform firePoint;
	[SerializeField] private float bulletValue = 20f;
	[SerializeField] public int health = 5;
	[SerializeField] public int energy = 6;
	private Rigidbody2D myRigidbody;
	Vector2 moveInput;
	Animator myAnimator;
	BoxCollider2D myFeetCollider;
	bool isAlive = true;
	[SerializeField] public GameEvent playerDamagedEvent;
	[SerializeField] public GameEvent playerShotEvent;
	[SerializeField] public GameObject gameOver;
	private bool isInvincible = false;
	private float invincibilityDuration = 0.5f;
	private SpriteRenderer spriteRenderer;
	private Color originalColor;
	private AudioManager audioManager;
    public GameObject loadBullet;
    public float reloadTime = 2f;
    public int maxAmmo = 6;
    private Vector3 originalScale;
	private GameSession gameSession;
    private bool isReloading = false;
    private bool isShooting = false;
	private InventoryController inventory;
    public GameObject lifePrefab;

    void Start()
	{
		myAnimator = GetComponent<Animator>();
		myRigidbody = GetComponent<Rigidbody2D>();
		myFeetCollider = GetComponent<BoxCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		originalColor = spriteRenderer.color;
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        loadBullet.SetActive(false);
		gameSession = FindObjectOfType<GameSession>();
		inventory = FindObjectOfType<InventoryController>();
    }

	void Update()
	{
		if (!isAlive) 
		{
            StartCoroutine(IsDead());
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(UseLife());
        }

        FlipSprite();
	}

    void OnEnable()
    {
        inventory.LoadInventory();
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

    private IEnumerator UseLife()
    {
        if (inventory.HasItem(lifePrefab) && health < 5)
        {

            yield return StartCoroutine(FlashGreen());
            health++;
            playerDamagedEvent.RaiseEvent(health);
            Debug.Log("Gain 1 life");
            inventory.RemoveItem(lifePrefab);
        }
        else
        {
            Debug.Log("Not have valid");
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
        if (!isAlive) yield break;

        health--;
        audioManager.PlaySFX(audioManager.hurtClip);
        isInvincible = true;
        playerDamagedEvent.RaiseEvent(health);
        if (health <= 0)
        {
            myAnimator.SetTrigger("isDie");
            moveSpeed = 0f;
            isAlive = false;
            StopAllCoroutines();
            yield break;
        }
        float originalSpeed = moveSpeed;
        moveSpeed = 0f;
        moveInput = Vector2.zero;
        myRigidbody.velocity = Vector2.zero;
        myAnimator.SetTrigger("isHit");
        yield return StartCoroutine(FlashRed());
        yield return new WaitForSeconds(0.5f);
        moveSpeed = originalSpeed;

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
	void OnFire(InputValue value)
	{
        if (isReloading || isShooting) return;
        if (!isAlive) { return; }
		if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Mainground"))) { return; }
        if (energy > 0)
        {
            energy--;
            playerShotEvent.RaiseEvent(energy);
            myAnimator.SetTrigger("Shoot");
            StartCoroutine(FireCoroutine());
        }
        if (energy == 0)
		{
            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator FireCoroutine()
    {
        isShooting = true;

        yield return new WaitForSeconds(1f);

        isShooting = false;
    }

    public void Fire()
	{
		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet script = bullet.GetComponent<Bullet>();
		if (script != null)
		{
			script.SetBulletValue(bulletValue);
		}
	}

	IEnumerator FlashRed()
	{
		spriteRenderer.color = Color.red;
		yield return new WaitForSeconds(0.2f);
		spriteRenderer.color = originalColor;
	}

    IEnumerator FlashGreen()
    {
        spriteRenderer.color = Color.green;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }

    private IEnumerator IsDead()
	{
		audioManager.StopMusicBackground();
        moveSpeed = 0f;
        moveInput = Vector2.zero;
        myRigidbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(3f);
        Time.timeScale = 0f;
		gameSession.GameOver();
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        loadBullet.SetActive(true);
        originalScale = loadBullet.transform.localPosition;
        float timeElapsed = 0;

        while (timeElapsed < reloadTime)
        {
            timeElapsed += Time.deltaTime;
            float progress = timeElapsed / reloadTime;

            loadBullet.transform.localScale = new Vector3(progress, 0.1f, 1);

            loadBullet.transform.localPosition = new Vector3(
                originalScale.x - ((1 - progress) * 0.5f),
                originalScale.y,
                originalScale.z
            );

            yield return null;
        }

        loadBullet.SetActive(false);
        energy = maxAmmo;

        loadBullet.transform.localScale = originalScale;
        loadBullet.transform.localPosition = originalScale;

        playerShotEvent.RaiseEvent(energy);

        isReloading = false;
    }

}
