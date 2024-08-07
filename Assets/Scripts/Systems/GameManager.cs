using PlayerEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;


    // Health and Values associated to being alive
    public int livesAmount = 0;
    private int maxLivesAmount = 3;
    private float damageCooldown = 2f;
    private bool isAlive = true;
    public bool canTakeDamage = true;
    public bool canCollect = true;
    private bool needsToCheckAlive = false;

    // Canvases
    public GameObject mainMenuGO;
    public GameObject hideMenuChild;

    public GameObject pauseCanvasGO;
    public GameObject heartsContainer;
    public HUDCanvasMenu pauseCanvas;

    public TMP_Text timerText;
    public TMP_Text scoreText;

    [SerializeField] private MinimapControl minimapControl;

    // Scores and More
    [SerializeField]
    public float playerBestScoreFloat;
    public string _playerBestScore;
    public string _playerNick;
    public string _playerRank;


    // Animators
    public AnimatorOverrideController playerAnimator;
    public AnimatorOverrideController menuPlayerAnimator;

    // Input Enum
    public enum InputMode
    {
        Tap_Performed,
        Tap_Release,
        Swipe,

    }
    public InputMode activeInputMode = InputMode.Tap_Performed;
    private bool isInGame;

    private bool needsToSyncDataToCloud = false;

    //Sounds
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip victorySound;

    [SerializeField] private SoundMixerManager soundMixerManager;
    void Awake()
    {
        // Check if instance already exists
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

        // Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        // Call your initialization functions here.
    }

    // Other functions of the GameManager
    private async void Start()
    {
        livesAmount = 3;
        await CloudSaveManager.Instance.LoadGameDataFromCloud();
        //if (SceneManager.GetActiveScene().name == "1 - Main Menu" || SceneManager.GetActiveScene().name == "SystemTesting")
        //{
        //    pauseCanvasGO.SetActive(false);
        //}
    }
    private void OnEnable()
    {
        //EventsPlayer.SetupInputsPlayer += SwitchInput;
        //EventsPlayer.ClearAllEventsvariables += ClearEventsReferences;
    }

    private void ClearEventsReferences()
    {
       // EventsPlayer.SetupInputsPlayer -= SwitchInput;
        //EventsPlayer.ClearAllEventsvariables -= ClearEventsReferences;
    }

    private void OnDisable()
    {
        ClearEventsReferences();
    }
    private void Update()
    {
        if (needsToCheckAlive && isAlive)
        {
            needsToCheckAlive = false;
            if (livesAmount >0)
            {
                print("Player has: " + livesAmount + " lives remaining.");
                // Calls a function that decreases the life of the player
            }
            else
            {
                isAlive = false;
                // Calls the function that initializes the death sequence
                Debug.Log("Player has Died");
                Time.timeScale = 0f;
                SoundFXManager.Instance.PlaySoundFXClip(loseSound, transform, 1f);
                pauseCanvas.OnDeath();
            }
        }
    }

    public void ConsumeHealing()
    {
        Debug.Log("ConsumeHealing()");
        Debug.Log(livesAmount);
        if (livesAmount < maxLivesAmount)
        {
            livesAmount++;
            pauseCanvas.OnHealing(livesAmount);
            Debug.Log("Recovered Health");
        }
        else
        {
            Debug.Log("Health was full");
        }
        // Initializes the sequence of updating the UI
    }

    public void TookDamage()
    {
        if(Player.Instance != null && Player.Instance.hitCounter == 1)
        {
            if(canTakeDamage)
            {
                StartCoroutine(DamageCooldown());
                needsToCheckAlive = true;
                pauseCanvas.OnDamageTaken(livesAmount);
                PlayableLevelManager.Instance.AddTimeHit();
                livesAmount--;
                Debug.Log("TookDamage()");
            }


        } else if(Player.Instance == null)
        {
            //needsToCheckAlive = true;
            //pauseCanvas.OnDamageTaken(livesAmount);
            //PlayableLevelManager.Instance.AddTimeHit();
            //livesAmount--;
            Debug.Log("TookDamage()");
        }


    }

    public void SwitchInput(int inputValue)
    {
        if (activeInputMode == InputMode.Tap_Performed)
        {
            activeInputMode = InputMode.Swipe;
            print("true");
            if(isInGame) 
            {
                inputValue = (int)activeInputMode;
                //EventsPlayer.OnsetupInputsPlayer(inputType);
    
            }
        }
        else
        {
            activeInputMode = InputMode.Tap_Performed;
            print("false");
            if (isInGame)
            {
                inputValue = (int)activeInputMode;
                //EventsPlayer.OnsetupInputsPlayer(inputType);
                
            }
        }
    }
    public void GetPlayerRanking(TMP_Text playersNick, TMP_Text playerScore, TMP_Text playerRank)
    {
        playersNick.text = _playerNick;
        playerScore.text = _playerBestScore;
        playerRank.text = _playerRank;
    }

    public async void SceneLoad()
    {
        if (needsToSyncDataToCloud)
        {
            needsToSyncDataToCloud = false;
            await CloudSaveManager.Instance.SyncGameDataToCloud();
        }
        soundMixerManager.SetElements();
        if (SceneManager.GetActiveScene().name == "1 - Main Menu")
        {
            isInGame = false;
            minimapControl.canTrack = false;
            mainMenuGO.SetActive(true);
            hideMenuChild.SetActive(true);
            GameManagerMainMenuCanvasScript.Instance.ShowPurchaseSigns();
            pauseCanvasGO.SetActive(false);
            pauseCanvas.OnTutorialEnd();
            Time.timeScale = 1f;

            
        }
        else if(SceneManager.GetActiveScene().name == "2 - CampaignMap")
        {
            isInGame = false;
            minimapControl.canTrack = false;
            hideMenuChild.SetActive(false);
            GameManagerMainMenuCanvasScript.Instance.HidePurchaseSigns();
            pauseCanvasGO.SetActive(false);
        }
        else
        {

            isInGame = true;
            canTakeDamage = true;
            canCollect = true;
            minimapControl.canTrack = true;
            mainMenuGO.SetActive(false);
            hideMenuChild.SetActive(false);
            GameManagerMainMenuCanvasScript.Instance.HidePurchaseSigns();
            pauseCanvasGO.SetActive(true);
            CurrencyManager.instance.currentCoinAmount = 0;
            CurrencyManager.instance.levelCoinAmount = 0;
            CurrencyManager.instance.UpdateCoinAmount(0);
            QuestingDictionary.Instance.InitialColorSet();
            RewardedAdExample.instance._showAdButton.interactable = true;
            timerText.text = "0.00";

            livesAmount = 3;
            isAlive = true;
            pauseCanvas.OnNewLevel();
            Time.timeScale = 1f;
            minimapControl.GetFinishLine();
            heartsContainer.SetActive(true);
            int inputValue = (int)activeInputMode;
            EventsPlayer.OnsetupInputsPlayer(inputValue);

            

            for (int i = 0; i < HUDCanvasMenu.instance.questStars.Length; i++)
            {
                HUDCanvasMenu.instance.UpdateStarSprite(i, 0);
            }

            if (SceneManager.GetActiveScene().name == "Level_Teste")
            {
                EventsTutorialPlayer.OnsetupInputsPlayerTutorial(inputValue);
                pauseCanvas.OnTutorialStart();
            }


        }
    }

    public void Victory()
    {
        VibrationManager.instance.VibeWin();
        QuestManager.Instance.CheckQuests();
        AdsInitializer.instance.LoadAds();
        RewardedAdExample.instance.LoadAd();
        HUDCanvasMenu.instance.OnWinningLevel();
        SoundFXManager.Instance.PlaySoundFXClip(victorySound, transform, 1f);

        //if (PlayableLevelManager.Instance != null)
        //{
        //    PlayableLevelManager.Instance.CheckForScores();
        //}

        needsToSyncDataToCloud = true;
    }
    
    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        canCollect = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }
}