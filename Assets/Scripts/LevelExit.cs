using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public GameObject keyPrefab;
    [SerializeField] float levelLoadDelay = 1f;
    private CircleCollider2D circleCollider2D;
    private Animator animator;
    private bool isEnter = false;
    private InventoryController inventoryController;

    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        inventoryController = FindObjectOfType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (inventoryController.HasItem(keyPrefab))
            {
                Debug.Log("C?a m?! Chuy?n màn...");
                inventoryController.RemoveItem(keyPrefab);
                animator.SetBool("IsEnter", true);
                StartCoroutine(LoadNextLevel());
            }
            else
            {
                Debug.Log("B?n c?n chìa khóa ?? m? c?a!");
            }
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        inventoryController.SaveInventory();
        SceneManager.LoadScene(nextSceneIndex);
    }
}
