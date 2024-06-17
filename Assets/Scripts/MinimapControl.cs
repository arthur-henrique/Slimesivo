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
    private string currentSceneName;
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
            Destroy(this.gameObject);
        }
    }
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
        if (canTrack && currentSceneName != SceneManager.GetSceneByBuildIndex(1).name)
        {
            m_Slider.value = Player.Instance.gameObject.transform.position.y;
        }
        else if (canTrack && currentSceneName == SceneManager.GetSceneByBuildIndex(1).name)
        {
            m_Slider.value = Player.Instance.gameObject.transform.position.y;
        }
        
    }
}
