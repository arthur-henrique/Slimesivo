using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardsPage : MonoBehaviour
{
    public string leaderboardID;
    [SerializeField] TextMeshPro[] nameTexts;
    [SerializeField] TextMeshPro[] scoreTexts;

    [SerializeField] TextMeshPro playerNick;
    [SerializeField] TextMeshPro playerScore;
    [SerializeField] TextMeshPro playerRank;


    public void SetUpLeaderboard()
    {
        for (int i = 0; i < nameTexts.Length; i++)
        {
            LeaderboardManager.instance.GetScoresData(i, nameTexts[i], scoreTexts[i]);
        }

        LeaderboardManager.instance.GetPlayerRanking(leaderboardID, playerNick, playerScore, playerRank);
    }

}
