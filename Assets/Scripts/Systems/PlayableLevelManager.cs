using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableLevelManager : MonoBehaviour
{
    public static PlayableLevelManager Instance;
    public ScoreManager scoreManager;
    
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
        timesHit = 0;
        coinsCollected = 0;
        timeSpent = 0;
        GameManager.instance.SceneLoad();
    }
 

    public void FixedUpdate()
    {
        if(TouchManager.instance != null && TouchManager.instance.hasStarted)
        {
            timeSpent += Time.deltaTime;
            GameManager.instance.timerText.text = timeSpent.ToString("F2");
        }
        else if(TouchManagerTutorial.instance != null && TouchManagerTutorial.instance.hasStarted)
        {
            timeSpent += Time.deltaTime;
            GameManager.instance.timerText.text = timeSpent.ToString("F2");
        }
        
    }

    public void CheckForScores()
    {
        int score = scoreManager.CalculateScore(coinsCollected, timeSpent, GameManager.instance.livesAmount);
        scoreManager.SaveScore(score);
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
