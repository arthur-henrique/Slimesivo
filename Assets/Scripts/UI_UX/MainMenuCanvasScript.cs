using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvasScript : MonoBehaviour
{
    [SerializeField] private GameObject infinityButton, campaignButton, rightChangeButton, leftChangeButton, campaignMode, storePagePanel, settingsButton;
    

    void Awake()
    {
        leftChangeButton.SetActive(false);
        rightChangeButton.SetActive(true);
        infinityButton.SetActive(true);
        campaignButton.SetActive(false);
        campaignMode.SetActive(false);
        storePagePanel.SetActive(false);
        settingsButton.SetActive(true);

    }
    public void ChangeModes()
    {
        if (infinityButton.activeSelf == true)
        {
            infinityButton.SetActive(false);
            leftChangeButton.SetActive(true);
            campaignButton.SetActive(true);
            rightChangeButton.SetActive(false);
        }
        else
        {
            campaignButton.SetActive(false);
            rightChangeButton.SetActive(true);
            infinityButton.SetActive(true);
            leftChangeButton.SetActive(false);
        }
    }

    public void InfinityMode()
    {
        print("carrega modo infinito - descomentar linha");
        //SceneManager.LoadScene(/*Index da cena do modo Infinito*/);
        
    }

    public void EnterExitCampaignMode()
    {
        if (campaignMode.activeSelf == true)
            campaignMode.SetActive(false);
        else
            campaignMode.SetActive(true);

        //SceneManager.LoadScene(0);
    }

    public void EnterExitStorePage()
    {
        if (storePagePanel.activeSelf == true)
        {
            settingsButton.SetActive(true);
            storePagePanel.SetActive(false);
        }
        else
        {
            settingsButton.SetActive(false);
            storePagePanel.SetActive(true);
        }
    }

    /// <summary>
    /// Somente para a barra de coletaveis, no botao de +
    /// </summary>
    public void OnlyEnterStorePage()
    {
        settingsButton.SetActive(false);
        storePagePanel.SetActive(true);
    }
}
