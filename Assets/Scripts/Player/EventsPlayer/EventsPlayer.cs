using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerEvents
{
    public static class EventsPlayer
    {
        public static event UnityAction JumpRight;
        public static void OnJumpRight() => JumpRight?.Invoke();
        
        public static event UnityAction JumpLeft;
        public static void OnJumpLeft() => JumpLeft?.Invoke();


        public static event UnityAction<bool> JumpSameSide;
        public static void OnJumpSameSide(bool isFacingRight) => JumpSameSide?.Invoke(isFacingRight);

        public static event UnityAction<Collider2D, Transform> Damage;
        public static void OnTakingDamage(Collider2D obstacleCollision, Transform player) => Damage?.Invoke(obstacleCollision,player);

        public static event UnityAction<int> SetupInputsPlayer;
        public static void OnsetupInputsPlayer(int inputType) => SetupInputsPlayer?.Invoke(inputType);

        public static event UnityAction<bool> WallStick;
        public static void OnWallStick(bool validCollision) => WallStick?.Invoke(validCollision);
       
        public static event UnityAction Grounded;
        public static void OnGround() => Grounded?.Invoke();

    }
}

