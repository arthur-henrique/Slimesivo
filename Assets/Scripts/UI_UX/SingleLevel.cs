using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleLevel : MonoBehaviour
{
    private int currentStarNumber = 0;
    private int currentLevelIndex;

    private void Start()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        print(currentLevelIndex);
    }
    public void BackButton()
    {
        //SceneManager.LoadScene("UI_Test_Map");
        PlayerPrefs.DeleteKey("Lv" + currentLevelIndex);
    }

    public void PressStarsButton(int _starsNumber)
    {
        currentStarNumber = _starsNumber;
        if (currentStarNumber > PlayerPrefs.GetInt("Lv" + currentLevelIndex)) //vai salvar a pontuacao (estrelas) somente se for maior que a anterior
        {
            PlayerPrefs.SetInt("Lv" + currentLevelIndex, _starsNumber);
        }
        print(PlayerPrefs.GetInt("Lv" + currentLevelIndex, _starsNumber));

    }
}
