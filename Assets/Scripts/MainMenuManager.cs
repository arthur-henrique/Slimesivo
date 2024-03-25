using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Start()
    {
        GameManager.instance.SceneLoad();
        StartCoroutine(FetchLeaderScores());
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FetchLeaderScores()
    {
        yield return new WaitForSeconds(1);
        LeaderboardManager.instance.FetchScores("Pontuacoes_Mais_Altas");
    }
}
