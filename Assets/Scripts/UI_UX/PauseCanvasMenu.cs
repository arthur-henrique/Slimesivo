using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PauseCanvasMenu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool gameIsPaused = false; //para acessar, basta colocar: if(PauseCanvasMenu.gameIsPaused == true)
    public GameObject pausePanel;
    public GameObject optionsPanel;
    public GameObject backgroundPanelForPause;
    public GameObject backgroundPanelForPlaying;
    Animator anim;
    private float timer;
    public TMP_Text timerText;
    private bool resumeInProgress = false; //para tirar os multiplos cliques do resume
    private bool canHidePanels = false; //para nao esconder no countdown

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        timerText.enabled = false;
        backgroundPanelForPause.SetActive(false);
        backgroundPanelForPlaying.SetActive(true);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if (gameIsPaused == true)
            //{
            //    ResumeGame();
            //}
            //faca isso com o botao, sem a tecla
            if(gameIsPaused == false)
            {
                PauseGame();
            }
        }

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
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }


        
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
            timerText.enabled = false;
            backgroundPanelForPause.SetActive(false);
            resumeInProgress = false;
            Time.timeScale = 1f;
            gameIsPaused = false;
        }

        //yield return null;
    }
        

    public void EndingResumeAnimation()
    {
        timer = 3f; //quero 3 segundos no timer countdown
        pausePanel.SetActive(false);
        timerText.enabled = true;
        StartCoroutine(StartCountdownCoroutine(timer));//countdown
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    if (pausePanel.activeSelf && EventSystem.current.IsPointerOverGameObject())
    //    {
    //        print("Outside button");
    //        backgroundPanelForPause.GetComponent<CanvasRenderer>().cull = true;
    //    }
    //}

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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (backgroundPanelForPause.activeSelf && EventSystem.current.IsPointerOverGameObject() && canHidePanels == true)
        {
            print("clicado");
            CullAllChildrenAndParent(backgroundPanelForPause.transform, true); //Set cull to true for both parent and children
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (backgroundPanelForPause.GetComponent<CanvasRenderer>().cull == true && canHidePanels == true)
        {
            print("levantado");
            CullAllChildrenAndParent(backgroundPanelForPause.transform, false);
        }
    }


}
