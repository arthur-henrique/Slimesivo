using PlayerEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollision : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            EventsPlayer.OnGround();
        }
       
    }
}
