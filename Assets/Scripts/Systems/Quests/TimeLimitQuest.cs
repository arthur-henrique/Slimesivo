using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeLimitQuest : MonoBehaviour, IQuestSettable
{
    [SerializeField]
    int timeLimit;

    void Start()
    {
        timeLimit = QuestingDictionary.Instance.questDictionary.TryGetValue(SceneManager.GetActiveScene().name + "_seconds", out timeLimit) ? timeLimit : 0;
    }

    public bool CompletedQuest()
    {
        if(timeLimit >= PlayableLevelManager.Instance.timeSpent)
        {
            print("SucceedTime");
            print(PlayableLevelManager.Instance.timeSpent);
            //QuestingDictionary.Instance.clearedQuestDictionary[dicKey + "_" + keyInt] = true;
            return true;
        }
        else
        {
            print("FailedTime");           
            return false;
        }

    }

    public void SetQuestCompleteToPrefs(string dicKey, int keyInt)
    {
        PlayerPrefs.SetInt(dicKey + "_" + keyInt, 1);
    }
}
