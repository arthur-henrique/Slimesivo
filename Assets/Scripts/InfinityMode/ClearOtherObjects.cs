using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearOtherObjects : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ClearWall"))
        {
            gameObject.SetActive(false);
        }
    }
}
