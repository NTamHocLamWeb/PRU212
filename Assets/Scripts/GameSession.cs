using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    public GameObject gameOver;


    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
        gameObject.SetActive(false);
    }

    //public void ProcessPlayerDeath()
    //{
    //    if (playerLives > 1)
    //    {
    //        TakeLife();
    //    }
    //    else
    //    {
    //        ResetGameSession();
    //    }
    //}

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }

    public void GameOver()
    {
        gameObject.SetActive(true);
    }

    //void ResetGameSession()
    //{
    //    FindObjectOfType<ScenePersist>().ResetScenePersist();
    //    SceneManager.LoadScene(0);
    //    Destroy(gameObject);
    //}

}
