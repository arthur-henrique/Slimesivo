using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MinimapControl : MonoBehaviour
{
    public static MinimapControl instance;
    [SerializeField] private Slider m_Slider;
    private GameObject finishLine;
    [SerializeField]
    public string currentSceneName;
    private string tutorialSceneName = "Level_Teste";
    public bool canTrack = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        currentSceneName = null;
        currentSceneName = SceneManager.GetActiveScene().name;
    }
    public void GetFinishLine()
    {
        finishLine = GameObject.FindGameObjectWithTag("FinishLine");
        m_Slider.maxValue = finishLine.transform.position.y - 2f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (canTrack /*&& currentSceneName == tutorialSceneName*/)
        {
            if(Player.Instance != null)
                m_Slider.value = Player.Instance.gameObject.transform.position.y;
            if(PlayerTutorial.Instance != null)
                m_Slider.value = PlayerTutorial.Instance.gameObject.transform.position.y;
        }
        //else if (canTrack && currentSceneName != tutorialSceneName)
        //{
        //    m_Slider.value = PlayerTutorial.Instance.gameObject.transform.position.y;
        //}
        
    }
}
