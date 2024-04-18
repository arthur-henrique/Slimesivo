using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageConsumable : MonoBehaviour, IDamageDealer
{
    public void Damage()
    {
        GameManager.instance.TookDamage();
        //Destroy(gameObject);
    }
}
