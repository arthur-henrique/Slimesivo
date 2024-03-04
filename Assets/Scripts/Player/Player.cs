using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float sideForce;
    

    private Rigidbody rig;

    private void Start()
    {
        Components(); 
    }



    #region Jump

    private void Components()
    {
        rig = GetComponent<Rigidbody>();
    }
    public void JumpRight()
    {
        rig.AddForce(Vector2.up * jumpForce + Vector2.right * sideForce, ForceMode.Impulse);
    }
    public void JumpLeft()
    {
        rig.AddForce(Vector2.up * jumpForce + Vector2.left * sideForce, ForceMode.Impulse);
    }
    #endregion
}
