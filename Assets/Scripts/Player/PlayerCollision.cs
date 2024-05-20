using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public static PlayerCollision Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void DamageCollision(Collider2D collision)
    {
        IDamageDealer consumableCollectible = collision.gameObject.GetComponent<IDamageDealer>();
        if (consumableCollectible != null)
        {
            VibrationManager.instance.VibeDamage();
            consumableCollectible.Damage();
            Debug.Log(collision);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICurrencyCollectible currencyCollectible = collision.gameObject.GetComponent<ICurrencyCollectible>();
        IConsumable consumableCollectible = collision.gameObject.GetComponent<IConsumable>();
        if(currencyCollectible != null || consumableCollectible != null)
        {
            if (currencyCollectible != null)
            {
                currencyCollectible.Collect();
                VibrationManager.instance.VibeCollectible();
                PlayableLevelManager.Instance.AddCoinCollected();
                return;
            }
            else if (consumableCollectible != null)
            {
                consumableCollectible.Consume();
                VibrationManager.instance.VibeCollectible();
                return;
            }
        }
        else if (collision.CompareTag("FinishLine"))
        {
            GameManager.instance.Victory();
        }
        
    }
}
