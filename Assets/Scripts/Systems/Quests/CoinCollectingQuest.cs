using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinCollectingQuest : MonoBehaviour, IQuestSettable
{
    [SerializeField]
    int coinsToCollect;

    void Start()
    {
        coinsToCollect = QuestingDictionary.Instance.questDictionary.TryGetValue(SceneManager.GetActiveScene().name + "_coins", out coinsToCollect) ? coinsToCollect : 0;
    }

    public bool CompletedQuest()
    {
        if (coinsToCollect <= PlayableLevelManager.Instance.coinsCollected)
        {
            print("SucceedCoin");
            print(PlayableLevelManager.Instance.coinsCollected);
            //QuestingDictionary.Instance.clearedQuestDictionary[dicKey+"_"+keyInt] = true;
            return true;
        }
        else
        {
            print("FailedCoin");
            return false;
        }
    }

    public void SetQuestCompleteToPrefs(string dicKey, int keyInt)
    {
        PlayerPrefs.SetInt(dicKey + "_" + keyInt, 1);
    }
}
