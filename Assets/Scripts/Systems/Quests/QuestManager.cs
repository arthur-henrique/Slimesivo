using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;


    [SerializeField] GameObject[] questGameObjects;
    public int questsCompleted = 0;
    // Rewards
    [SerializeField] int coinToAward;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        questsCompleted = 0;
    }
    public void CheckQuests()
    {
        if(PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_completed") == 0)
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_completed", 1);
        for (int i = 0; i < questGameObjects.Length; i++)
        {
            if (questGameObjects[i].gameObject.GetComponent<IQuestSettable>().CompletedQuest())
            {
                AwardThePlayer(i);
                questsCompleted++;
                // Call the function to green up / highlight the UI Elements
            }
            else
            {
                // Call the function to red up / highlight the UI Elements
            }
        }
        Debug.Log("The player has completed: " + questsCompleted + " quests.");
        // Call the function to show the Stars to the hud depending on the number of stars
        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_maxStars") < questsCompleted)
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_maxStars", questsCompleted);
        GameManager.instance.pauseCanvas.PressStarsButton
            (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_maxStars"));
        
        //StartCoroutine(GrabTheRewards());
    }

    public void AwardThePlayer(int questCompleted)
    {
        print("Checking Results");
        //if(questCompleted < 1)
        //{
        //    Debug.Log("No Quests Achieved");
        //    return;
        //}
        if(questCompleted == 0 && PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_1") == 0)
        {
            OneStarReward();
        }
        else if(questCompleted == 1 && PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_2") == 0)
        {
            TwoStarReward();
        }
        else if (questCompleted == 2 && PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_3") == 0)
        {
            ThreeStarReward();
        }
    }

    private void OneStarReward()
    {
        CurrencyManager.instance.UpdateCoinAmount(coinToAward);
        questGameObjects[0].gameObject.GetComponent<IQuestSettable>().
            SetQuestCompleteToPrefs(SceneManager.GetActiveScene().name, 1);
        Debug.Log("Received: " + coinToAward);
    }

    private void TwoStarReward()
    {
        questGameObjects[1].gameObject.GetComponent<IQuestSettable>().
            SetQuestCompleteToPrefs(SceneManager.GetActiveScene().name, 2);

        // Call the necessary EnergySystem.Replenish function
        Debug.Log("Recovered Energy");
    }

    private void ThreeStarReward()
    {
        questGameObjects[2].gameObject.GetComponent<IQuestSettable>().
            SetQuestCompleteToPrefs(SceneManager.GetActiveScene().name, 3);

        // Call the necessary Token Fragment function
        Debug.Log("Received a Token Fragment");
    }

    //IEnumerator GrabTheRewards()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    AwardThePlayer();
    //}
}


