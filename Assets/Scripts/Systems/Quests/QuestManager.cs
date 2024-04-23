using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;


    [SerializeField] GameObject[] questGameObjects;
    [SerializeField] List<GameObject> activeQuests = new List<GameObject>();
    [SerializeField] List<int> activeQuestsIndex = new List<int>();
    string levelName;
    public int questsCompleted = 0;
    // Rewards
    [SerializeField] int coinToAward;

    private void Awake()
    {
        Instance = this;
        levelName = SceneManager.GetActiveScene().name;
        if (QuestingDictionary.Instance.questDictionary.ContainsKey(levelName + "_coins"))
        {
            GameObject coinsQuestPrefab = Instantiate(questGameObjects[0], gameObject.transform);
            activeQuests.Add(coinsQuestPrefab);
            activeQuestsIndex.Add(activeQuestsIndex.Count + 1);
        }
        if (QuestingDictionary.Instance.questDictionary.ContainsKey(levelName + "_hits"))
        {
            GameObject hitsQuestPrefab = Instantiate(questGameObjects[1], gameObject.transform);
            activeQuests.Add(hitsQuestPrefab);
            activeQuestsIndex.Add(activeQuestsIndex.Count + 1);
        }
        if (QuestingDictionary.Instance.questDictionary.ContainsKey(levelName + "_lives"))
        {
            GameObject livesQuestPrefab = Instantiate(questGameObjects[2], gameObject.transform);
            activeQuests.Add(livesQuestPrefab);
            activeQuestsIndex.Add(activeQuestsIndex.Count + 1);
        }
       
        if (QuestingDictionary.Instance.questDictionary.ContainsKey(levelName + "_seconds"))
        {
            GameObject secondsQuestPrefab = Instantiate(questGameObjects[3], gameObject.transform);
            activeQuests.Add(secondsQuestPrefab);
            activeQuestsIndex.Add(activeQuestsIndex.Count + 1);
        }
        
    }
    private void Start()
    {
        questsCompleted = 0;
        for (int i = 1; i < activeQuests.Count+1; i++)
        {
            if(!PlayerPrefs.HasKey(levelName + "_" + i + "rewarded"))
                PlayerPrefs.SetInt(levelName + "_"+ i + "rewarded", 0);

        }
    }
    public void CheckQuests()
    {
        if(PlayerPrefs.GetInt(levelName + "_completed") == 0)
                PlayerPrefs.SetInt(levelName + "_completed", 1);
        for (int i = 0; i < activeQuests.Count; i++)
        {
            if (activeQuests[i].gameObject.GetComponent<IQuestSettable>().CompletedQuest(levelName, activeQuestsIndex[i]))
            {

                print(questsCompleted);
                // Call the function to green up / highlight the UI Elements
            }
            else
            {
                // Call the function to red up / highlight the UI Elements
            }
        }

        AwardThePlayer();
        Debug.Log("The player has completed: " + questsCompleted + " quests.");
        // Call the function to show the Stars to the hud depending on the number of stars
        if (PlayerPrefs.GetInt(levelName + "_maxStars") < questsCompleted)
            PlayerPrefs.SetInt(levelName + "_maxStars", questsCompleted);
        GameManager.instance.pauseCanvas.PressStarsButton
            (PlayerPrefs.GetInt(levelName + "_maxStars"));
        
        //StartCoroutine(GrabTheRewards());
    }

    public void AwardThePlayer()
    {
        print("Checking Results");
        if (questsCompleted > PlayerPrefs.GetInt(levelName + "maxStars"))
        {
            switch (questsCompleted)
            {
                case 1:
                    OneStarReward();
                    break;
                case 2:
                    OneStarReward();
                    TwoStarReward();
                    break;
                case 3:
                    OneStarReward();
                    TwoStarReward();
                    ThreeStarReward();
                    break;
            }

        }
    }

    private void OneStarReward()
    {
        if(PlayerPrefs.GetInt(levelName+ "_1rewarded") == 0)
        {
            PlayerPrefs.SetInt(levelName + "_1rewarded", 1);
            CurrencyManager.instance.UpdateCoinAmount(coinToAward);
            //activeQuests[0].gameObject.GetComponent<IQuestSettable>().SetQuestCompleteToPrefs(levelName, 1);
            Debug.Log("Received: " + coinToAward);
        }
        
    }

    private void TwoStarReward()
    {
        if (PlayerPrefs.GetInt(levelName + "_2rewarded") == 0)
        {
            PlayerPrefs.SetInt(levelName + "_2rewarded", 1);
            EnergyManager.Instance.AddEnergy(1);
            //activeQuests[1].gameObject.GetComponent<IQuestSettable>().SetQuestCompleteToPrefs(levelName, 2);

            // Call the necessary EnergySystem.Replenish function
            Debug.Log("Recovered Energy");
        }
        
    }

    private void ThreeStarReward()
    {
        if (PlayerPrefs.GetInt(levelName + "_3rewarded") == 0)
        {
            PlayerPrefs.SetInt(levelName + "_3rewarded", 1);
            EnergyManager.Instance.AddEnergy(1);
            //activeQuests[2].gameObject.GetComponent<IQuestSettable>().SetQuestCompleteToPrefs(levelName, 3);

            // Call the necessary Token Fragment function
            Debug.Log("Received a Token Fragment");
        }
        
    }

    //IEnumerator GrabTheRewards()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    AwardThePlayer();
    //}
}


