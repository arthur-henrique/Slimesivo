using PlayerEvents;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float maxAngle = 45;
    [SerializeField] private bool isRightLocation;
    [SerializeField] private float impulseForce = 10;
    private float _worldWidth;

    private void Start()
    {
        DefineWorldWidth();
        float range = 0.3f;
        if (gameObject.transform.position.x / _worldWidth > range)
        {
            isRightLocation = true;
        }
        else
        {
            isRightLocation = false;
        }
    }

    void DefineWorldWidth()
    {
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = Camera.main.orthographicSize * 2;
        _worldWidth = worldHeight * aspect;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactNormal = collision.contacts[0].normal;
        Rigidbody2D rig = collision.gameObject.GetComponent<Rigidbody2D>();
        //float angle = Vector2.SignedAngle(Vector2.right, contactNormal);
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            if (isRightLocation)
            {
                if (contactNormal == Vector2.right)
                {
                    EventsPlayer.OnWallStick(true);
                }
                else
                {
                    EventsPlayer.OnWallStick(false);
                }

            }
            else
            {
                
                if (contactNormal == Vector2.left)
                {
                    EventsPlayer.OnWallStick(true);
                }
                else
                {
                    EventsPlayer.OnWallStick(false);
                }                      
            }
        }

    }
}