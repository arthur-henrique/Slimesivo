using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MinimapControl : MonoBehaviour
{
    [SerializeField] private Slider m_Slider;
    private GameObject finishLine;
    private string currentSceneName;
    private string tutorialSceneName = "Level_Teste";
    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }
    public void GetFinishLine()
    {
        finishLine = GameObject.FindGameObjectWithTag("FinishLine");
        m_Slider.maxValue = finishLine.transform.position.y - 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentSceneName != tutorialSceneName)
        {
            m_Slider.value = Player.Instance.gameObject.transform.position.y;
        }
        else
        {
            m_Slider.value = PlayerTutorial.Instance.gameObject.transform.position.y;
        }
        
    }
}
