using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitQuest : MonoBehaviour, IQuestSettable
{
    [SerializeField]
    float timeLimit;

    public bool CompletedQuest()
    {
        if(timeLimit >= PlayableLevelManager.Instance.timeSpent)
        {
            print("SucceedTime");
            print(PlayableLevelManager.Instance.timeSpent);
            return true;
        }
        else
        {
            print("FailedTime");           
            return false;
        }
    }
}
