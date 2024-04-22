using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingConsumable : MonoBehaviour, IConsumable
{
    private bool _isConsumed = false;
    public void Consume()
    {
        if (!_isConsumed)
        {
            _isConsumed = true;
            GameManager.instance.ConsumeHealing();
            Destroy(gameObject);
        }
        
    }
}
