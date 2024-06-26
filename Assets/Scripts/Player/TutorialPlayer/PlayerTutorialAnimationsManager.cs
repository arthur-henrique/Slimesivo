using PlayerEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorialAnimationsManager : MonoBehaviour
{
    //Components
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>(); 
    }

    private void OnEnable()
    {
        EventsTutorialPlayer.JumpRightTutorial += JumpRightAnimation;
        EventsTutorialPlayer.JumpLeftTutorial += JumpLeftAnimation;
        EventsTutorialPlayer.JumpSameSideTutorial += JumpSameSideAnimation;
        EventsTutorialPlayer.DamageTutorial += DamageAnimation;
    }


    private void OnDisable()
    {
        ClearEventsReferences();
    }
    private void ClearEventsReferences()
    {
        EventsTutorialPlayer.JumpRightTutorial -= JumpRightAnimation;
        EventsTutorialPlayer.JumpLeftTutorial -= JumpLeftAnimation;
        EventsTutorialPlayer.JumpSameSideTutorial -= JumpSameSideAnimation;
        EventsTutorialPlayer.DamageTutorial -= DamageAnimation;
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
