using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectingQuest : MonoBehaviour, IQuestSettable
{
    [SerializeField]
    int coinsToCollect;

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
