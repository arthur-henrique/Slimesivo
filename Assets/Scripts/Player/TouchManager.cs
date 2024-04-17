using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class TouchManager : MonoBehaviour
{
    [SerializeField] private LayerMask uiLayer;

    //Input
    private PlayerInput playerInput;
    private float screenSideX;
    private InputAction touchPositionAction;
    [HideInInspector] public InputAction _touchPressAction;
    private Vector2 value;
    private float _worldWidth;



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
    public float worldWidth
    {
        get { return _worldWidth; } 
        set { _worldWidth = value; }
    }
   


    private void Awake()
    {
        DefineWorldWidth();
        Components();
    }
  
    #region Inputs
    void DefineWorldWidth()
    { 
       float aspect = (float)Screen.width / Screen.height;
       float worldHeight = Camera.main.orthographicSize * 2;
       _worldWidth = worldHeight * aspect;  
    }
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
        value = touchPositionAction.ReadValue<Vector2>();
        screenSideX = value.x / Camera.main.pixelWidth;

                   

        if (!PointerIsUIHit(value)) 
            //Dai checa pra ver se foi esquerda ou direita, maior q 0.5 direita menor esquerda
            if (screenSideX > 0.5)
            {
                if((GetPlayerPositionInScreen(0.2f, true)))
                {
                    _isFacingRight = true;
                    playerScript.JumpSameSide();
                }
                else
                {
                    _isFacingRight = true;
                    playerScript.JumpManager();
                }
               

            }
            else
            {
                if (GetPlayerPositionInScreen(-0.2f,false))
                {
                    _isFacingRight = false;
                    playerScript.JumpSameSide();
                }
                else
                {
                    _isFacingRight = false;
                    playerScript.JumpManager();
                    Debug.Log("esquerda");
                }
            }
    }


        

    public bool GetPlayerPositionInScreen(float range, bool greaterOrLess)
    {
        if (greaterOrLess)
        {
            return gameObject.transform.position.x / _worldWidth > range;
        }
        else
        {
            return gameObject.transform.position.x / _worldWidth < range;
        }
        
    }
    private bool PointerIsUIHit(Vector2 position)
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = position;
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        // UI Elements must have `picking mode` set to `position` to be hit
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            foreach (RaycastResult result in raycastResults)
            {
                if (result.distance == 0 && ((1 << result.gameObject.layer) & uiLayer) != 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    #endregion


}
