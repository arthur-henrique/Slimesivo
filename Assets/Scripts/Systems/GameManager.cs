using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;

    // Health and Values associated to being alive
    private int livesAmount = 0;
    private int maxLivesAmount = 4;
    private bool isAlive = true;
    private bool needsToCheckAlive = false;

    // Canvases
    public GameObject pauseCanvasGO;
    public HUDCanvasMenu pauseCanvas;

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
        //if (SceneManager.GetActiveScene().name == "1 - Main Menu")
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
        needsToCheckAlive = true;
        pauseCanvas.OnDamageTaken(livesAmount);
        livesAmount--;
        Debug.Log("TookDamage()");
        // Initializes the sequence of updating the UI

    }

    public void SceneLoad()
    {
        if (SceneManager.GetActiveScene().name == "1 - Main Menu")
        {
            pauseCanvasGO.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            pauseCanvasGO.SetActive(true);
            livesAmount = 3;
            isAlive = true;
            pauseCanvas.OnNewLevel();
            Time.timeScale = 1f;
        }
    }

    
}
