using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    private CircleCollider2D circleCollider2D;
    private Animator animator;
    private bool isEnter = false;

    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }

	//private void OnCollisionEnter2D(Collision2D collision)
	//{
	//    if (circleCollider2D.IsTouchingLayers(LayerMask.GetMask("Player")))
	//    {
	//        StartCoroutine(LoadNextLevel());
	//    }
	//}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			animator.SetBool("IsEnter", true);
			animator.SetBool("IsExit", false);
			isEnter = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") && isEnter)
		{
			animator.SetBool("IsExit", true);
			animator.SetBool("IsEnter", false);
			isEnter = false;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		
	}


	IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        //FindObjectOfType<ScenePersist>().ResetScenePersist();
        //SceneManager.LoadScene(nextSceneIndex);
    }
}

