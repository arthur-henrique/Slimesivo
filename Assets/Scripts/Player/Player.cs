using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Jump variables
    [SerializeField] private float jumpForce;
    [SerializeField] private float sideForce;
    [SerializeField] private Transform groundCheck;
    private int groundLayer;
    private bool isGrounded = true;

    //components
    private Rigidbody rig;
    private TouchManager touchManager;

    //Wall Slide
    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 5f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallStickTime;
    private bool isTouchingWall;
    private int wallLayer;
    private Collider[] arrColliders;

    //Wall Jump
    private bool isWallJumping;
    private float wallJumpingCounter;
    private float wallJumpTime = 0.2f;
    private float wallJumpDurantion = 0.2f;
    
    private void Start()
    {
        Components();
        wallLayer = LayerMask.NameToLayer("Wall");
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    private void Update()
    {
        WallStick();

    }


       




    #region Jump

    private void Components()
    {
        rig = GetComponent<Rigidbody>();
        touchManager = GetComponent<TouchManager>();
    }
    public bool IsOnGround()
    {
        arrColliders = Physics.OverlapSphere(groundCheck.position, 0.2f);
        foreach (Collider c in arrColliders)
        {
            if (c.gameObject.layer == groundLayer)
            {
                return true;

            }

        }
        return false;
    }
    public void Jump()
    {
        if (touchManager.isFacingRight && IsOnGround())
        {
            rig.velocity = new Vector2(sideForce, jumpForce);
            

        }
        else if(IsOnGround() && !touchManager.isFacingRight)
        {
            rig.velocity = new Vector2(-sideForce, jumpForce);
        }

        if(!IsOnGround() && IsWalled())
        {
            WallJump();
        }




    }
   
    #endregion

    #region WallSlide
    private bool IsWalled()
    {
        arrColliders = Physics.OverlapSphere(wallCheck.position, 0.6f);
        foreach (Collider c in arrColliders)
        {
            if (c.gameObject.layer == wallLayer)
            {
                return true;

            }

        }
        return false;

    }

    private void WallStick()
    {
        if (IsWalled() && !IsOnGround() && !isWallJumping)
        {
            isWallSliding = true;
            
            StartCoroutine(WallStickTimer());
            
            
            
            
        }
        else
        {
            isWallSliding = false;
        }
       

       
    }
    IEnumerator WallStickTimer()
    {
        float timer = 0;
        rig.useGravity = false;
        while (timer < wallStickTime)
        {
            
                rig.velocity = new Vector2(0, 0);
                timer += Time.deltaTime;
                yield return null;
            
            
           
        }
        rig.useGravity = true;
        
    }

    private void WallJump()
    {
        StopAllCoroutines();
        rig.useGravity = true;
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
            isWallJumping = true;
            wallJumpingCounter = 0;
            if (touchManager.isFacingRight)
            {
                rig.velocity = new Vector2(sideForce, jumpForce);


            }
            else if (!touchManager.isFacingRight)
            {
                rig.velocity = new Vector2(-sideForce, jumpForce);
            }
            Invoke(nameof(StopWallJump), wallJumpDurantion);
        }
    }
    private void StopWallJump()
    {
        isWallJumping = false;
    }
       
    #endregion
}
