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
            return true;
        }
        else
        {
            print("FailedCoin");
            return false;
        }
    }
}
