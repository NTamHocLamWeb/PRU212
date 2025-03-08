using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public int health = 100;
    public Animator animator;
	Vector2 moveInput;
	public float moveSpeed = 1f;
    public PolygonCollider2D zombieBody;
    public BoxCollider2D attackZone;
    public Rigidbody2D rb;
    public Transform playerTransform;
	private Transform player;
	private bool isChasing = false;
	void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
	    animator = GetComponent<Animator>();
        zombieBody = GetComponent<PolygonCollider2D>();
        attackZone = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
	}
    void Update()
    {
		//rb.velocity = new Vector2(moveSpeed, 0f);
		//      if (attackZone.gameObject.CompareTag("Player"))
		//{
		//	AttackPlayer();
		//}
		if (isChasing == true) 
		{ 
			AttackPlayer(); 
		}
	}

    void OnCollisionEnter2D(Collision2D collision)
	{ 
        if (collision.gameObject.CompareTag("Player")) 
        {
			AttackPlayer();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player")) 
		{ 
			isChasing = true;
			
		}
	}

	void TakeDamage(int damage) 
    { 
        health -= damage;
        Debug.Log("Zombie Health: " + health);
        if (health <= 0) 
        { 
            Die(); 
        }
        animator.SetTrigger("Hurt");

    } 
    void Die() 
    {
        animator.SetTrigger("Die");
        Debug.Log("Zombie Died!");
		StartCoroutine(WaitAndPrint());
    }

	IEnumerator WaitAndPrint()
	{ 
        Debug.Log("Starting wait..."); 
        yield return new WaitForSeconds(3);
        Debug.Log("3 seconds have passed");

		Destroy(gameObject);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		moveSpeed = -moveSpeed;
		FlipSprite();
	}

	void FlipSprite()
	{
		transform.localScale = new Vector2(-4, 4);

	}

	void AttackPlayer()
    {
		transform.position = Vector2.MoveTowards(transform.position, playerTransform.transform.position, 0.5f * Time.deltaTime); 
        Debug.Log("Zombie is moving towards the player");
		//animator.SetTrigger("Attack");
		animator.SetBool("Walking", true);
	}
}
