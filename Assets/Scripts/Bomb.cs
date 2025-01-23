using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public float throwForce = 5f;
	public Animator animator;
	private Rigidbody2D rb;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		ThrowBomb();
	}
	void Update()
	{
	}
	void ThrowBomb()
	{
		rb.AddForce(new Vector2(throwForce, throwForce), ForceMode2D.Impulse);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			rb.velocity = Vector2.zero;
			animator.SetTrigger("Explosion");
			StartCoroutine(WaitAndPrint());
		}
	}

	IEnumerator WaitAndPrint()
	{
		Debug.Log("Starting wait...");
		yield return new WaitForSeconds(1);
		Debug.Log("3 seconds have passed");

		Destroy(gameObject);
	}
}
