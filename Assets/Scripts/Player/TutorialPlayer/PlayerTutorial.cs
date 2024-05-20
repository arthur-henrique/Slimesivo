using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerTutorial : MonoBehaviour
{
    public static PlayerTutorial Instance;

    [Header("Jump variables")]
    [Tooltip("O quão alto o player pode pular no primeiro pulo")]
    [SerializeField] private float firstJumpForce;
    private float sideForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpSameSideTimer = 1f;
    private bool isJumping;
    public bool canDoubleJump;


    //components
    private Rigidbody2D rig;
    private TouchManagerTutorial _touchManager;
    private PlayerStatsTutorial playerStatsScript;
    private Animator anim;
    [Header("components")]
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private TutorialManager tutorialManagerScript;
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
    public TouchManagerTutorial touchManager
    {
        get { return _touchManager; }
        set { _touchManager = value; }
    }


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Components();
        DefinePlayerSpeed();
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
        _touchManager = GetComponent<TouchManagerTutorial>();
        playerStatsScript = GetComponent<PlayerStatsTutorial>();
        anim = GetComponentInChildren<Animator>();
        playerCollider = GetComponent<Collider2D>();
        originalGravity = rig.gravityScale;
         
    }
    public bool IsOnGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);
    }
    public void JumpManager()
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
        anim.SetInteger("AnimParameter", 1);

        
        if (_touchManager.IsFacingRight && IsOnGround())
        {
            _touchManager.inputActions.Disable();
            rig.velocity = new Vector2(sideForce, firstJumpForce);
           

        }
        else if(IsOnGround() && !_touchManager.IsFacingRight)
        {

            _touchManager.inputActions.Disable();
            rig.velocity = new Vector2(-sideForce, firstJumpForce);
            anim.SetInteger("AnimParameter", 2);

        }

        if(!IsOnGround() && IsWalled())
        {
            WallJump();
            if (tutorialManagerScript.tutorialStages == TutorialManager.TutorialFases.Stage2)
            {
                canDoubleJump = true;
            }
            else if(tutorialManagerScript.tutorialStages == TutorialManager.TutorialFases.Stage1)
            {
                _touchManager.inputActions.Disable();
            }

        }

        if(!IsOnGround() && !IsWalled() && doubleJumpCounter < doubleJumpMaxCounter)
        {
           if(tutorialManagerScript.doubleJumpOpen)
           {
              DoubleJump();
           }
            
        }
        playerStatsScript.isRepawning = false;    
            
    }
    public void JumpSameSide()
    {
        StopAllCoroutines();
        ResetPlayerRotation();
        canApplyGravity = false;
        anim.SetInteger("AnimParameter", 5);
        _touchManager.inputActions.Disable();
        isWallJumping = true;
        playerStatsScript.isRepawning = false;
        if (!_touchManager.IsFacingRight)
        {
            anim.SetInteger("AnimParameter", 6);
        }

        wallSlideStates = WallSlideStates.WallStick;
        rig.gravityScale = originalGravity;
        rig.velocity = new Vector2(rig.velocity.x, doubleJumpForce);
     
        Invoke(nameof(StopJumpSameSide), jumpSameSideTimer);
    }
    
   private void StopJumpSameSide()
    {
        isWallJumping = false;
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
            CancelInvoke(nameof(StopWallJump));
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
            if (_touchManager.IsFacingRight)
            {
                rig.AddForce(new Vector2(sideForce , firstJumpForce * 0.8f),ForceMode2D.Impulse);
            }
            else
            {
                rig.AddForce(new Vector2(-sideForce , firstJumpForce * 0.8f), ForceMode2D.Impulse);
                anim.SetInteger("AnimParameter", 2);
            }
           
            Invoke(nameof(StopWallJump), wallJumpDurantion);
        }
    }
    private void StopWallJump()
    {
        isWallJumping = false;
        
    }

    private void DoubleJump()
    {
        if (canDoubleJump)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = .02f;
            tutorialManagerScript.tutorialStages = TutorialManager.TutorialFases.Stage3;
            canDoubleJump = false;
        }
        _touchManager.inputActions.Disable();
        rig.velocity = Vector3.zero;
        doubleJumpCounter++;
        if (_touchManager.IsFacingRight)
        {
            rig.velocity = new Vector2(sideForce, doubleJumpForce);
        }
        else 
        {
            rig.velocity = new Vector2(-sideForce, doubleJumpForce);
            anim.SetInteger("AnimParameter", 2);

        }
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
        if (tutorialManagerScript.canWallSlde)
        {
            rig.gravityScale = 1;
            wallSlideStates = WallSlideStates.WallSlideSlow;
        }
        
         
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
            KnockbackPlayer(collision);
        }
    }

    private void KnockbackPlayer(Collider2D obstacleCollision)
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
            anim.SetInteger("AnimParameter", 4);
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
            
        
       


