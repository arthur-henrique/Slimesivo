using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using Unity.Services.Leaderboards;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance = null;
    // Variables
    public List<double> scoreEntries = new List<double>();
    public List<string> nameEntries = new List<string>();

    private void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;
        }
    }

    public async void UpdateScores(string levelLeaderboardID, float playersBestScore, float playersLatestScore)
    {
        if(playersLatestScore > playersBestScore)
        {
            var playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(levelLeaderboardID, playersLatestScore);
            Debug.Log(JsonConvert.SerializeObject(playerEntry));
        }
    }

    public async void FetchScores(string levelLeaderboardID)
    {

        var options = new GetScoresOptions
        {
            Offset = 0, // Start at the beginning of the leaderboard
            Limit = 20, // Number of entries to retrieve
            IncludeMetadata = true
        };

        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(levelLeaderboardID, options);

        foreach (var item in scoresResponse.Results)
        {
            scoreEntries.Add(item.Score);
            nameEntries.Add(item.PlayerName);
        }
    }

    public void GetScoresData(int pos, TMP_Text playersNick, TMP_Text playerScore)
    {
        playersNick.text = nameEntries[pos];
        playerScore.text = scoreEntries[pos].ToString();
    }

    public async void GetPlayerRanking(string levelLeaderboardID, TMP_Text playersNick, TMP_Text playerScore, TMP_Text playerRank)
    {
        var scoresResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(levelLeaderboardID);
        playersNick.text = scoresResponse.PlayerName;
        playerScore.text = scoresResponse.Score.ToString();
        playerRank.text = (scoresResponse.Rank + 1).ToString();
    }

    
}
