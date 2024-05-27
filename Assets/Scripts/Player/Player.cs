using PlayerEvents;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Jump variables")]
    [Tooltip("O quão alto o player pode pular no primeiro pulo")]
    [SerializeField] private float firstJumpForce;
    private float sideForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpSameSideTimer = 1f;
    private bool isJumping;
    private float initialJumpForce;
    private float jumpWallForce;


    //components
    private Rigidbody2D rig;
    private TouchManager _touchManager;
    private PlayerStats playerStatsScript;
    private Animator anim;
    [Header("components")]
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private CameraController cameraController;
    Collider2D playerCollider;

   //wall slide
    private bool isWallSliding;
    private float deathCounter;
    [Header("Wall Slide variables")]
    [Tooltip("Tempo que o player fica no estado parado na parede")]
    [SerializeField] private float wallStickTime;
    [Tooltip("Tempo que o player fica no estado de deslizar mais lento na parede")]
    [SerializeField] private float wallSlideFase1Time;
    [Tooltip("Velocidade que o player desliza no estado mais lento")]
    [SerializeField] private float wallSlideSpeedMin;
    [Tooltip("Velocidade que o player desliza no estado mais rapido")]
    [SerializeField] private float wallSlideSpeedMax;
    private enum WallSlideStates
    {
        WallStick,
        WallSlideSlow,
        WallSlideFast,
    }
    private WallSlideStates wallSlideStates = WallSlideStates.WallStick;
    private bool isTouchingWall;
    [SerializeField] private LayerMask wallLayer;
    private bool isWallJumping;
    private float wallJumpingCounter;
    private float wallJumpTime = 0.2f;
    private float wallJumpDurantion = 0.1f;


    //Double Jump
    [Header("Double Jump variables")]
    [Tooltip("Quantidade de pulos que o player pode dar no ar")]
   
    [SerializeField] private int doubleJumpMaxCounter;
    private int doubleJumpCounter;
     [Tooltip("O quão alto o player pode pular no double jump")]
  
    [SerializeField] private float doubleJumpForce;



    [Header("Damage and respawn variables")]  
    [SerializeField] private LayerMask damageLayer;
    
    [Tooltip("Tempo que leva pro player respawnar depois que ele leva dano")]
    [SerializeField] private float respawnTime;
    
    [Tooltip("Tempo que leva pro player morrer se ele não encostar em nada")]
    [SerializeField] private float maxTimeToDie;

    [SerializeField] private float knockbackForce;
    private bool getHit;
    public float hitCounter;


    //Gravity variables
    private float originalGravity;
    private bool canApplyGravity;
    [SerializeField] private float gravityMultiplyer;
    public TouchManager touchManager
    {
        get { return _touchManager; }
        set { _touchManager = value; }
    }


    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    private void Start()
    {
        Components();
        DefinePlayerSpeed();
        initialJumpForce = firstJumpForce;
    }
    private void OnEnable()
    {
       EventsPlayer.JumpRight += JumpRight;
       EventsPlayer.JumpSameSide += JumpSameSide;
       EventsPlayer.JumpLeft += JumpLeft;
       EventsPlayer.Damage += KnockbackPlayer;   
    }
    private void OnDisable()
    {
        ClearEventsReferences();
    }

    private void ClearEventsReferences()
    {
       
        EventsPlayer.JumpRight -= JumpRight;
        EventsPlayer.JumpSameSide -= JumpSameSide;
        EventsPlayer.JumpLeft -= JumpLeft;
        EventsPlayer.Damage -= KnockbackPlayer;
    }
    private void FixedUpdate()
    {
        
        if (!getHit)
        {
            WallStick();
            CheckPlayerStillAlive();
        }
       
    }
            
        
       

    private void DefinePlayerSpeed()
    {
        float aspect = (float)Screen.width / Screen.height;

        float worldHeight = Camera.main.orthographicSize * 2;

        sideForce = worldHeight * aspect;
    }
    private void CheckPlayerStillAlive()
    {
        if (!IsWalled() && !IsOnGround())
        {
            deathCounter += Time.deltaTime;
            if (canApplyGravity)
            {
                rig.gravityScale += gravityMultiplyer * Time.deltaTime;
            }
            

        }
        else
        {
            deathCounter = 0;
        }

        if (deathCounter > maxTimeToDie)
        {
            
            hitCounter++;
            GameManager.instance.TookDamage();
            ResetPlayer();
            playerStatsScript.RespawnPlayer();

        }
       
    }


    #region Jump

    private void Components()
    {
        rig = GetComponent<Rigidbody2D>();
        _touchManager = GetComponent<TouchManager>();
        playerStatsScript = GetComponent<PlayerStats>();
        anim = GetComponentInChildren<Animator>();
        playerCollider = GetComponent<Collider2D>();
        originalGravity = rig.gravityScale;
        jumpWallForce = firstJumpForce * 0.8f;

    }
    public bool IsOnGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);
    }
    public void JumpManager()
    {
        Debug.Log(isWallJumping);
        JumpLogic();                          
        canApplyGravity = true; 
        if (!IsOnGround() && IsWalled())
        {
            WallJump();
           firstJumpForce = jumpWallForce;
        
        }
        else
        {
            isWallJumping = false;
            firstJumpForce = initialJumpForce;
        }

        if(!IsOnGround() && !IsWalled() && doubleJumpCounter < doubleJumpMaxCounter)
        {
            DoubleJump();
        }
        playerStatsScript.isRepawning = false;    
            
    }

    private void JumpRight()
    {
        JumpManager();
        rig.velocity = new Vector2(sideForce, firstJumpForce);
    }

   private void JumpLeft()
    {
        JumpManager();
        rig.velocity = new Vector2(-sideForce, firstJumpForce);
    }
    private void JumpSameSide(bool isFacingRight)
    {
        Debug.Log(isWallJumping);
        CancelInvoke(nameof(StopWallSlide));
        JumpLogic();
        firstJumpForce = initialJumpForce;
        canApplyGravity = false;
        isWallJumping = true;
        playerStatsScript.isRepawning = false;
        rig.velocity = new Vector2(0, firstJumpForce);
        Invoke(nameof(StopWallSlide), jumpSameSideTimer);

    }
    private void StopWallSlide()
    {
        isWallJumping = false;

    }
    private void JumpLogic()
    {
        if (playerStatsScript.isRepawning)
        {
            cameraController.CameraSettingsReset();
            playerStatsScript.isRepawning = false;
        }

        isJumping = true;
        StopAllCoroutines();
        ResetPlayerRotation();
        rig.gravityScale = originalGravity;
        wallSlideStates = WallSlideStates.WallStick;
    }
    




    #endregion

    #region WallSlide and WallJump
    public bool IsWalled()
    {
       return Physics2D.OverlapCircle(gameObject.transform.position, 0.5f, wallLayer);
    }
   
    private void WallStick()
    {
       
            if (IsWalled() && !IsOnGround() && !isWallJumping)
            {

                _touchManager.inputActions.Enable();
                canApplyGravity = true;
                doubleJumpCounter = 0;
                anim.SetInteger("AnimParameter", 3);
                isWallSliding = true;
                isJumping = false;
                switch (wallSlideStates)
                {
                    case WallSlideStates.WallStick:
                        StartCoroutine(WallStickTimer());
 
                        playerStatsScript.SaveCurrentPlayerPos();
                        break;
                    case WallSlideStates.WallSlideSlow:
                        StartCoroutine(WallSlideMinTimer());
                        break;
                    case WallSlideStates.WallSlideFast:
                        StartCoroutine(WallSlideMaxTimer());
                        break;
                }
                 
                    PlayerOrientationChecker();                 
            }
            else if (IsOnGround() && !isJumping)
            {
                 anim.SetInteger("AnimParameter", 0);
                 _touchManager.inputActions.Enable();
                 isWallSliding = false;
                 playerSprite.transform.rotation = Quaternion.Euler(0, 0, 0);

            }
            else
            {
                isWallSliding = false;
            }
            
       

    }

  

    private void WallJump()
    {
        if (isWallSliding)
        {
            deathCounter = 0;
            isWallJumping = false;
            wallJumpingCounter = wallJumpTime;
            StopCoroutine(StopWallJump());
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (wallJumpingCounter > 0f)
        {
            ResetPlayerRotation();
            isWallJumping = true;
            wallJumpingCounter = 0;
            StartCoroutine(StopWallJump());
        }
    }
    private IEnumerator StopWallJump()
    {
        yield return new WaitForSeconds(wallJumpDurantion);
        isWallJumping = false;
        
    }

    private void DoubleJump()
    {
        _touchManager.inputActions.Disable();
        rig.velocity = Vector3.zero;
        doubleJumpCounter++;
    }

    #endregion
    #region WallCoroutines
  
    IEnumerator WallStickTimer()
    {
        float timer = 0;
        rig.gravityScale = 0;
        while (timer < wallStickTime)
        {

            rig.velocity = new Vector2(0, 0);
            timer += Time.deltaTime;
            yield return null;



        }
        rig.gravityScale = 1;
        wallSlideStates = WallSlideStates.WallSlideSlow;

    }

    IEnumerator WallSlideMinTimer()
    {
        float timer = 0;
        
        while (timer < wallSlideFase1Time)
        {
            
            rig.velocity = new Vector2(0, -1* wallSlideSpeedMin);
            timer += Time.deltaTime;
            yield return null;



        }
        wallSlideStates = WallSlideStates.WallSlideFast;

    }
    IEnumerator WallSlideMaxTimer()
    {
        rig.velocity = new Vector2(0, -1 * wallSlideSpeedMax);
        yield return null;
        
    }





    #endregion

    #region Rotation amd velocity Manager
    public void ResetVelocityPlayer()
    {
        rig.velocity = new Vector2(0, 0);
    }
    private void PlayerOrientationChecker()
    {

        if (_touchManager.GetPlayerPositionInScreen(0.2f,true))
        {
            playerSprite.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            playerSprite.transform.rotation = Quaternion.Euler(0, -180, 90);
        }
    }

    private void ResetPlayerRotation()
    {
        playerSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    #endregion
    #region Collision and Damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Damage Detection
        if (((1 << collision.gameObject.layer) & damageLayer) != 0)
        {
            EventsPlayer.OnTakingDamage(collision,gameObject.transform);
        }
    }

    private void KnockbackPlayer(Collider2D obstacleCollision, Transform player)
    {
        if (hitCounter == 0)
        {
            canApplyGravity = false;
            rig.gravityScale = 1;
            isJumping = false;
            _touchManager.inputActions.Disable();
            hitCounter++;
            PlayerCollision.Instance.DamageCollision(obstacleCollision);

            Vector3 opositePosition = (transform.position - obstacleCollision.gameObject.transform.position) * -1;
            opositePosition = opositePosition.normalized;

            //Zera a velocidade atual e adiciona a nova
            rig.velocity = Vector2.zero;
            getHit = true;
            if (_touchManager.GetPlayerPositionInScreen(-0.2f, false) || _touchManager.GetPlayerPositionInScreen(0.2f, true))
            {
                Debug.Log(opositePosition.x);
                rig.AddForce(new Vector2(opositePosition.x * knockbackForce, 4), ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log(opositePosition.x);
                rig.AddForce(new Vector2(opositePosition.x * -knockbackForce, -4f), ForceMode2D.Impulse);
            }


            //Pra chamar o respawn do player
            Invoke("ResetPlayer", respawnTime);
        }
    }
    private void ResetPlayer()
    {  
      getHit = false;
      playerCollider.enabled = true;
      hitCounter--;
      playerStatsScript.RespawnPlayer();
    }
    #endregion
}
            
        
       


