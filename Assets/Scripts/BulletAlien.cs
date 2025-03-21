using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAlien : MonoBehaviour
{
	public float speed = 10f;
	public Animator animator;
	public Rigidbody2D rb;
	private AudioManager audioManager;

	private void Awake()
	{
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		rb.velocity = transform.right * speed;
		audioManager.PlaySFX(audioManager.shotClip);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			animator.SetTrigger("Hit");
			Debug.Log("Touch enemy");
			rb.velocity = Vector2.zero;
			GetComponent<Collider2D>().enabled = false;
			StartCoroutine(WaitToDestroy());
		}

		if (collision.gameObject.layer == LayerMask.NameToLayer("Mainground"))
		{
			Destroy(gameObject);
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
