using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private bool unlocked = false;
    public Image lockImage;
    public GameObject[] stars;

    private void Update()
    {
        UpdateLevelImage();
    }

    private void UpdateLevelImage()
    {
        if (unlocked == false)
        {
            lockImage.gameObject.SetActive(true);

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(false);
            }
        }
        else
        {
            lockImage.gameObject.SetActive(false);

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(true);
            }
        }
    }

    public void GoToLevel(int levelIndex)
    {
        if (unlocked)
        {
            SceneManager.LoadScene("UI_Test_Level " + levelIndex); //MUDAR ESTE NOME DEPOIS
        }
    }
}
