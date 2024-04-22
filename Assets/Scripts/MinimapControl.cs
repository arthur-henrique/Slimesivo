using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MinimapControl : MonoBehaviour
{
    [SerializeField] private Slider m_Slider;
    private GameObject finishLine;
    // Start is called before the first frame update
    public void GetFinishLine()
    {
        finishLine = GameObject.FindGameObjectWithTag("FinishLine");
        m_Slider.maxValue = finishLine.transform.position.y - 2f;
    }

    // Update is called once per frame
    void Update()
    {
        m_Slider.value = Player.Instance.gameObject.transform.position.y;
    }
}
