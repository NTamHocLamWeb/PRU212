using System.Collections;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    Animator animator;
    public GameObject attackArea;
    public float attackDelay = 0.5f;
	public float attackDuration = 0.5f;

	void Start()
    {
        animator = GetComponent<Animator>();
        attackArea.SetActive(false);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("Touch");
		}
    }
    public void Attack()
    {
		StartCoroutine(EnableHitbox());
	}
	IEnumerator EnableHitbox()
	{
		attackArea.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
		attackArea.SetActive(false);
	}
}
