using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
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
    [SerializeField] private GameObject damageCamera;
    [SerializeField] private GameObject damageCameraSprite;
    private Vector3 worldPosition;

   [HideInInspector] public bool doubleJumpOpen;
    [HideInInspector] public bool canWallSlde;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        DefineWorldWidth();
    }

   public void ManagerTutorialStage()
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
        doubleJumpOpen = true;
    }
    private void Stage3Parameters()
    {
        canWallSlde = true;
    }
    private void Stage4Parameters()
    {
        damageCamera.SetActive(false);
        damageCameraSprite.SetActive(true);
    }
    void DefineWorldWidth()
    {
        Vector3 screenPosition = new Vector3(Screen.width/2, Screen.height/2,0);
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    }
}
