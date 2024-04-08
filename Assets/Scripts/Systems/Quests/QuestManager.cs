using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        for (int i = 0; i < questGameObjects.Length; i++)
        {
            if (questGameObjects[i].gameObject.GetComponent<IQuestSettable>().CompletedQuest())
            {
                questsCompleted++;
                // Call the function to green up / highlight the UI Elements
            }
            else
            {
                // Call the function to red up / highlight the UI Elements
            }
        }
        // Debug.Log("The player has completed: " + questsCompleted + " quests.");
        // Call the function to show the Stars to the hud depending on the number of stars
        GameManager.instance.pauseCanvas.PressStarsButton(questsCompleted);
    }

    public void AwardThePlayer()
    {
        if(questsCompleted < 1)
        {
            Debug.Log("No Quests Achieved");
            return;
        }
        if(questsCompleted == 1)
        {
            OneStarReward();
        }
        else if(questsCompleted == 2)
        {
            OneStarReward();
            TwoStarReward();
        }
        else if (questsCompleted == 3)
        {
            OneStarReward();
            TwoStarReward();
            ThreeStarReward();
        }
    }

    private void OneStarReward()
    {
        CurrencyManager.instance.UpdateCoinAmount(coinToAward);
        Debug.Log("Received: " + coinToAward);
    }

    private void TwoStarReward()
    {
        // Call the necessary EnergySystem.Replenish function
        Debug.Log("Recovered Energy");
    }

    private void ThreeStarReward()
    {
        // Call the necessary Token Fragment function
        Debug.Log("Received a Token Fragment");
    }
}
