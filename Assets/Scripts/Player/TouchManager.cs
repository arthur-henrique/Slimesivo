using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class TouchManager : MonoBehaviour
{
    
    private PlayerInput playerInput;
    private float screenSide;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    private Player playerScript;
    private void Awake()
    {
        Components();
    }
    #region Inputs
    private void Components()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();

        touchPositionAction = playerInput.actions["TouchPosition"];
        touchPressAction = playerInput.actions["TouchPress"];
        playerScript = GetComponent<Player>();
    }
    private void OnEnable()
    {
        touchPressAction.performed += TouchPress;
    }
    private void OnDisable()
    {
        touchPressAction.performed -= TouchPress;
    }

    private void TouchPress(InputAction.CallbackContext context)
    {
        //Vector3 position = Camera.main.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
        //position.z = 0;
        //Debug.Log(position);
        Vector2 value = touchPositionAction.ReadValue<Vector2>();
        screenSide = value.x / Camera.main.pixelWidth;
        
        if(screenSide > 0.5)
        {
            playerScript.JumpRight();
            Debug.Log("Direita");
        }
        else
        {
            playerScript.JumpLeft();
            Debug.Log("Esquerda");
        }
        
        Debug.Log(screenSide);
        
    }

    #endregion


}
