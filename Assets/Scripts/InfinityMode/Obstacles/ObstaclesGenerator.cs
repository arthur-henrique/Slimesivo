using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{
    [SerializeField] private GameObject obstacle;
    [SerializeField] private List<GameObject> line1Points = new List<GameObject>();
    [SerializeField] private List<GameObject> line2Points = new List<GameObject>();
    [SerializeField] private List<GameObject> line3Points = new List<GameObject>();
    
    [SerializeField] private int dificultyLevel;

    //Obj Pooling
    [SerializeField]  private int amountToPool;
    private List<GameObject> pooledObjects = new List<GameObject>();


    private int lineToChoose;
    private void Start()
    {
        ObjectPooling();
        GenerateObstacles();
    }
    void ObjectPooling()
    {
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(obstacle);
            pooledObjects.Add(tmp);
            tmp.SetActive(false);

        }
    }

    void GenerateObstacles()
    {
        GameObject obstaclePooled;
        lineToChoose = Random.Range(0, 4);
       int pointToChose = Random.Range(0, 4);
        for (int i = 0; i < dificultyLevel; i++)
        {
            switch (lineToChoose)
            {
                case 0:
                    obstaclePooled = GetPooledObj();
                    obstaclePooled.transform.SetLocalPositionAndRotation(line1Points[pointToChose].transform.position, line1Points[pointToChose].transform.rotation);
                    obstaclePooled.SetActive(true);
                    break;
                case 1:
                    obstaclePooled = GetPooledObj();
                    obstaclePooled.transform.SetLocalPositionAndRotation(line2Points[pointToChose].transform.position, line2Points[pointToChose].transform.rotation);
                    obstaclePooled.SetActive(true);
                    break;
                   
                case 2:
                    obstaclePooled = GetPooledObj();
                    obstaclePooled.transform.SetLocalPositionAndRotation(line3Points[pointToChose].transform.position, line3Points[pointToChose].transform.rotation);
                    obstaclePooled.SetActive(true);
                    break;


            }
        }
    }

    private GameObject GetPooledObj()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
