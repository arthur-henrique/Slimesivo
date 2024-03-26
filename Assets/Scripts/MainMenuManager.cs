using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private LeaderboardsPage lbPage;
    public void Start()
    {
        GameManager.instance.SceneLoad();
        StartCoroutine(FetchLeaderScores());
        StartCoroutine(FetchPlayerScores());
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FetchLeaderScores()
    {
        yield return new WaitForSeconds(1);
        LeaderboardManager.instance.GetPlayerRanking("Pontuacoes_Mais_Altas");
        LeaderboardManager.instance.FetchScores("Pontuacoes_Mais_Altas");
        yield return new WaitForSeconds(1);
        lbPage.SetUpPlayerRank();
    }

    IEnumerator FetchPlayerScores()
    {
        yield return new WaitForSeconds(1.5f);
        lbPage.SetUpPlayerRank();
    }
}
