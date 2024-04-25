using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EletricWireObstacle : MonoBehaviour
{
    [SerializeField] private int cooldownTime;
    [SerializeField] private int dealingDamageTime;
    //Visual Components
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Collider2D collider;
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
                StartCoroutine(CooldownTimer());
                collider.enabled = false;
                break;
            case EletricWireStates.DealingDamage:
                StartCoroutine(DealingDamageTimer());
                collider.enabled = true;
                break;
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
