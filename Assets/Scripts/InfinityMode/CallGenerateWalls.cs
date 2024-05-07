using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallGenerateWalls : MonoBehaviour
{
    [SerializeField] private WallsGenerator wallsGenerator;
    int hitCounter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hitCounter++;
        if(hitCounter <= 1)
        {
            wallsGenerator.GenerateWallTile();
            wallsGenerator.MoveTrigger();
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        hitCounter = 0;
    }
}
