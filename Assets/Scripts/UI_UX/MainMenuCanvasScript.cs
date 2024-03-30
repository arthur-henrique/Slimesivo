using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvasScript : MonoBehaviour
{
    [SerializeField] private GameObject infinityButton, campaignButton, rightChangeButton, leftChangeButton, storePagePanel, settingsButton, leaderboardPanel;
    [SerializeField] private LeaderboardsPage leaderboardPage;

    void Awake()
    {
        leftChangeButton.SetActive(false);
        rightChangeButton.SetActive(true);
        infinityButton.SetActive(false);
        campaignButton.SetActive(true);
        storePagePanel.SetActive(false);
        settingsButton.SetActive(true);
        //leaderboardPanel.SetActive(false);

    }
    public void ChangeModes()
    {
        if (infinityButton.activeSelf == true)
        {
            infinityButton.SetActive(false);
            rightChangeButton.SetActive(true);
            campaignButton.SetActive(true);
            leftChangeButton.SetActive(false);
        }
        else
        {
            campaignButton.SetActive(false);
            leftChangeButton.SetActive(true);
            infinityButton.SetActive(true);
            rightChangeButton.SetActive(false);
        }
    }

    public void InfinityMode()
    {
        print("carrega modo infinito - descomentar linha");
        //SceneManager.LoadScene(/*Index da cena do modo Infinito*/);
        
    }

    public void GoToCampaignMapScene()
    {
        SceneManager.LoadScene("CampaignMap"); //Nome da cena do mapa
    }

    public void EnterExitStorePage()
    {
        if (storePagePanel.activeSelf == true)
        {
            settingsButton.SetActive(true);
            storePagePanel.SetActive(false);
        }
        else
        {
            settingsButton.SetActive(false);
            storePagePanel.SetActive(true);
        }
    }

    /// <summary>
    /// Somente para a barra de coletaveis, no botao de +
    /// </summary>
    public void OnlyEnterStorePage()
    {
        settingsButton.SetActive(false);
        storePagePanel.SetActive(true);
    }

    public void EnterExitLeaderboardPanel()
    {
        if(leaderboardPanel.activeSelf == false)
        {
            leaderboardPanel.SetActive(true);
            leaderboardPage.SetUpLeaderboard();
        }
        else
        {
            leaderboardPanel.SetActive(false);

        }
    }

}
