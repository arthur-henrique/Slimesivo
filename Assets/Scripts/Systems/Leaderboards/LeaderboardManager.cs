using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using Unity.Services.Leaderboards;
using Unity.VisualScripting;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance = null;
    // Variables
    public List<double> scoreEntries = new List<double>();
    public List<string> nameEntries = new List<string>();

    private void Awake()
    {
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;
        }
        // If instance already exists and it's not this:
        else if (instance != this)
        {
            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
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

    public async void NewScore(string levelLeaderboardID)
    {
            var playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(levelLeaderboardID, 0);
            Debug.Log(JsonConvert.SerializeObject(playerEntry));

    }

    public async void FetchScores(string levelLeaderboardID)
    {
        Debug.Log("Started Fetch");
        var options = new GetScoresOptions
        {
            Offset = 0, // Start at the beginning of the leaderboard
            Limit = 20, // Number of entries to retrieve
            IncludeMetadata = true
        };

        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(levelLeaderboardID, options);
        scoreEntries.Clear();
        nameEntries.Clear();
        foreach (var item in scoresResponse.Results)
        {
            scoreEntries.Add(item.Score);
            nameEntries.Add(item.PlayerName);
        }
        Debug.Log("Finished Fetch");
    }

    public void GetScoresData(TMP_Text[] playersNick, TMP_Text[] playerScore)
    {
        
        for (int i = 0; i < scoreEntries.Count; i++)
        {
            playersNick[i].gameObject.SetActive(true);
            playerScore[i].gameObject.SetActive(true);
            playersNick[i].text = nameEntries[i].Substring(0, 6) + "...";
            playerScore[i].text = scoreEntries[i].ToString() + "pts";
            print(playersNick[i].text);
        }
        
    }

    public async void GetPlayerRanking(string levelLeaderboardID)
    {
        var scoresResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(levelLeaderboardID);
        GameManager.instance._playerNick = scoresResponse.PlayerName.Substring(0, 6) + "...";
        GameManager.instance.playerBestScoreFloat = (float)scoresResponse.Score;
        GameManager.instance._playerBestScore = scoresResponse.Score.ToString();
        GameManager.instance._playerRank = (scoresResponse.Rank + 1).ToString();
    }

    
}
