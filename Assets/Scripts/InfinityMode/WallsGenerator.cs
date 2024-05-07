using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
public class WallsGenerator : MonoBehaviour
{
    [SerializeField] private int wallLength;
    [SerializeField] private GameObject[] triggerWall;
    private int currentTrigger = 0;
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
        //Right Wall
        for (int i = 0; i < wallLength; i++)
        {
            posToPlaceRight++;
            rightTilemap.SetTile(new Vector3Int(rightWallReference.x, rightWallReference.y + posToPlaceRight), tileSetWalls[Random.Range(0, tileSetWalls.Count())]);
        }
      
        
        
        
        //Left Wall
        for (int i = 0; i < wallLength; i++)
        {
            posToPlaceLeft++;
            leftTilemap.SetTile(new Vector3Int(leftWallReference.x, leftWallReference.y + posToPlaceLeft), tileSetWalls[Random.Range(0, tileSetWalls.Count())]);
        }
       
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
