using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvasScript : MonoBehaviour
{
    public static MainMenuCanvasScript Instance;
    [SerializeField] private GameObject infinityButton, campaignButton, rightChangeButton, leftChangeButton, storePagePanel, inventoryPagePanel, settingsButton, leaderboardPanel, energyPanel;
    [SerializeField] private GameObject[] purchaseSignButtons;
    [SerializeField] private LeaderboardsPage leaderboardPage;
    public GameObject menuPlayer;

    
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
        leftChangeButton.SetActive(false);
        rightChangeButton.SetActive(true);
        infinityButton.SetActive(false);
        campaignButton.SetActive(true);
        storePagePanel.SetActive(false);
        //settingsButton.SetActive(true);
        energyPanel.SetActive(false);
        //leaderboardPanel.SetActive(false);

    }

    private void Start()
    {
        if(GameManager.instance.menuPlayerAnimator != null)
            menuPlayer.GetComponent<Animator>().runtimeAnimatorController = GameManager.instance.menuPlayerAnimator;
    }

    public void ChangePlayerSkin(AnimatorOverrideController anim)
    {
        menuPlayer.GetComponent<Animator>().runtimeAnimatorController = anim;
    }
    public void ChangeModes()
    {
        VibrationManager.instance.VibeUI();
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
    public void HideInventory()
    {
        inventoryPagePanel.SetActive(false);
    }
    public void InfinityMode()
    {
        print("carrega modo infinito - descomentar linha");
        VibrationManager.instance.VibeUI();
        //SceneManager.LoadScene(/*Index da cena do modo Infinito*/);

    }

    public void GoToCampaignMapScene()
    {
        VibrationManager.instance.VibeUI();
        SceneManager.LoadScene("2 - CampaignMap"); //Nome da cena do mapa
    }

    public void EnterExitStorePage()
    {
        if (storePagePanel.activeSelf == true) //exit
        {
            settingsButton.SetActive(true);
            storePagePanel.SetActive(false);
            VibrationManager.instance.VibeUI();
            for (int i = 0; i < purchaseSignButtons.Length; i++)
            {
                purchaseSignButtons[i].SetActive(true);
            }
        }
        else //enter
        {
            settingsButton.SetActive(false);
            storePagePanel.SetActive(true);
            VibrationManager.instance.VibeUI();
            for (int i = 0; i < purchaseSignButtons.Length; i++)
            {
                purchaseSignButtons[i].SetActive(false);
            }
        }
    }

    public void EnterExitInventoryPage()
    {
        if (inventoryPagePanel.activeSelf == true) //exit
        {
            GameManagerMainMenuCanvasScript.Instance.settingsButton.SetActive(true);
            inventoryPagePanel.SetActive(false);
            VibrationManager.instance.VibeUI();
            for (int i = 0; i < purchaseSignButtons.Length; i++)
            {
                purchaseSignButtons[i].SetActive(true);
            }
            GameManagerMainMenuCanvasScript.Instance.ShowSideButtons();
        }
        else //enter
        {
            GameManagerMainMenuCanvasScript.Instance.settingsButton.SetActive(false);
            inventoryPagePanel.SetActive(true);
            VibrationManager.instance.VibeUI();
            //InventoryDisplayer.Instance.DisplaySkins(SkinSystemChecker.Instance.allSkins);
            GameManagerMainMenuCanvasScript.Instance.HideSideButtons();
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
        VibrationManager.instance.VibeUI();
        for (int i = 0; i < purchaseSignButtons.Length; i++)
        {
            purchaseSignButtons[i].SetActive(false);
        }
        storePagePanel.SetActive(true);
    }

    public void EnterExitLeaderboardPanel()
    {
        if(leaderboardPanel.activeSelf == false)
        {
            leaderboardPanel.SetActive(true);
            leaderboardPage.SetUpLeaderboard();
            VibrationManager.instance.VibeUI();
        }
        else
        {
            leaderboardPanel.SetActive(false);
            VibrationManager.instance.VibeUI();
        }
    }

    public void ShowOrHideEnergyPopUpPanel()
    {
        VibrationManager.instance.VibeUI();
        if (energyPanel.activeSelf == false)
        {
            energyPanel.SetActive(true);
        }
        else
        {
            energyPanel.SetActive(false);
        }
    }

    public void LoadTutorial()
    {
        VibrationManager.instance.VibeUI();
        SceneManager.LoadScene("Level_Teste");
    }

}
