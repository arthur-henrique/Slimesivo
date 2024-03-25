using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapStarsTotalCount : MonoBehaviour
{
    [SerializeField] private TMP_Text starsCountText;

    [Tooltip("Quantos niveis existem no mapa")]
    [SerializeField] private int howManyLevelsInMap;

    private void Update()
    {
        TotalStarsCount(); //TODO: Tirar daqui
    }

    private void TotalStarsCount()
    {
        int currentStars = 0;
        for (int i = 1; i <= howManyLevelsInMap; i++) //vai ser menor do que a quantidade de niveis no mapa
        {
            //currentStars += PlayerPrefs.GetInt(i.ToString());
            currentStars += PlayerPrefs.GetInt("Level_" + i.ToString("000"));
        }
        starsCountText.text = currentStars + "/" + howManyLevelsInMap * 3;
    }
}

