using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectible : MonoBehaviour, ICurrencyCollectible
{
    public int Value => 2;
    public string CurrencyId => "TESTCURRENCY";


    

    public void Collect()
    {
        CurrencyManager.instance.UpdateCoinAmount(Value);
    }
}
