using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePauseController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    private static GamePauseController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            pauseMenuUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            pauseMenuUI.SetActive(false);
        }
    }

    public void RestartGame()
    {
        DestroyAllDontDestroyOnLoad();
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    void DestroyAllDontDestroyOnLoad()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.buildIndex == -1)
            {
                Destroy(obj);
            }
        }
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
