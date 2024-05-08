using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
public class WallsGenerator : MonoBehaviour
{
    [SerializeField] private int wallLength;
    [SerializeField] private GameObject[] triggerWall;
    [SerializeField] private int chanceToGenerateHole;
    
    private int currentTrigger = 0;

    //leftWall Counter
    private int holeCounterLeft;
    private int hasHoleLef;
    //RightWall counter
    private int holeCounterRight;
    private int hasHoleRight;
    //Transforms references
    private Vector3Int leftWallReference;
    private Vector3Int rightWallReference;

    //Tiles 
    [SerializeField] private Tile[] tileSetWalls;
    [SerializeField] private Tilemap rightTilemap;
    [SerializeField] private Tilemap leftTilemap;

    int posToPlaceLeft;
    int posToPlaceRight;
    // Start is called before the first frame update
    void Start()
    {
        GenerateWallTile();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GenerateWallTile();           
        }
    }
    public void GenerateWallTile()
    {
        Debug.Log(RandomizeHoles(hasHoleRight));
        Debug.Log(RandomizeHoles(hasHoleLef));
        int maxHolePerSection = 3;
        //Right Wall
        for (int i = 0; i < wallLength; i++)
        {
            if(RandomizeHoles(hasHoleRight) != 0)
            {
                rightTilemap.SetTile(new Vector3Int(rightWallReference.x, rightWallReference.y + posToPlaceRight), tileSetWalls[Random.Range(0, tileSetWalls.Count())]);
            }
            else if (holeCounterRight < maxHolePerSection)
            {
                holeCounterRight++;
                posToPlaceRight++;
            }

            posToPlaceRight++;

        }
      
        
        
        
        //Left Wall
        for (int i = 0; i < wallLength; i++)
        {
            if (RandomizeHoles(hasHoleLef) != 0)
            {
                leftTilemap.SetTile(new Vector3Int(leftWallReference.x, leftWallReference.y + posToPlaceLeft), tileSetWalls[Random.Range(0, tileSetWalls.Count())]);
               
            }
            else if (holeCounterLeft < maxHolePerSection)
            {
                holeCounterLeft++;
                posToPlaceLeft++;

            }
            posToPlaceLeft++;

        }
       
    }
    float RandomizeHoles(int numberToRandom)
    {
        return numberToRandom = Random.Range(0, chanceToGenerateHole + 1);
        
    }
    public void MoveTrigger()
    {
        triggerWall[currentTrigger].transform.position = new Vector3Int(0, leftWallReference.y + posToPlaceLeft/2);
        if(currentTrigger >= triggerWall.Length)
        {
            currentTrigger++;
        }
        else
        {
            currentTrigger = 0;
        }
    }
   
}
