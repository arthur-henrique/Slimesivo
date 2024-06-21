using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectible : MonoBehaviour, ICurrencyCollectible
{
    private bool _isCollected = false;
    public int Value => 1;
    public string CurrencyId => "TESTCURRENCY";
    [SerializeField] private AudioClip[] collectSounds;


    

    public void Collect()
    {
        if(!_isCollected && GameManager.instance.canTakeDamage)
        {
            _isCollected = true;
            Debug.Log("Collected Coin");
            CurrencyManager.instance.UpdateCoinAmount(Value);
            SoundFXManager.Instance.PlayRandomSoundFXClip(collectSounds, transform, 1f);
            Destroy(gameObject);
        }
        
    }
}
