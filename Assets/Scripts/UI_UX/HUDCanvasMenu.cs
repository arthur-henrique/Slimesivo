using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HUDCanvasMenu : MonoBehaviour
{
    public static HUDCanvasMenu instance;

    [SerializeField] private GameObject pausePanel, optionsPanel, backgroundPanelForPause, backgroundPanelForPlaying, backgroundPanelForWinning, backgroundPanelForGameOver, timerObject, heart1, heart2, heart3, clapperboardIcon;
    [SerializeField] private TMP_Text timerText;

    public static bool gameIsPaused = false; //para acessar, basta colocar: if(HUDCanvasMenu.gameIsPaused == true)
    Animator anim;
    private float timer;
    private bool resumeInProgress = false; //para tirar os multiplos cliques do resume
    private bool canHidePanels = false; //para nao esconder no countdown


    //int quantoDeVida = 3; //SUBSTITUIR DEPOIS PELO INT DA VIDA DO PLAYER!!!!

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        anim = gameObject.GetComponent<Animator>();
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        timerObject.SetActive(true);
        timerText.text = "";
        backgroundPanelForPause.SetActive(false);
        backgroundPanelForGameOver.SetActive(false);
        backgroundPanelForWinning.SetActive(false);
        backgroundPanelForPlaying.SetActive(true);
        heart1.SetActive(true);
        heart2.SetActive(true);
        heart3.SetActive(true);
    }

    public void PauseGame()
    {
        canHidePanels = true;
        anim.SetTrigger("PauseTriggerAnimation");
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        //Time.timeScale = 1f;
        pausePanel.SetActive(false);
        backgroundPanelForGameOver.SetActive(false);
        StartCoroutine(StartCountdownCoroutine(0f));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void ResumeGame()
    {
        if (resumeInProgress == false)
        {
            canHidePanels = false;
            resumeInProgress = true;
            anim.SetTrigger("ResumeTriggerAnimation");
        }
    }
    /// <summary>
    /// Eh ativado com o evento da Timeline da animacao do ResumeGame() - Comeca a contagem regressiva
    /// </summary>
    public void EndingResumeAnimation()
    {
        timer = 3f; //quero 3 segundos no timer countdown
        pausePanel.SetActive(false);
        StartCoroutine(StartCountdownCoroutine(timer));//countdown
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
    public void hideBackgroundPanel()
    {
        if (canHidePanels == true)
            CullAllChildrenAndParent(backgroundPanelForPause.transform, true); //Set cull to true for both parent and children
    }
    public void showBackgroundPanel()
    {
        CullAllChildrenAndParent(backgroundPanelForPause.transform, false);
    }


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
        backgroundPanelForGameOver.SetActive(true);
    }
    public void OnNewLevel()
    {
        heart1.SetActive(true);
        heart2.SetActive(true);
        heart3.SetActive(true);
    }

    public void OnWinningLevel()
    {
        backgroundPanelForGameOver.SetActive(false);
        backgroundPanelForWinning.SetActive(true);
    }

}