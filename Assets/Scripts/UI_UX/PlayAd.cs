using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAd : MonoBehaviour
{
    [SerializeField] int amountToChangeCoins, amountToChangeTokens, amountToChangeStamina, initialCoins, initialTokens, initialStamina;

    [Tooltip("If checked, it'll add to the amount, if unchecked, it will multiply it")]
    [SerializeField] bool addOrTimes;

    public void PlayAdFunction()
    {
        if (addOrTimes)
        {
            initialCoins += amountToChangeCoins;
            initialTokens += amountToChangeTokens;
            initialStamina += amountToChangeStamina;

            print("adicionou do número initial de antes");
        }
        else
        {
            initialCoins *= amountToChangeCoins;
            initialTokens *= amountToChangeTokens;
            initialStamina *= amountToChangeStamina;

            print("multiplicou do quanto ganhou");
        }

        //total player coins += initialCoins;
        //total player tokens += initialTokens;
        //total player stamina += initialStamina;

        print("resultado das moedas: " + initialCoins + "/ resultado dos tokens: " + initialTokens + "/ resultado da stamina: " + initialStamina);
    }

}
