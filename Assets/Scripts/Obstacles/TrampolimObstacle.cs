using PlayerEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolimObstacle : MonoBehaviour
{
    [SerializeField] private float upForce;
    [SerializeField] private float sideForce;
    private float _worldWidth;
    private bool isInright;



    private void Start()
    {
        StartCoroutine("CheckSideOfScreen");
    }






    void DefineWorldWidth()
    {
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = Camera.main.orthographicSize * 2;
        _worldWidth = worldHeight * aspect;
    }
    IEnumerator  CheckSideOfScreen()
    {
        DefineWorldWidth();
        float waitTime = 0.2f;
        yield return new WaitForSeconds(waitTime);
        float range = 0.2f;
        isInright =  gameObject.transform.position.x / _worldWidth > range;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player playerChecker = collision.GetComponent<Player>();
        Rigidbody2D rig = collision.GetComponent<Rigidbody2D>();
        
        if (playerChecker != null )
        {
            if (isInright)
            {
                rig.velocity = new Vector2(0,0);
                playerChecker._validCollision = true;
                EventsPlayer.OnJumpLeft();
            }
            else
            {
                rig.velocity = new Vector2(0, 0);
                playerChecker._validCollision = true;
                EventsPlayer.OnJumpRight();
            }

        }
    }
}
