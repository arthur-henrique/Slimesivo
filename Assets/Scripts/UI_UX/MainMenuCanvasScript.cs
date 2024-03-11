using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCanvasScript : MonoBehaviour
{
    public GameObject infinityButton, campaignButton, rightChangeButton, leftChangeButton, campaignMode;
    

    void Awake()
    {
        leftChangeButton.SetActive(false);
        rightChangeButton.SetActive(true);
        infinityButton.SetActive(true);
        campaignButton.SetActive(false);
        campaignMode.SetActive(false);
     
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

    public void CampaignMode()
    {
        campaignMode.SetActive (true);

    }

    public void ExitCampaignMode()
    {
        campaignMode.SetActive(false);
    }
}
