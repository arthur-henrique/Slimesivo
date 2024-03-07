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
    private InputAction _touchPressAction;
    
    private Player playerScript;

    //Direction
    private float _rightCounter;
    private float _leftCounter;

    private bool _isFacingRight;

    public bool isFacingRight
    {
        get { return _isFacingRight; }
        set { _isFacingRight = value; }
    }

   

    public InputAction touchPressAction
    {
        get { return _touchPressAction; }
        set { _touchPressAction = value; }
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
            _isFacingRight = true;
            playerScript.Jump();
            Debug.Log("Direita");
        }
        else
        {
            _isFacingRight = false;
            playerScript.Jump();
            Debug.Log("Esquerda");
        }
        
        
        
    }

    #endregion


}
