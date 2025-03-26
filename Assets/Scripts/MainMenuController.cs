using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject RankUI;
    private bool isOpen = false;
    private GameSession gameSession;
    [SerializeField] TextMeshProUGUI top1;
    [SerializeField] TextMeshProUGUI top2;
    [SerializeField] TextMeshProUGUI top3;
    [SerializeField] TextMeshProUGUI top4;
    [SerializeField] TextMeshProUGUI top5;
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }


    public void ToggleRank()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            List<int> rank = gameSession.DisplayScoreHistory();

            if (rank == null || rank.Count == 0)
            {
                Debug.LogWarning("Không có ?i?m nào ?? hi?n th?.");
                RankUI.SetActive(true);
                top1.text = "N/A";
                top2.text = "N/A";
                top3.text = "N/A";
                top4.text = "N/A";
                top5.text = "N/A";
                return;
            }

            RankUI.SetActive(true);

            top1.text = rank.Count > 0 ? rank[0].ToString() : "N/A";
            top2.text = rank.Count > 1 ? rank[1].ToString() : "N/A";
            top3.text = rank.Count > 2 ? rank[2].ToString() : "N/A";
            top4.text = rank.Count > 3 ? rank[3].ToString() : "N/A";
            top5.text = rank.Count > 4 ? rank[4].ToString() : "N/A";
        }
        else
        {
            RankUI.SetActive(false);
        }
    }

    public void StartGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
