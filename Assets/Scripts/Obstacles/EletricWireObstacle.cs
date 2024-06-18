using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EletricWireObstacle : MonoBehaviour
{
    [SerializeField] private float cooldownTime;
    [SerializeField] private float ChargingDamageTime;
    [SerializeField] private float dealingDamageTime;
    //Visual Components
    [SerializeField] private GameObject[] crystals;
    [SerializeField] private Sprite spriteDeActivate;
    [SerializeField] private Sprite spriteActivate;
    [SerializeField] private BoxCollider2D wireCollider;
    private List<Animator> crystalsAnim = new List<Animator>();
    LineRenderer lineRenderer;

    //Particle System
    [SerializeField] private ParticleSystem chargingParticleSystem;
    [SerializeField] private GameObject[] spawnPos;
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
        for (int i = 0; i < crystals.Length; i++)
        {
            Animator anim = crystals[i].GetComponent<Animator>();
            crystalsAnim.Add(anim);
        }
        SetUpParticleSystem();
    }
    public void ChangeStates()
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
                wireCollider.enabled = true;
                lineRenderer.enabled = true;
                StartCoroutine(DealingDamageTimer());
                break;
        }
    }
    public void SpawnChargingParticles()
    {
        for (int i = 0; i < spawnPos.Length; i++)
        {
            Instantiate(chargingParticleSystem, spawnPos[i].transform);
        }
    }
    IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(cooldownTime);
        wireState = EletricWireStates.Charging;
        for (int i = 0; i < crystalsAnim.Count; i++)
        {
            crystalsAnim[i].SetInteger("ChangeState", 1);
        }
    }
    IEnumerator ChargingTimer()
    {
        yield return new WaitForSeconds(ChargingDamageTime);
        wireState = EletricWireStates.DealingDamage;
        ChangeStates();
    }
    IEnumerator DealingDamageTimer()
    {      
        yield return new WaitForSeconds(cooldownTime);
        wireState = EletricWireStates.Cooldown;
        for (int i = 0; i < crystalsAnim.Count; i++)
        {
            crystalsAnim[i].SetInteger("ChangeState", 2);
        }
    }

    #region Setups Functions
    private void SetUpLine()
    {
        lineRenderer.positionCount = crystals.Length;
        for (int i = 0; i < crystals.Length; i++)
        {
            lineRenderer.SetPosition(i, crystals[i].transform.position);
        }
        float distance = Vector2.Distance(crystals[0].transform.position, crystals[1].transform.position) / 10;

        wireCollider.size = new Vector2(distance, 0.2f);
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
    private void SetUpParticleSystem()
    {
        var main = chargingParticleSystem.main;
        main.duration = ChargingDamageTime + dealingDamageTime / 2;
        main.startLifetime = ChargingDamageTime + dealingDamageTime;

    }
    #endregion
}
