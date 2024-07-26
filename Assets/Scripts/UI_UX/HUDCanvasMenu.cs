using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using PlayerEvents;
using System;
using UnityEngine.InputSystem;
using System.Diagnostics.Contracts;
//using UnityEngine.UIElements;

public class HUDCanvasMenu : MonoBehaviour
{
    public static HUDCanvasMenu instance;

    [SerializeField] private GameObject pausePanel, optionsPanel, backgroundPanelForPause, backgroundPanelForPlaying, pauseButton, WinningPanelBG, backgroundPanelForWinning, backgroundPanelForWinningTutorial, backgroundPanelForGameOver, timerObject, heart1, heart2, heart3, clapperboardIcon;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Button retryButton/*TODO: optionsButton*/;
    [SerializeField] private GameObject mainMenuButton, winningMainMenuButton, nextLevelButton, lifeBar, skipButton;

    public static bool gameIsPaused = false; //para acessar, basta colocar: if(HUDCanvasMenu.gameIsPaused == true)
    private float timer;
    private bool resumeInProgress = false; //para tirar os multiplos cliques do resume
    //private bool canHidePanels = false; //para nao esconder no countdown
    private int currentStarNumber = 0;
    private string currentLevelName;
    [SerializeField] private GameObject[] stars;
    [SerializeField] public GameObject[] questStars;
    public Sprite[] starSprite;
    public static bool playerChoosesCountdown = true;

    public GameObject[] hideableUI;
    public GameObject minimapSlider;
    public bool canShowMinimap = true;


    //int quantoDeVida = 3; //SUBSTITUIR DEPOIS PELO INT DA VIDA DO PLAYER!!!!

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        LevelUIPreparation();

