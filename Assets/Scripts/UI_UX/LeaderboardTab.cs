using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardTab : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private Image[] tabButtons;
    [SerializeField] private Sprite inactiveTabBG, activeTabBG;
    [SerializeField] private TMP_Text friendsText, globalText;
    private bool isTheFirstButtonActive = true;
    [SerializeField] private int inactiveTextColorRed, inactiveTextColorGreen, inactiveTextColorBlue;
    [SerializeField] private int activeTextColorRed, activeTextColorGreen, activeTextColorBlue;
    [SerializeField] private Vector2 inactiveTabButtonSize, activeTabButtonSize;

    public void SwitchToTab (int TabID)
    {

        foreach (GameObject go in tabs) //esconde ou mostra o que tem dentro de cada tab
        {
            go.SetActive (false);
        }
        tabs[TabID].SetActive (true);

        foreach (Image im in tabButtons)
        {
            im.sprite = inactiveTabBG;
            im.rectTransform.sizeDelta = inactiveTabButtonSize;
        }
        tabButtons[TabID].sprite = activeTabBG;
        tabButtons[TabID].rectTransform.sizeDelta = activeTabButtonSize;


        if (isTheFirstButtonActive == true) //minha funcao de trocar a cor dos textos
        {
            isTheFirstButtonActive = false;
            friendsText.color = new Color(activeTextColorRed / 255f, activeTextColorGreen / 255f, activeTextColorBlue / 255f);
            globalText.color = new Color(inactiveTextColorRed / 255f, inactiveTextColorGreen / 255f, inactiveTextColorBlue / 255f);
        }
        else
        {
            isTheFirstButtonActive = true;
            globalText.color = new Color(activeTextColorRed / 255f, activeTextColorGreen / 255f, activeTextColorBlue / 255f);
            friendsText.color = new Color(inactiveTextColorRed / 255f, inactiveTextColorGreen / 255f, inactiveTextColorBlue / 255f);
        }
    }

    void Update()
    {
        print(isTheFirstButtonActive);
    }
}
