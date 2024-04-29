using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTutorialFase : MonoBehaviour
{
    [SerializeField] private int faseToChange;   
    [SerializeField] private TutorialManager tutorialManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        tutorialManager.tutorialStages = (TutorialManager.TutorialFases)faseToChange;
    }
}
