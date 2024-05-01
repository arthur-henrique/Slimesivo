using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplayer : MonoBehaviour
{
    public static InventoryDisplayer Instance;
    public Button[] skinButtons;
    public TMP_Text[] skinTexts;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void DisplaySkins(Dictionary<string, SkinInfo> allSkins)
    {
        int i = 0;
        foreach (KeyValuePair<string, SkinInfo> skin in allSkins)
        {
            skinButtons[i].interactable = skin.Value.Owned;
            skinTexts[i].text = skin.Value.DisplayName;
            i++;
        }
    }
}
