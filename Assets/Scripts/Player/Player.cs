using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Jump variables")]
    [Tooltip("O quão alto o player pode pular no jump")]
    [SerializeField] private float jumpForce;
    private float sideForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private float jumpSameSideTimer = 1f;


    //components
    private Rigidbody2D rig;
    private TouchManager _touchManager;
    private PlayerStats playerStatsScript;
    private Animator anim;
    [Header("components")]
    [SerializeField] private GameObject playerSprite;

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
    private float wallSlideState;
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
    private float hitCounter;

    public TouchManager touchManager
    {
        get { return _touchManager; }
        set { _touchManager = value; }
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


    #region Jump

    private void Components()
    {
        rig = GetComponent<Rigidbody2D>();
        _touchManager = GetComponent<TouchManager>();
        playerStatsScript = GetComponent<PlayerStats>();
        anim = GetComponentInChildren<Animator>();
         
    }
    public bool IsOnGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);
    }
    public void JumpManager()
    {
        StopAllCoroutines();
        ResetPlayerRotation();
        rig.gravityScale = 1;
        wallSlideState = 0;
        anim.SetInteger("AnimParameter", 1);

        playerStatsScript.isRepawning = false;
        if (_touchManager.isFacingRight && IsOnGround())
        {
            _touchManager._touchPressAction.Disable();
            rig.velocity = new Vector2(sideForce, jumpForce);
           

        }
        else if(IsOnGround() && !_touchManager.isFacingRight)
        {

            _touchManager._touchPressAction.Disable();
            rig.velocity = new Vector2(-sideForce, jumpForce);
            anim.SetInteger("AnimParameter", 2);

        }

        if(!IsOnGround() && IsWalled())
        {
            WallJump();
        
        }

        if(!IsOnGround() && !IsWalled() && doubleJumpCounter < doubleJumpMaxCounter)
        {
            DoubleJump();
        }
        playerStatsScript.isRepawning = false;    
            
    }
    public void JumpSameSide()
    {
        StopAllCoroutines();
        ResetPlayerRotation();
        anim.SetInteger("AnimParameter", 1);
        _touchManager._touchPressAction.Disable();
        isWallJumping = true;
        playerStatsScript.isRepawning = false;
        if (!_touchManager.isFacingRight)
        {
            anim.SetInteger("AnimParameter", 2);
        }
       
        wallSlideState = 0;
        rig.gravityScale = 1;
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
                
                _touchManager._touchPressAction.Enable();
                doubleJumpCounter = 0;
                anim.SetInteger("AnimParameter", 3);
                isWallSliding = true;
                switch (wallSlideState)
                {
                    case 0:
                        StartCoroutine(WallStickTimer());
                        playerStatsScript.SaveCurrentPlayerPos();
                        break;
                    case 1:
                        StartCoroutine(WallSlideMinTimer());
                        break;
                    case 2:
                        StartCoroutine(WallSlideMaxTimer());
                        break;





                }
                 
                    PlayerOrientationChecker();
                   





            }
            else
            {
                isWallSliding = false;
               
            }
            
       

    }
      
       
    private void CheckPlayerStillAlive()
    {
        if (!IsWalled() && !IsOnGround())
        {
            deathCounter += Time.deltaTime;
            
        }
        else
        {
            deathCounter = 0;
        }

        if (deathCounter > maxTimeToDie)
        {
            playerStatsScript.RespawnPlayer();
            
           
        }
    }
  

    private void WallJump()
    {
        StopAllCoroutines();
        wallSlideState = 0;
        rig.gravityScale = 1;
        if (isWallSliding)
        {
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
            if (_touchManager.isFacingRight)
            {
                rig.velocity = new Vector2(sideForce, jumpForce);
                

            }
            else
            {
                rig.velocity = new Vector2(-sideForce, jumpForce);
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
        _touchManager._touchPressAction.Disable();
        rig.velocity = Vector3.zero;
        doubleJumpCounter++;
        if (_touchManager.isFacingRight )
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
        rig.gravityScale = 1;
        wallSlideState = 1;

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
        wallSlideState = 2;

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

        if (_touchManager.isFacingRight)
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
            if (hitCounter == 0)
            {
                hitCounter++;

                Vector3 opositePosition = collision.gameObject.transform.position - transform.position;
                opositePosition = (opositePosition * -1).normalized;
                Debug.Log(opositePosition);
                //Zera a velocidade atual e adiciona a nova
                rig.velocity = Vector2.zero;
                getHit = true;
                rig.AddForce(opositePosition * knockbackForce, ForceMode2D.Impulse);
                //Pra chamar o respawn do player
                anim.SetInteger("AnimParameter", 4);
                Invoke("ResetPlayer", respawnTime);
            }
            


        }
    }
    private void ResetPlayer()
    {  
      getHit = false;
        hitCounter--;
      playerStatsScript.RespawnPlayer();
    }
    #endregion
}
            
        
       


