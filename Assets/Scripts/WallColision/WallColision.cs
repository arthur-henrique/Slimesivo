using PlayerEvents;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float maxAngle = 45;
    [SerializeField] bool isRightLocation;
    private float _worldWidth;

    private void Start()
    {
        DefineWorldWidth();
        float range = 0.5f;
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
        float angle = Vector2.SignedAngle(Vector2.right, contactNormal);
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            if (isRightLocation)
            {
                if (angle > -maxAngle && angle < maxAngle)
                {
                    EventsPlayer.OnWallStick(true);
                }

            }
            else
            {
                if (angle < -maxAngle || angle > maxAngle)
                {
                    EventsPlayer.OnWallStick(true);
                }                        
            }
        }

    }
}