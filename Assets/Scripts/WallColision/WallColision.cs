using PlayerEvents;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float maxAngle = 45;
    [SerializeField] private bool isRightLocation;
    private float impulseForce = 3f;
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
        Vector3 contactNormal = collision.contacts[0].normal;
        GameObject player = collision.gameObject;
        Rigidbody2D rigPlayer = player.gameObject.GetComponent<Rigidbody2D>();
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            
                       
            if (isRightLocation)
            {
                if (contactNormal == Vector3.right)
                {
                    EventsPlayer.OnWallStick(true);
                }
                else
                {
                    Debug.LogWarning(contactNormal);
                    rigPlayer.AddForce(new Vector2(player.transform.position.x, 0) * impulseForce, ForceMode2D.Impulse);
                    EventsPlayer.OnWallStick(false);
                }              
            }
            else
            {

                if (contactNormal == Vector3.left)
                {
                    EventsPlayer.OnWallStick(true);
                }
                else
                {
                    Debug.LogWarning(contactNormal);
                    rigPlayer.AddForce(new Vector2(player.transform.position.x, 0) * impulseForce, ForceMode2D.Impulse);
                    EventsPlayer.OnWallStick(false);
                }
            }
            
        }

    }
}