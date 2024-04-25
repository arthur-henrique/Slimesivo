using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialFases
    {
        Stage1, 
        Stage2, 
        Stage3, 
        Stage4, 
    }
    public TutorialFases tutorialStages;

    //Components
    [SerializeField] private PlayerTutorial playerTutorialScript;
    [SerializeField] private TouchManagerTutorial touchManagerTutorialScript;

    //Sprites
    [SerializeField] private GameObject leftJumpsprite;
    [SerializeField] private GameObject rightJumpsprite;
    [SerializeField] private GameObject initialJumpSprite;
    void Update()
    {
        switch (tutorialStages)
        {
            case TutorialFases.Stage1:
                Stage1Parameters();
                break;
            case TutorialFases.Stage2:
                Stage2Parameters();
                break;
            case TutorialFases.Stage3:
               Stage3Parameters();
               break;
            case TutorialFases.Stage4:
                Stage4Parameters();
                break;
        }
    }
    private void Stage1Parameters()
    {

        if (playerTutorialScript.IsWalled())
        {
            initialJumpSprite.SetActive(false);

             if (touchManagerTutorialScript.GetPlayerPositionInScreen(0.2f, true))
            {
                leftJumpsprite.SetActive(false);
                rightJumpsprite.SetActive(true);
                
            }
             if(touchManagerTutorialScript.GetPlayerPositionInScreen(-0.2f, false))
            {
                rightJumpsprite.SetActive(false);
                leftJumpsprite.SetActive(true);
            }
        }
    }
    private void Stage2Parameters()
    {

    }
    private void Stage3Parameters()
    {

    }
    private void Stage4Parameters()
    {

    }
}
