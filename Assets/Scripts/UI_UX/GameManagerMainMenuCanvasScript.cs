using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerMainMenuCanvasScript : MonoBehaviour
{
    public static GameManagerMainMenuCanvasScript Instance;

    [SerializeField] private GameObject storePagePanel, settingsButton, leaderboardPanel, energyPanel;
    [SerializeField] private GameObject[] purchaseSignButtons;
    [SerializeField] private LeaderboardsPage leaderboardPage;
    [SerializeField] public TMP_Text coinText;

    void Awake()
    {
        Instance = this;
        storePagePanel.SetActive(false);
        settingsButton.SetActive(true);
        energyPanel.SetActive(false);
        //leaderboardPanel.SetActive(false);

    }
    public void EnterExitStorePage()
    {
        if (storePagePanel.activeSelf == true) //exit
        {
            settingsButton.SetActive(true);
            storePagePanel.SetActive(false);
            for (int i = 0; i < purchaseSignButtons.Length; i++)
            {
                purchaseSignButtons[i].SetActive(true);
            }
        }
        else //enter
        {
            settingsButton.SetActive(false);
            storePagePanel.SetActive(true);
            for (int i = 0; i < purchaseSignButtons.Length; i++)
            {
                purchaseSignButtons[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Somente para a barra de coletaveis, no botao de + (exceto a energia)
    /// </summary>
    public void OnlyEnterStorePage()
    {
        energyPanel.SetActive(false);
        settingsButton.SetActive(false);
        for (int i = 0; i < purchaseSignButtons.Length; i++)
        {
            purchaseSignButtons[i].SetActive(false);
        }
        storePagePanel.SetActive(true);
    }

    public void EnterExitLeaderboardPanel()
    {
        if (leaderboardPanel.activeSelf == false)
        {
            leaderboardPanel.SetActive(true);
            leaderboardPage.SetUpLeaderboard();
        }
        else
        {
            leaderboardPanel.SetActive(false);

        }
    }

    public void ShowOrHideEnergyPopUpPanel()
    {
        if (energyPanel.activeSelf == false)
        {
            energyPanel.SetActive(true);
        }
        else
        {
            energyPanel.SetActive(false);
        }
    }

}
