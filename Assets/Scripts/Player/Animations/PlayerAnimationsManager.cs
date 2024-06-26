using PlayerEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsManager : MonoBehaviour
{
    //Components
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>(); 
        if(GameManager.instance.playerAnimator != null)
        {
            anim.runtimeAnimatorController = GameManager.instance.playerAnimator;
        }
    }

    private void OnEnable()
    {
        EventsPlayer.JumpRight += JumpRightAnimation;
        EventsPlayer.JumpLeft += JumpLeftAnimation;
        EventsPlayer.JumpSameSide += JumpSameSideAnimation;
        EventsPlayer.Damage += DamageAnimation;
    }


    private void OnDisable()
    {
        ClearEventsReferences();
    }
    private void ClearEventsReferences()
    {
        EventsPlayer.JumpRight -= JumpRightAnimation;
        EventsPlayer.JumpLeft -= JumpLeftAnimation;
        EventsPlayer.JumpSameSide -= JumpSameSideAnimation;
        EventsPlayer.Damage -= DamageAnimation;
    }


    private void JumpRightAnimation()
    {
        
        anim.SetInteger("AnimParameter", 1);
    }
    private void JumpLeftAnimation()
    {
        anim.SetInteger("AnimParameter", 2);
    }
    private void JumpSameSideAnimation(bool isFacingRight)
    {
        if(anim.GetInteger("AnimParameter") != 5|| anim.GetInteger("AnimParameter") != 6)
        {
            if (isFacingRight)
            {
                anim.SetInteger("AnimParameter", 5);
            }
            else
            {
                Debug.Log(isFacingRight);
                anim.SetInteger("AnimParameter", 6);
            }
        }
      
        
    }
    private void DamageAnimation(Collider2D collider, Transform player)
    {
        anim.SetInteger("AnimParameter", 4);
    }
}
