using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Jump variables
    [SerializeField] private float jumpForce;
    [SerializeField] private float sideForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private float jumpSameSideTimer = 1f;
   
    

    //components
    private Rigidbody2D rig;
    private TouchManager _touchManager;
    private PlayerStats playerStatsScript;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject playerSprite;

    //Wall Slide
    private bool isWallSliding;
    [SerializeField] private float maxTimeToDie;
    private float deathCounter;
    [SerializeField] private float wallStickTime;
    [SerializeField] private float wallSlideTime;
    [SerializeField] private float wallSlideSpeedMin;
    [SerializeField] private float wallSlideSpeedMax;
    private float wallSlideState;
    private bool isTouchingWall;
    [SerializeField] private LayerMask wallLayer;
    

    //Wall Jump
    private bool isWallJumping;
    private float wallJumpingCounter;
    private float wallJumpTime = 0.2f;
    private float wallJumpDurantion = 0.2f;

    public TouchManager touchManager
    {
        get { return _touchManager; }
        set { _touchManager = value; }
    }


    private void Start()
    {
        Components();
        
    }
       

    private void FixedUpdate()
    {
       WallStick();
       CheckPlayerStillAlive();
    }


       




    #region Jump

    private void Components()
    {
        rig = GetComponent<Rigidbody2D>();
        _touchManager = GetComponent<TouchManager>();
        playerStatsScript = GetComponent<PlayerStats>();
        anim = playerSprite.GetComponent<Animator>();
    }
    public bool IsOnGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);
    }
    public void Jump()
    {
        StopAllCoroutines();
        ResetPlayerRotation();
        _touchManager._touchPressAction.Disable();
        rig.gravityScale = 1;
        wallSlideState = 0;
        anim.SetInteger("AnimParameter", 1);

        playerStatsScript.isRepawning = false;
        if (_touchManager.isFacingRight && IsOnGround())
        {
            
            rig.velocity = new Vector2(sideForce, jumpForce);
           

        }
        else if(IsOnGround() && !_touchManager.isFacingRight)
        {
            
            
            rig.velocity = new Vector2(-sideForce, jumpForce);
            anim.SetInteger("AnimParameter", 2);

        }

        if(!IsOnGround() && IsWalled())
        {
            WallJump();
        
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
        rig.velocity = new Vector2(rig.velocity.x, jumpForce);
     
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
        
        while (timer < wallSlideTime)
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

}
