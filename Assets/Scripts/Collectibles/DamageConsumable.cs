using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageConsumable : MonoBehaviour, IConsumable
{
    public void Consume()
    {
        GameManager.instance.TookDamage();
        //Destroy(gameObject);
    }
}
