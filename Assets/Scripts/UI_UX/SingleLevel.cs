using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleLevel : MonoBehaviour
{
    public void BackButton()
    {
        SceneManager.LoadScene("UI_Test_Map");
    }

    public void PressStarsButton(int _StarsNumber)
    {

    }
}
