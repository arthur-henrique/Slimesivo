using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Jump variables
    [SerializeField] private float jumpForce;
    [SerializeField] private float sideForce;
    private bool canJump = true;
    private bool wallJump;

    //components
    private Rigidbody rig;

    //Wall Slide
    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 5f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallStickTime;
    private bool isTouchingWall;
    private int wallLayer;
    private Collider[] arrColliders;
    private void Start()
    {
        Components();
        wallLayer = LayerMask.NameToLayer("Wall");
    }

    private void FixedUpdate()
    {
        WallStick();

    }



    #region Jump

    private void Components()
    {
        rig = GetComponent<Rigidbody>();
    }
    public void JumpRight()
    {
        if (canJump)
        {
            if (isTouchingWall)
            {
                StopAllCoroutines();
                rig.useGravity = true;
                rig.velocity = new Vector2(sideForce, jumpForce);
                canJump = false;
            }
            //rig.AddForce(Vector2.up * jumpForce + Vector2.right * sideForce, ForceMode.Impulse);
            rig.velocity = new Vector2(sideForce, jumpForce);
            canJump = false;

        }

    }
    public void JumpLeft()
    {
        if (canJump)
        {
            if (isTouchingWall)
            {
                StopAllCoroutines();
                rig.useGravity = true;
                rig.velocity = new Vector2(-sideForce, jumpForce);
                canJump = false;
                wallJump = true;
            }
            
            rig.velocity = new Vector2(-sideForce, jumpForce);
            canJump = false;
        }
    }
    #endregion

    #region WallSlide
    private bool IsWalled()
    {
        arrColliders = Physics.OverlapSphere(wallCheck.position, 0.2f);
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
        if (IsWalled() && !canJump)
        {
            isWallSliding = true;
            StartCoroutine(WallStickTimer());
            canJump = true;
            isTouchingWall = true;
            wallJump = true;
            
            
        }
       

        if (isWallSliding)
        {
            rig.velocity = new Vector2(rig.velocity.x, Mathf.Clamp(rig.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
    }
    IEnumerator WallStickTimer()
    {
        float timer = 0;
        rig.useGravity = false;
        while (timer < wallStickTime)
        {
            if (!wallJump)
            {
                rig.velocity = new Vector2(0, 0);
                timer += Time.deltaTime;
                yield return null;
            }
            
           
        }
        rig.useGravity = true;
        isWallSliding = true;
    }


       
    #endregion
}
