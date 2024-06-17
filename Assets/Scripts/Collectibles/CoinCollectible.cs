using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectible : MonoBehaviour, ICurrencyCollectible
{
    private bool _isCollected = false;
    public int Value => 1;
    public string CurrencyId => "TESTCURRENCY";


    

    public void Collect()
    {
        if(!_isCollected && GameManager.instance.canTakeDamage)
        {
            _isCollected = true;
            Debug.Log("Collected Coin");
            CurrencyManager.instance.UpdateCoinAmount(Value);
            Destroy(gameObject);
        }
        
    }
}
