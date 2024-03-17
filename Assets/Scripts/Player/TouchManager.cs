using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class TouchManager : MonoBehaviour
{
    //Input
    private PlayerInput playerInput;
    private float screenSide;
    private InputAction touchPositionAction;
    [HideInInspector] public InputAction _touchPressAction;
    
    private Player playerScript;

    //Direction
   [HideInInspector] public float rightCounter;
   [HideInInspector] public float leftCounter;

    private bool _isFacingRight;

    public bool isFacingRight
    {
        get { return _isFacingRight; }
        set { _isFacingRight = value; }
    }

   


    private void Awake()
    {
        Components();
    }
    #region Inputs
    private void Components()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();

        touchPositionAction = playerInput.actions["TouchPosition"];
        _touchPressAction = playerInput.actions["TouchPress"];
        playerScript = GetComponent<Player>();
    }
    private void OnEnable()
    {
        _touchPressAction.performed += TouchPress;
        
    }
    private void OnDisable()
    {
        _touchPressAction.performed -= TouchPress;
    }

    
    private void TouchPress(InputAction.CallbackContext context)
    {
       //Pega o valor do pixel onde o player clica e divide pelo valor total de pixeis da tela
        Vector2 value = touchPositionAction.ReadValue<Vector2>();
        screenSide = value.x / Camera.main.pixelWidth;
        
        
        //Dai checa pra ver se foi esquerda ou direita, maior q 0.5 direita menor esquerda
        if(screenSide > 0.5)
        {
            switch (rightCounter)
            {
                case 0:
                    _isFacingRight = true;
                    playerScript.Jump();
                    rightCounter++;
                    leftCounter = 0;
                    break;
                case 1:
                    _isFacingRight = true;
                    playerScript.JumpSameSide();
                    break;
                   
            }
           
            
        }
        else
        {
            switch (leftCounter)
            {
                case 0:
                    _isFacingRight = false;
                    playerScript.Jump();
                    rightCounter = 0;
                    leftCounter++;
                    break;
                case 1:
                    _isFacingRight = false;
                    playerScript.JumpSameSide();
                    break;
                    
            }
        }
        
        
        
    }

    #endregion


}