        if (!PlayerPrefs.HasKey("OptionsPreferences"))
        {
            playerChoosesCountdown = true;
            PlayerPrefs.SetInt("OptionsPreferences", Convert.ToInt32(playerChoosesCountdown));
        }
    }

    #region To call everytime a new scene is loaded, since this canvas isnt destroyed

    private void Start()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    
    private void LateUpdate()
    {
        HideShowMinimapSlider();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // This method will be called whenever a new scene is loaded
        //Debug.Log("Scene loaded: " + scene.name);

        // Call your function here
        LevelUIPreparation();
    }

    #endregion

    private void LevelUIPreparation()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        timerObject.SetActive(true);
        timerText.text = "";
        backgroundPanelForPause.SetActive(false);
        backgroundPanelForGameOver.SetActive(false);
        WinningPanelBG.SetActive(false);
        backgroundPanelForWinning.SetActive(false);
        backgroundPanelForWinningTutorial.SetActive(false);
        backgroundPanelForPlaying.SetActive(true);
        pauseButton.SetActive(true);
        heart1.SetActive(true);
        heart2.SetActive(true);
        heart3.SetActive(true);
        currentLevelName = SceneManager.GetActiveScene().name;
        minimapSlider.SetActive(false);
        canShowMinimap = true;
        minimapSlider.GetComponent<MinimapControl>().currentSceneName = "";
        if (minimapSlider.activeSelf)
            minimapSlider.SetActive(false);

    }
    public void OnEnable()
    {
    }
    public void PauseGame()
    {
        pauseButton.SetActive(false);
        retryButton.enabled = true;
        //TODO: optionsButton.enabled = true;
        //canHidePanels = true;
        //animacao de pausar
        backgroundPanelForPause.SetActive(true);
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        //if player tocar background, esconde ele
    }

    public void MainMenu()
    {
        pausePanel.SetActive(false);
        StartCoroutine(StartCountdownCoroutine(0f));
        backgroundPanelForGameOver.SetActive(false);
        backgroundPanelForWinning.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("1 - Main Menu"); //nome da cena do Main Menu
    }

    public void Retry()
    {
        //Time.timeScale = 1f;
        //pausePanel.SetActive(false);
        //backgroundPanelForGameOver.SetActive(false);
        //backgroundPanelForWinning.SetActive(false);
        //StartCoroutine(StartCountdownCoroutine(0f));

        //Reativar

        if (EnergyManager.Instance.UseEnergy(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
            Debug.Log("Not enough energy to retry");
    }
 

    public void NextLevel()
    {
        if (EnergyManager.Instance.UseEnergy(0))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            Debug.Log("Not enough energy to go to the next level");
    }

    public void ResumeGame() //comeca a contagem regressiva
    {
        if (resumeInProgress == false)
        {
            retryButton.enabled = false;
            //TODO: optionsButton.enabled = false;
            pauseButton.SetActive(true);
            //canHidePanels = false;
            resumeInProgress = true;



            if (playerChoosesCountdown = Convert.ToBoolean(PlayerPrefs.GetInt("OptionsPreferences")) == true) //se o player deixar essa opcao ativa no menu de Options
                timer = 3f; //quero 3 segundos no timer countdown
            //animacao de despausar para o countdown
            else
                timer = 0f;
            pausePanel.SetActive(false);
            StartCoroutine(StartCountdownCoroutine(timer));//countdown
        }
    }

    public void Options()
    {
        if(!optionsPanel.activeSelf)
        {
            optionsPanel.SetActive(true);
            pausePanel.SetActive(false);
        }
        else
        {
            optionsPanel.SetActive(false);
            pausePanel.SetActive(true);
        }
    }

    /// <summary>
    /// Para mostrar a contagem regressiva de quando despausa o jogo
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    IEnumerator StartCountdownCoroutine(float seconds)
    {
        while (timer > 0) //countdown
        {
            timerText.text = timer.ToString();
            yield return new WaitForSecondsRealtime(1f);
            VibrationManager.instance.VibeUI();
            timer -= 1f;
        }

        if (timer == 0)
        {
            timerText.text = "";
            backgroundPanelForPause.SetActive(false);
            resumeInProgress = false;
            Time.timeScale = 1f;
            gameIsPaused = false;
        }

        //yield return null;
    }


    /// <summary>
    /// Funcao de, enquanto pausado, deixar o dedo clicado fora do pop up da UI para aparecer o jogo
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="cull"></param>
    private void CullAllChildrenAndParent(Transform parent, bool cull)
    {
        CanvasRenderer canvasRenderer = parent.GetComponent<CanvasRenderer>();
        if (canvasRenderer != null)
        {
            canvasRenderer.cull = cull; //Set the cull property of the parent
        }

        foreach (Transform child in parent)
        {
            CanvasRenderer childCanvasRenderer = child.GetComponent<CanvasRenderer>();
            if (childCanvasRenderer != null)
            {
                childCanvasRenderer.cull = cull; //Set the cull property of each child
            }
            CullAllChildrenAndParent(child, cull); //Recursively call the function for each child
        }
    }
    //public void hideBackgroundPanel()
    //{
    //    if (canHidePanels == true)
    //        CullAllChildrenAndParent(backgroundPanelForPause.transform, true); //Set cull to true for both parent and children
    //}
    //public void showBackgroundPanel()
    //{
    //    CullAllChildrenAndParent(backgroundPanelForPause.transform, false);
    //}


    /// <summary>
    /// Para mostrar na UI a quantidade de vida do player e ativar o Game Over Panel se tiver com vida 0
    /// </summary>
    public void OnDamageTaken(int quantoDeVida)
    {
        if (quantoDeVida == 3)
        {
            heart1.SetActive(false);
            //quantoDeVida -= 1;
        }
        else if (quantoDeVida == 2)
        {
            heart2.SetActive(false);
            //quantoDeVida -= 1;
        }
        else if (quantoDeVida == 1)
        {
            heart3.SetActive(false);
            //quantoDeVida -= 1;
            //backgroundPanelForGameOver.SetActive(true);
        }
        print(quantoDeVida);
    }
    public void OnHealing(int quantoDeVida)
    {
        if (quantoDeVida == 3)
        {
            heart1.SetActive(true);
            //quantoDeVida -= 1;
        }
        else if (quantoDeVida == 2)
        {
            heart2.SetActive(true);
            //quantoDeVida -= 1;
        }
        else if (quantoDeVida == 1)
        {
            heart3.SetActive(true);
            //quantoDeVida -= 1;
            //backgroundPanelForGameOver.SetActive(true);
        }
        print(quantoDeVida);
    }
    public void OnDeath()
    {
        pauseButton.SetActive(false);
        GameManager.instance.mainMenuGO.SetActive(true);
        VibrationManager.instance.VibeDeath();
        backgroundPanelForGameOver.SetActive(true);
    }
    public void OnNewLevel()
    {
        heart1.SetActive(true);
        heart2.SetActive(true);
        heart3.SetActive(true);
        
        backgroundPanelForWinning.SetActive(false);
        backgroundPanelForGameOver.SetActive(false);
    }

    public void OnWinningLevel()
    {
        pauseButton.SetActive(false);
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            backgroundPanelForWinning.SetActive(false);
            backgroundPanelForWinningTutorial.SetActive(true);
            CurrencyManager.instance.TutorialCoinHold();
            AchievementsManager.instance.UnlockAchievement(GPGSIds.achievement_tutorial_master);

        }
        else
        {
            backgroundPanelForWinning.SetActive(true);
            backgroundPanelForWinningTutorial.SetActive(false);
            CurrencyManager.instance.SetBalance();
            GameManagerMainMenuCanvasScript.Instance.UpdateCoins();

            if (PlayableLevelManager.Instance != null)
            {
                PlayableLevelManager.Instance.CheckForScores();
            }
        }
        Time.timeScale = 0f;
        GameManager.instance.mainMenuGO.SetActive(true);
        
        backgroundPanelForGameOver.SetActive(false);
        WinningPanelBG.SetActive(true);
    }

    public void OnTutorialStart()
    {
        mainMenuButton.SetActive(false);
        winningMainMenuButton.SetActive(false);
        nextLevelButton.SetActive(false);
        lifeBar.SetActive(false);
        skipButton.SetActive(true);
    }

    public void OnTutorialEnd()
    {
        mainMenuButton.SetActive(true);
        winningMainMenuButton.SetActive(true);
        nextLevelButton.SetActive(true);
        lifeBar.SetActive(true);
        skipButton.SetActive(false);
        backgroundPanelForWinningTutorial.SetActive(false);
    }

    public void SkipedTutorial()
    {
        if (PlayerPrefs.GetInt("Level_Teste_completed") != 1)
            PlayerPrefs.SetInt("Level_Teste_completed", 1); ;
    }

    public void HideShowMinimapSlider()
    {
        if(canShowMinimap && !minimapSlider.activeSelf)
        {
            
            if (currentLevelName != "Level_Teste")
            {
                if(Player.Instance.gameObject.transform.position.y > Player.Instance.initialY + 10)
                {
                    minimapSlider.SetActive(true);
                    canShowMinimap = false;
                }
                
            }
            else if (currentLevelName == "Level_Teste")
            {
                if (PlayerTutorial.Instance.gameObject.transform.position.y > PlayerTutorial.Instance.initialY + 10)
                {
                    minimapSlider.SetActive(true);
                    canShowMinimap = false;
                }
            }
        }
    }

    #region Mostra quantas estrelas o Player conseguiu no nivel (principalmente visualmente)
    public void PressStarsButton(int _starsNumber)
    {
        currentStarNumber = _starsNumber;
        if (currentStarNumber > PlayerPrefs.GetInt(currentLevelName)) //vai salvar a pontuacao (estrelas) somente se for maior que a anterior
        {
            PlayerPrefs.SetInt(currentLevelName, _starsNumber);
        }
        //print(PlayerPrefs.GetInt(currentLevelName, _starsNumber));

        for (int i = 0; i < _starsNumber; i++)
        {
            stars[i].gameObject.GetComponent<Image>().sprite = starSprite[1];
            print("aeee");
        }
    }

    public void UpdateStarSprite(int i, int spriteIndex)
    {
        questStars[i].gameObject.GetComponent<Image>().sprite = starSprite[spriteIndex];
    }

    /// <summary>
    /// Esta funcao serve apenas para testes, ela nao estara na versao final do jogo
    /// </summary>
    public void ResetStarsButton() //TODO: deletar esta funcao
    {
        PlayerPrefs.DeleteKey(currentLevelName);
    }

    IEnumerator SetShowMinimap()
    {
        yield return new WaitForSeconds(.5f);
        canShowMinimap = true;
        if (minimapSlider.activeSelf)
            minimapSlider.SetActive(false);
    }

    #endregion

}