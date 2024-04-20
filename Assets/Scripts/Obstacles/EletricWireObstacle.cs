using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EletricWireObstacle : MonoBehaviour
{
    [SerializeField] private int cooldownTime;
    [SerializeField] private int dealingDamageTime;
    //Visual Components
    [SerializeField] private SpriteRenderer sprite;
    private enum EletricWireStates
    {
        Cooldown,
        DealingDamage,
    }
    private EletricWireStates wireState = EletricWireStates.Cooldown;
    
    private void FixedUpdate()
    {
        switch (wireState)
        {
            case EletricWireStates.Cooldown:
                StartCoroutine("CooldownTimer");
                break;
            case EletricWireStates.DealingDamage:
                StartCoroutine("DealingDamageTimer");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player playerChecker = collision.GetComponent<Player>();
        if (playerChecker != null)
        {
            if (wireState == EletricWireStates.DealingDamage)
            {
                Debug.Log("Damage");
            }
        }
    }



    IEnumerator CooldownTimer()
    {
        sprite.color = Color.white;
        yield return new WaitForSeconds(cooldownTime);
        wireState = EletricWireStates.DealingDamage;
    }
    IEnumerator DealingDamageTimer()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(cooldownTime);
        wireState = EletricWireStates.Cooldown;
    }
}
