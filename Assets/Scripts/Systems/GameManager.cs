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
    private bool isAlive = true;
    private bool needsToCheckAlive = false;

    // Canvases
    public GameObject mainMenuGO;
    public GameObject hideMenuChild;

    public GameObject pauseCanvasGO;
    public GameObject heartsContainer;
    public HUDCanvasMenu pauseCanvas;

    [SerializeField] private MinimapControl minimapControl;

    // Scores and More
    [SerializeField]
    public float playerBestScoreFloat;
    public string _playerBestScore;
    public string _playerNick;
    public string _playerRank;


    // Input Enum
    public enum InputMode
    {
        Tap_Performed,
        Tap_Release,
        Swipe,

    }
    public InputMode activeInputMode = InputMode.Tap_Performed;
    private bool isInGame;
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
    private void Start()
    {
        livesAmount = 3;
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
            needsToCheckAlive = true;
            pauseCanvas.OnDamageTaken(livesAmount);
            PlayableLevelManager.Instance.AddTimeHit();
            livesAmount--;
            Debug.Log("TookDamage()");
            // Initializes the sequence of updating the UI
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

    public void SceneLoad()
    {
        if (SceneManager.GetActiveScene().name == "1 - Main Menu")
        {
            isInGame = false;
            mainMenuGO.SetActive(true);
            hideMenuChild.SetActive(true);
            pauseCanvasGO.SetActive(false);
            pauseCanvas.OnTutorialEnd();
            Time.timeScale = 1f;
        }
        else if(SceneManager.GetActiveScene().name == "2 - CampaignMap")
        {
            isInGame = false;
            hideMenuChild.SetActive(false);
            pauseCanvasGO.SetActive(false);
        }
        else
        {

            isInGame = true;
            mainMenuGO.SetActive(false);
            hideMenuChild.SetActive(false);
            pauseCanvasGO.SetActive(true);
            CurrencyManager.instance.currentCoinAmount = 0;
            livesAmount = 3;
            isAlive = true;
            pauseCanvas.OnNewLevel();
            Time.timeScale = 1f;
            minimapControl.GetFinishLine();
            heartsContainer.SetActive(true);
            int inputValue = (int)activeInputMode;
            EventsPlayer.OnsetupInputsPlayer(inputValue);


            if (SceneManager.GetActiveScene().name == "Level_Teste")
            {
                pauseCanvas.OnTutorialStart();
            }


        }
    }

    public void Victory()
    {
        HUDCanvasMenu.instance.OnWinningLevel();
        QuestManager.Instance.CheckQuests();
    }
    
}