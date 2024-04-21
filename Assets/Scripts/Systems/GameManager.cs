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
    private int maxLivesAmount = 4;
    private bool isAlive = true;
    private bool needsToCheckAlive = false;

    // Canvases
    public GameObject mainMenyGO;
    public GameObject pauseCanvasGO;
    public HUDCanvasMenu pauseCanvas;

    // Scores and More
    [SerializeField]
    public float playerBestScoreFloat;
    public string _playerBestScore;
    public string _playerNick;
    public string _playerRank;

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
        if(Player.Instance.hitCounter == 1)
        {
            needsToCheckAlive = true;
            pauseCanvas.OnDamageTaken(livesAmount);
            PlayableLevelManager.Instance.AddTimeHit();
            livesAmount--;
            Debug.Log("TookDamage()");
            // Initializes the sequence of updating the UI
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
        if (SceneManager.GetActiveScene().name == "1 - Main Menu" || SceneManager.GetActiveScene().name == "SystemTesting")
        {
            mainMenyGO.SetActive(true);
            pauseCanvasGO.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            mainMenyGO.SetActive(false);
            pauseCanvasGO.SetActive(true);
            CurrencyManager.instance.currentCoinAmount = 0;
            livesAmount = 3;
            isAlive = true;
            pauseCanvas.OnNewLevel();
            Time.timeScale = 1f;
        }
    }

    public void Victory()
    {
        HUDCanvasMenu.instance.OnWinningLevel();
        QuestManager.Instance.CheckQuests();
    }
    
}