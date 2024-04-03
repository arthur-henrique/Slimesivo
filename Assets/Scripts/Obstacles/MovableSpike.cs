using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovableSpike : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform spikeObject;
    [SerializeField] private float moveSpeed;

    private int currentWaypoint;
  
    

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(waypoints[currentWaypoint].transform.position, spikeObject.transform.position) < 0.5)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
               currentWaypoint = 0;

            }
  
        }
       

        spikeObject.transform.position = Vector3.MoveTowards(spikeObject.transform.position, waypoints[currentWaypoint].transform.position, moveSpeed * Time.deltaTime);
    }

    
   
    private void OnDrawGizmos()
    {
        if(waypoints != null && spikeObject != null)
        {
            
            Gizmos.DrawLine(spikeObject.position, waypoints[0].position);
            for (int i = 0; i < waypoints.Length; i++)
            {
                int nextWaypoint = i + 1 ;
                if (nextWaypoint >= waypoints.Length)
                {
                    nextWaypoint = i;
                }
                Gizmos.DrawLine(waypoints[i].position, waypoints[nextWaypoint].position);
                
              

                
            }
           
                

        }
    }
}
