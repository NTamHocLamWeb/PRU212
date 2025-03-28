using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreEntry
{

    public int score;
    public string timestamp;

    public ScoreEntry(int playerScore)
    {
        score = playerScore;
        timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
