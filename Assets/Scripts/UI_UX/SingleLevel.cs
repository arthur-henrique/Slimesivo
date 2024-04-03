using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleLevel : MonoBehaviour
{
    //TODO: APAGAR ESTE SCRIPT DEPOIS - ELE SO SERVE PARA TESTES DOS NIVEIS DA CAMPANHA, MAS JA ESTA IMPLEMENTADO NO SCRIPT HudCanvasMenu
    
    private int currentStarNumber = 0;
    private string currentLevelName;
    //private int currentLevelIndex;

    private void Start()
    {
        //currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        currentLevelName = SceneManager.GetActiveScene().name;
    }
    public void BackButton()
    {
        SceneManager.LoadScene("CampaignMap"); //Nome da cena do mapa
    }

    public void PressStarsButton(int _starsNumber)
    {
        currentStarNumber = _starsNumber;
        if (currentStarNumber > PlayerPrefs.GetInt(currentLevelName)) //vai salvar a pontuacao (estrelas) somente se for maior que a anterior
        {
            PlayerPrefs.SetInt(currentLevelName, _starsNumber);
        }
        print(PlayerPrefs.GetInt(currentLevelName, _starsNumber));

    }

    /// <summary>
    /// Esta funcao serve apenas para testes, ela nao estara na versao final do jogo
    /// </summary>
    public void ResetStarsButton() //TODO: deletar esta funcao
    {
        PlayerPrefs.DeleteKey(currentLevelName);
    }
}
