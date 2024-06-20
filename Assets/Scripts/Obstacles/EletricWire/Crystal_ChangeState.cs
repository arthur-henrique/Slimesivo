using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_ChangeState : MonoBehaviour
{
    [SerializeField] private EletricWireObstacle eletric;

    public void OnChangeState()
    {
        if(eletric != null)
        {
            eletric.ChangeStates();
        }
    }
}
