using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class ScoreManager : MonoBehaviour
{
    public int coinScoreMultiplier = 10;
    public int maxTimeThreshold = 120; // Maximum time in seconds
    public int timeScoreMultiplier = 5;
    public int livesScoreMultiplier = 100;
    public string level_name;

    public LocalizedString scoreString;

    public int CalculateScore(int coinsCollected, float timeTaken, int livesRemaining)
    {
        int coinScore = coinsCollected * coinScoreMultiplier;

        int timeScore = 0;
        if (timeTaken <= maxTimeThreshold)
        {
            timeScore = (maxTimeThreshold - (int)timeTaken) * timeScoreMultiplier;
        }

        int livesScore = livesRemaining * livesScoreMultiplier;

        int totalScore = coinScore + timeScore + livesScore;
        if (PlayerPrefs.GetInt(level_name + "_1rewarded") == 1)
            totalScore += 250;
        if (PlayerPrefs.GetInt(level_name + "_2rewarded") == 1)
            totalScore += 250;
        if (PlayerPrefs.GetInt(level_name + "_3rewarded") == 1)
            totalScore += 250;

        Debug.LogAssertion("Total Score: " + totalScore);
        return totalScore;
    }

    public void SaveScore(int score)
    {
        // Save the score to the player's best score
        if (score > PlayerPrefs.GetInt(level_name + "_score"))
        {
            int scoreDif = score - PlayerPrefs.GetInt(level_name + "_score");
            PlayerPrefs.SetInt(level_name + "_score", score);
            UpdateScoreText(score);
            Debug.LogWarning(scoreDif);
            LeaderboardManager.instance.AddLevelScore(scoreDif);
        }
        else
        {
            UpdateScoreText(PlayerPrefs.GetInt(level_name + "_score"));
        }
    }
    
    public void UpdateScoreText(int score)
    {
        scoreString.Arguments = new object[] { score };
        scoreString.StringChanged += (localizedString) =>
        {
            GameManager.instance.scoreText.text = localizedString;
        };
    }
}
