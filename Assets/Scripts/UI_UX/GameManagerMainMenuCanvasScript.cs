using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerMainMenuCanvasScript : MonoBehaviour
{
    public static GameManagerMainMenuCanvasScript Instance;

    [SerializeField] public GameObject storePagePanel, settingsButton, leaderboardPanel, energyPanel, storeButton, leaderboardButton, settingsPanel;
    [SerializeField] private GameObject[] purchaseSignButtons;
    [SerializeField] private LeaderboardsPage leaderboardPage;
    [SerializeField] public TMP_Text coinText;
    [SerializeField] public Image profileImage;


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
        settingsPanel.SetActive(false);
        //leaderboardPanel.SetActive(false);

        if(GoogleLogin.isSignedInWithGooglePlayGames)
            StartCoroutine(LoadProfileImage(GoogleLogin.PlayerProfileImageUrl));

    }

    public void EnterExitSettingsPanel()
    {
        if (settingsPanel.activeSelf == true)
        {
            settingsPanel.SetActive(false);
        }
        else
        {
            settingsPanel.SetActive(true);
        }
    }

    private IEnumerator LoadProfileImage(string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error loading image: " + uwr.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                profileImage.sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
            }
        }
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
    public void UpdateCoins()
    {
        Debug.LogWarning("CalledUpdateCoins");
        coinText.text = CurrencyManager.instance.currentCurrency.ToString();
        //StartCoroutine(FetchCoins());
    }
    //IEnumerator FetchCoins()
    //{
    //    Debug.LogWarning("StartedCoroutine");
    //    //CurrencyManager.instance.FetchCoinBalance();
        
    //    Debug.LogWarning("EndedCoroutine");


    //}

}
