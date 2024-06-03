using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


    public static class EventsTutorialPlayer
    {
        public static event UnityAction JumpRightTutorial;
        public static void OnJumpRightTutorial() => JumpRightTutorial?.Invoke();
        
        public static event UnityAction JumpLeftTutorial;
        public static void OnJumpLeftTutorial() => JumpLeftTutorial?.Invoke();


        public static event UnityAction<bool> JumpSameSideTutorial;
        public static void OnJumpSameSideTutorial(bool isFacingRight) => JumpSameSideTutorial?.Invoke(isFacingRight);

        public static event UnityAction<Collider2D, Transform> DamageTutorial;
        public static void OnTakingDamageTutorial(Collider2D obstacleCollision, Transform player) => DamageTutorial?.Invoke(obstacleCollision,player);

        public static event UnityAction<int> SetupInputsPlayerTutorial;
        public static void OnsetupInputsPlayerTutorial(int inputType) => SetupInputsPlayerTutorial?.Invoke(inputType);

        public static event UnityAction WallStickTutorial;
        public static void OnWallStickTutorial() => WallStickTutorial?.Invoke();

    }


