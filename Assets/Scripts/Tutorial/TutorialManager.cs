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
    private Vector3 worldPosition;
    private void Start()
    {
        DefineWorldWidth();
    }

    void Update()
    {
        Debug.Log((float)tutorialStages);
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
        if (playerTutorialScript.canDoubleJump)
        {
            float scale = 0.5f;
            if (playerTutorialScript.gameObject.transform.position.x == worldPosition.x)
            {
                scale = 0;
                Time.timeScale = scale;
                Time.fixedDeltaTime = scale * .02f;
            }
            else
            {
                Time.timeScale = scale;
                Time.fixedDeltaTime = scale * .02f;
            }
        }
    }
    private void Stage3Parameters()
    {

    }
    private void Stage4Parameters()
    {

    }
    void DefineWorldWidth()
    {
        Vector3 screenPosition = new Vector3(Screen.width/2, Screen.height/2,0);
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    }
}
