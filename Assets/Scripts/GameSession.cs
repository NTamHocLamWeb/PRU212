using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    public GameObject gameOver;
    private static GameSession instance;

    // Khóa lưu trữ điểm
    private const string SCORE_HISTORY_KEY = "GameScoreHistory";

    // Danh sách điểm
    private List<ScoreEntry> scoreHistory = new List<ScoreEntry>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (gameOver != null)
            {
                gameOver.transform.SetParent(transform);
            }
            DontDestroyOnLoad(GameObject.Find("GameTabs"));
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        scoreText.text = score.ToString();
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        // Hiển thị màn hình game over
        gameOver.SetActive(true);

        // Lưu điểm
        SaveScore();
    }

    // Phương thức lưu điểm
    private void SaveScore()
    {
        // Tải danh sách điểm hiện tại
        LoadScoreHistory();

        // Tạo điểm số mới
        ScoreEntry newScore = new ScoreEntry(score);

        // Thêm điểm mới vào danh sách
        scoreHistory.Add(newScore);

        // Sắp xếp điểm theo thứ tự giảm dần
        scoreHistory = scoreHistory
            .OrderByDescending(x => x.score)
            .Take(5) // Giới hạn 10 điểm cao nhất
            .ToList();

        // Lưu lại danh sách điểm
        SaveScoreHistory();

        // Hiển thị điểm
        DisplayScoreHistory();
    }

    // Lưu lịch sử điểm
    private void SaveScoreHistory()
    {
        // Chuyển đổi danh sách điểm sang JSON
        string jsonData = JsonUtility.ToJson(new SerializableScoreHistory(scoreHistory));

        // Lưu vào PlayerPrefs
        PlayerPrefs.SetString(SCORE_HISTORY_KEY, jsonData);
        PlayerPrefs.Save();
    }

    // Tải lịch sử điểm
    private void LoadScoreHistory()
    {
        if (PlayerPrefs.HasKey(SCORE_HISTORY_KEY))
        {
            string jsonData = PlayerPrefs.GetString(SCORE_HISTORY_KEY);
            SerializableScoreHistory loadedScores = JsonUtility.FromJson<SerializableScoreHistory>(jsonData);
            scoreHistory = loadedScores.scores;
        }
        else
        {
            scoreHistory = new List<ScoreEntry>();
        }
    }

    // Hiển thị lịch sử điểm
    public List<int> DisplayScoreHistory()
    {
        List<int> listScore = new List<int>();
        LoadScoreHistory();

        Debug.Log("--- LỊCH SỬ ĐIỂM ---");
        for (int i = 0; i < scoreHistory.Count; i++)
        {
            Debug.Log($"{i + 1}. {scoreHistory[i].score} điểm ({scoreHistory[i].timestamp})");
            listScore.Add(scoreHistory[i].score);
        }
        return listScore;
    }

    [System.Serializable]
    private class SerializableScoreHistory
    {
        public List<ScoreEntry> scores;

        public SerializableScoreHistory(List<ScoreEntry> scoreList)
        {
            scores = scoreList;
        }
    }
}