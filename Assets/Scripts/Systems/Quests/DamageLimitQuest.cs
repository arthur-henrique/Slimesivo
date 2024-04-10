using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageLimitQuest : MonoBehaviour, IQuestSettable
{
    [SerializeField]
    int damageLimit;

    public bool CompletedQuest()
    {
        if(damageLimit >= PlayableLevelManager.Instance.timesHit)
        {
            print("SucceedDamage");           
            print(PlayableLevelManager.Instance.timesHit);
            //QuestingDictionary.Instance.clearedQuestDictionary[dicKey + "_" + keyInt] = true;
            return true;
        }
        else
        {
            print("FailedDamage");
            return false;
        }
    }

    public void SetQuestCompleteToPrefs(string dicKey, int keyInt)
    {
        PlayerPrefs.SetInt(dicKey + "_" + keyInt, 1);
    }
}
