using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PauseCanvasMenu : MonoBehaviour
{
    public static bool gameIsPaused = false; //para acessar, basta colocar: if(PauseCanvasMenu.gameIsPaused == true)
    public GameObject pausePanel;
    public GameObject backgroundPanel;
    Animator anim;
    private bool startResumeAnimation = false;

    void Awake()
    {
        anim = pausePanel.GetComponent<Animator>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }




        print(startResumeAnimation);
    }

    public void ResumeGame()
    {
        startResumeAnimation = true;
        StartCoroutine(DelayCoroutine(0.25f));//com o tempo de duracao da animacao fade out resume
    }

    public void PauseGame()
    {
        backgroundPanel.SetActive(true);
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }


    IEnumerator DelayCoroutine(float seconds)
    {
        if (startResumeAnimation)
        {
            anim.SetTrigger("ResumeTriggerAnimation");
            yield return new WaitForSecondsRealtime(seconds);
            pausePanel.SetActive(false);
            //countdown
            backgroundPanel.SetActive(false);
            Time.timeScale = 1f;
            gameIsPaused = false;
        }

        else
        {
            yield return null;
        }

            startResumeAnimation = false;
    }

}
