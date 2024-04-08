using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableLevelManager : MonoBehaviour
{
    public static PlayableLevelManager Instance;
    
    // Questing Variables
        // TimeLimitQuest
    public bool canCountTime;
    public float timeSpent;
        // DamageLimitQuest
    public int timesHit;
        // CoinCollectingQuest
    public int coinsCollected;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    public void Start()
    {
        GameManager.instance.SceneLoad();
        //canCountTime = false;
        timesHit = 0;
        coinsCollected = 0;
        timeSpent = 0;
        print("HasZeroed");
    }

    public void FixedUpdate()
    {
        if(canCountTime)
        {
            timeSpent += Time.deltaTime;
        }
    }

    public void CountBegins()
    {
        canCountTime = true;
    }

    public void AddTimeHit()
    {
        timesHit++;
    }

    public void AddCoinCollected()
    {
        coinsCollected++;
    }
}
