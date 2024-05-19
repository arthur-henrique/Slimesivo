using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using PlayerEvents;
using System;
public class TouchManager : MonoBehaviour
{
    public static TouchManager instance;
    [SerializeField] private LayerMask uiLayer;
   
    
    //Swipe Variables in inspector
    [Header("Swipe Variables")]
    [Tooltip("Distancia minima para ser considerado um swipe")]
    [SerializeField] private float minimunDistance = .2f;
    [Tooltip("Tempo máximo para ser considerado um swipe")]
    [SerializeField] private float maximumTime = 1f;
    [Tooltip("O quão errado a linha vai detectar, tipo 0 vc pode fazer na maior diagonal que vai e 1 tem que ser perfeitamente na horizontal e vertical")]
    [SerializeField, Range(0f, 1f)] private float directionThreshold = .9f;

    //Swipe Variables hide
    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;

    //Input
    private float screenSideX;
    [HideInInspector] public PlayerTouchControls inputActions;
    private Vector2 value;
    private float _worldWidth;
    private Camera mainCamera;


    //Direction
   [HideInInspector] public float rightCounter;
   [HideInInspector] public float leftCounter;

    private bool _isFacingRight;

    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        set { _isFacingRight = value; }
    }
    public float WorldWidth
    {
        get { return _worldWidth; } 
        set { _worldWidth = value; }
    }
   


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DefineWorldWidth();
        
    }
    private void Start()
    {
        Components();
    }
    private void OnEnable()
    {
        EventsPlayer.SetupInputsPlayer += CheckActiveInputMode;
        EventsPlayer.ClearAllEventsvariables += ClearEventsReferences;
    }

    private void ClearEventsReferences()
    {
        EventsPlayer.SetupInputsPlayer -= CheckActiveInputMode;
        EventsPlayer.ClearAllEventsvariables -= ClearEventsReferences;
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
        inputActions = new PlayerTouchControls();
        mainCamera = Camera.main;
        EventsPlayer.OnsetupInputsPlayer(GameManager.instance.activeInputMode);
        
    }


    public void CheckActiveInputMode(Enum inputType)
    {
        switch (inputType)
        {
            case GameManager.InputMode.Tap_Performed:
                inputActions.Touch.TouchPress.performed += TouchPress;
                break;
            case GameManager.InputMode.Tap_Release:
                inputActions.Touch.TouchPress.canceled += TouchPress;
                break;
            case GameManager.InputMode.Swipe:
                inputActions.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
                inputActions.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
                break;
        }
                                                 
    }
    private void OnDisable()
    {
        inputActions.Touch.TouchPress.canceled -= TouchPress;
        inputActions.Touch.TouchPress.performed -= TouchPress;
        inputActions.Touch.PrimaryContact.started -= ctx => StartTouchPrimary(ctx);
        inputActions.Touch.PrimaryContact.canceled -= ctx => EndTouchPrimary(ctx);
        ClearEventsReferences();
    }
    #endregion

    #region Swipe
    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        startPosition = ScreenToWorld(mainCamera, inputActions.Touch.PrimaryPosition.ReadValue<Vector2>());
        startTime = (float)context.startTime; ;
    }
    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
       endPosition = ScreenToWorld(mainCamera, inputActions.Touch.PrimaryPosition.ReadValue<Vector2>());
        endTime = (float)context.time;

        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if(Vector3.Distance(startPosition, endPosition)>= minimunDistance && (endTime - startTime) <= maximumTime)
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }
    private void SwipeDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.right,direction) > directionThreshold)
        {
            Debug.Log("swipe Right");
            _isFacingRight = true;
            EventsPlayer.OnJumpRight();

        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log("swipe Left");
            _isFacingRight = false;
            EventsPlayer.OnJumpLeft();
        }
        else if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Debug.Log("swipe Up");
            if ((GetPlayerPositionInScreen(0.2f, true)))
            {
                if (Player.Instance.IsWalled())
                {
                    _isFacingRight = true;
                    EventsPlayer.OnJumpSameSide(_isFacingRight);

                }
                else
                {
                    _isFacingRight = true;
                    EventsPlayer.OnJumpRight();

                }
            }
            else if (GetPlayerPositionInScreen(-0.2f, false))
            {
                if (Player.Instance.IsWalled())
                {
                    _isFacingRight = false;
                    EventsPlayer.OnJumpSameSide(_isFacingRight);

                }
                else
                {
                    _isFacingRight = false;
                    EventsPlayer.OnJumpLeft();
                }
            }
        }
    }
    #endregion

    #region TapInput
    private void TouchPress(InputAction.CallbackContext context)
    {
       //Pega o valor do pixel onde o player clica e divide pelo valor total de pixeis da tela
        value = inputActions.Touch.TouchPosition.ReadValue<Vector2>();
        screenSideX = value.x / Camera.main.pixelWidth;

                   

        if (!PointerIsUIHit(value)) 
            //Dai checa pra ver se foi esquerda ou direita, maior q 0.5 direita menor esquerda
            if (screenSideX > 0.5)
            {
                if((GetPlayerPositionInScreen(0.2f, true)))
                {
                    if (Player.Instance.IsWalled())
                    {
                        _isFacingRight = true;
                        EventsPlayer.OnJumpSameSide(_isFacingRight);

                    }
                    else
                    {
                        _isFacingRight = true;
                        EventsPlayer.OnJumpRight();
                    }

                }
                else
                {
                    _isFacingRight = true;
                    EventsPlayer.OnJumpRight();
                }
               

            }
            else
            {
                if (GetPlayerPositionInScreen(-0.2f,false))
                {
                    if (Player.Instance.IsWalled())
                    {
                        _isFacingRight = false;
                        EventsPlayer.OnJumpSameSide(_isFacingRight);

                    }
                    else
                    {
                        _isFacingRight = false;
                        EventsPlayer.OnJumpRight();
                    }
                }
                else
                {
                    _isFacingRight = false;
                    EventsPlayer.OnJumpLeft();

                }
            }
    }
    #endregion

    #region Detection
    private Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
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
