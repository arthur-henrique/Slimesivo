using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardsPage : MonoBehaviour
{
    public string leaderboardID;
    [SerializeField] TMP_Text[] nameTexts;
    [SerializeField] TMP_Text[] scoreTexts;

    [SerializeField] TMP_Text playerNick;
    [SerializeField] TMP_Text playerScore;
    [SerializeField] TMP_Text playerRank;


    public void SetUpLeaderboard()
    {
        LeaderboardManager.instance.GetPlayerRanking(leaderboardID, playerNick, playerScore, playerRank);
        for (int i = 0; i < nameTexts.Length; i++)
        {
            LeaderboardManager.instance.GetScoresData(i, nameTexts[i], scoreTexts[i]);
        }

    }

}
