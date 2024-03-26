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

    public void Awake()
    {
        print("LBPage");
        for (int i = 0; i < nameTexts.Length; i++)
        {
            nameTexts[i].gameObject.SetActive(false);
            scoreTexts[i].gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
    public void SetUpLeaderboard()
    {
        GameManager.instance.GetPlayerRanking(playerNick, playerScore, playerRank);
        LeaderboardManager.instance.GetScoresData(nameTexts, scoreTexts);
    }
    public void SetUpPlayerRank()
    {
        GameManager.instance.GetPlayerRanking(playerNick, playerScore, playerRank);
    }

}
