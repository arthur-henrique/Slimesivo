using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerMainMenuCanvasScript : MonoBehaviour
{
    public static GameManagerMainMenuCanvasScript Instance;

    [SerializeField] private GameObject storePagePanel, settingsButton, leaderboardPanel, energyPanel, storeButton, leaderboardButton;
    [SerializeField] private GameObject[] purchaseSignButtons;
    [SerializeField] private LeaderboardsPage leaderboardPage;
    [SerializeField] public TMP_Text coinText;

    void Awake()
    {
        if (Instance == null)
        {
            // If not, set instance to this
            Instance = this;
        }
        // If instance already exists and it's not this:
        else if (Instance != this)
        {
            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
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
        MainMenuCanvasScript.Instance.HideInventory();
        ShowSideButtons();
        for (int i = 0; i < purchaseSignButtons.Length; i++)
        {
            purchaseSignButtons[i].SetActive(false);
        }
        storePagePanel.SetActive(true);
    }

    public void HideSideButtons()
    {
        storeButton.SetActive(false);
        leaderboardButton.SetActive(false);
    }
    public void ShowSideButtons()
    {
        storeButton.SetActive(true);
        leaderboardButton.SetActive(true);
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
