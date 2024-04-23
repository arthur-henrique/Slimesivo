using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageLimitQuest : MonoBehaviour, IQuestSettable
{
    [SerializeField]
    int damageLimit;

    void Start()
    {
        damageLimit = QuestingDictionary.Instance.questDictionary.TryGetValue(SceneManager.GetActiveScene().name + "_hits", out damageLimit) ? damageLimit : 0;
    }
    public bool CompletedQuest(string dicKey, int keyInt)
    {
        if(damageLimit >= PlayableLevelManager.Instance.timesHit)
        {
            print("SucceedDamage");           
            print(PlayableLevelManager.Instance.timesHit);
            QuestManager.Instance.questsCompleted++;
            SetQuestCompleteToPrefs(dicKey, keyInt);
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
