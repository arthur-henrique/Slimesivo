using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private LeaderboardsPage lbPage;

    
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
        yield return new WaitForSeconds(0.5f);
        GameManagerMainMenuCanvasScript.Instance.coinText.text = CurrencyManager.instance.UpdateMainMenuCoins();
        print("HasFetchedCoins");
        print(GameManagerMainMenuCanvasScript.Instance.coinText.text);
        print(CurrencyManager.instance.currentCurrency.ToString());

        //CurrencyManager.instance.currentCurrency.ToString()
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
