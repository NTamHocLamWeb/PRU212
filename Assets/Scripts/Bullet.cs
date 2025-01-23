using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float speed = 20f; 
	public Animator animator;
	public Rigidbody2D rb; 
	void Start() 
	{ 
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		rb.velocity = transform.right * speed; 
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			animator.SetTrigger("Hit");
			Debug.Log("Touch enemy");
			rb.velocity = Vector2.zero;
			rb.isKinematic = true;
			StartCoroutine(WaitToDestroy());
		}
	}

	IEnumerator WaitToDestroy()
	{
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}

	public void SetBulletValue(float value) 
	{ 
		speed = value; 
	}
}
