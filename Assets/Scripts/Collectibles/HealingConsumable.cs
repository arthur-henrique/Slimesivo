using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingConsumable : MonoBehaviour, IConsumable
{
    public void Consume()
    {
        GameManager.instance.ConsumeHealing();
        Destroy(gameObject);
    }
}
