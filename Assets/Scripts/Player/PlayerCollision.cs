using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICurrencyCollectible currencyCollectible = collision.gameObject.GetComponent<ICurrencyCollectible>();
        IConsumable consumableCollectible = collision.gameObject.GetComponent<IConsumable>();
        if(currencyCollectible != null || consumableCollectible != null)
        {
            if (currencyCollectible != null)
            {
                currencyCollectible.Collect();
                VibrationManager.Instance.CallVibration(10);
                return;
            }
            else if (consumableCollectible != null)
            {
                consumableCollectible.Consume();
                return;
            }
        }
        else if (collision.CompareTag("FinishLine"))
        {
            GameManager.instance.Victory();
        }
        
    }
}
