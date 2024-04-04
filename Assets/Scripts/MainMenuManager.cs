using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private LeaderboardsPage lbPage;

    [SerializeField]
    private TMP_Text coinText;
    public void Start()
    {
        GameManager.instance.SceneLoad();
        CurrencyManager.instance.FetchCoinBalance();
        StartCoroutine(FetchLeaderScores());
        StartCoroutine(FetchPlayerScores());
        StartCoroutine(FetchCoins());
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    IEnumerator FetchCoins()
    {
        yield return new WaitForSeconds(0.25f);
        coinText.text = CurrencyManager.instance.currentCurrency.ToString();
        print("HasFetchedCoins");
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
