using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EletricWireObstacle : MonoBehaviour
{
    [SerializeField] private int cooldownTime;
    [SerializeField] private int dealingDamageTime;
    //Visual Components
    [SerializeField] private GameObject[] crystals;
    [SerializeField] private Sprite spriteDeActivate;
    [SerializeField] private Sprite spriteActivate;
    [SerializeField] private Collider2D wireCollider;
    LineRenderer lineRenderer;
    private enum EletricWireStates
    {
        Cooldown,
        Charging,
        DealingDamage,
    }
    private EletricWireStates wireState = EletricWireStates.Cooldown;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetUpLine();
    }
    private void Start()
    {
        ChangeStates();
    }
    private void ChangeStates()
    {
        switch (wireState)
        {
            case EletricWireStates.Cooldown:
                ChangeSpriteToDeactivate();
                StartCoroutine(CooldownTimer());
                wireCollider.enabled = false;
                lineRenderer.enabled = false;
                break;
            case EletricWireStates.Charging:
                ChangeSpriteToActivate();
                StartCoroutine(ChargingTimer());
                break;
            case EletricWireStates.DealingDamage:
                StartCoroutine(DealingDamageTimer());
                wireCollider.enabled = true;
                lineRenderer.enabled = true;
                break;
        }
    }


    private void ChangeSpriteToActivate()
    {
        for (int i = 0; i < crystals.Length; i++)
        {
            SpriteRenderer atualSprite = crystals[i].GetComponent<SpriteRenderer>();
            Debug.LogWarning(i);
            atualSprite.sprite = spriteActivate;
        }
    }
    private void ChangeSpriteToDeactivate()
    {
        for (int i = 0; i < crystals.Length; i++)
        {
            SpriteRenderer atualSprite = crystals[i].GetComponent<SpriteRenderer>();
            Debug.LogWarning(i);
            atualSprite.sprite = spriteDeActivate;
        }
    }

    IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(cooldownTime);
        wireState = EletricWireStates.Charging;
        ChangeStates();
    }
    IEnumerator ChargingTimer()
    {
        yield return new WaitForSeconds(cooldownTime);
        wireState = EletricWireStates.DealingDamage;
        ChangeStates();
    }
    IEnumerator DealingDamageTimer()
    {      
        yield return new WaitForSeconds(cooldownTime);
        wireState = EletricWireStates.Cooldown;
        ChangeStates();
    }


    private void SetUpLine()
    {
        lineRenderer.positionCount = crystals.Length;
        for (int i = 0; i < crystals.Length; i++)
        {
            lineRenderer.SetPosition(i, crystals[i].transform.position);
        }
    }
}
