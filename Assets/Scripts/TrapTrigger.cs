using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    PolygonCollider2D collider2D;
    Animator animator;
	public float damage = 10f;
    private bool canDamage = false;
	void Start()
    {
        collider2D = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {
        canDamage = true;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("Touch");
        }
	}
}
