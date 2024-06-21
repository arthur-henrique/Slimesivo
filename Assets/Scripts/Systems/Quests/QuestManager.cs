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
    [SerializeField] int energyToAward;

    [SerializeField] string achivementToUnlock;
    [SerializeField] MapIncrementalQuest mIQ;

    private void Awake()
    {
        if (Instance == null)
        {
            // If not, set instance to this
            Instance = this;
        }
        // If instance already exists and it's not this:
        else if (Instance != this)
        {
            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        levelName = SceneManager.GetActiveScene().name;
        if (QuestingDictionary.Instance.questDictionary.ContainsKey(levelName + "_coins"))
        {
            GameObject coinsQuestPrefab = Instantiate(questGameObjects[0], gameObject.transform);
            activeQuests.Add(coinsQuestPrefab);
            QuestingDictionary.Instance.pause_TMP_Texts[activeQuestsIndex.Count].text = QuestingDictionary.Instance.questTextDictionary[levelName + "_coins"];
            QuestingDictionary.Instance.winning_TMP_Texts[activeQuestsIndex.Count].text = QuestingDictionary.Instance.questTextDictionary[levelName + "_coins"];
            QuestingDictionary.Instance.losing_TMP_Texts[activeQuestsIndex.Count].text = QuestingDictionary.Instance.questTextDictionary[levelName + "_coins"];
            activeQuestsIndex.Add(activeQuestsIndex.Count + 1);
        }
        if (QuestingDictionary.Instance.questDictionary.ContainsKey(levelName + "_hits"))
        {
            GameObject hitsQuestPrefab = Instantiate(questGameObjects[1], gameObject.transform);
            activeQuests.Add(hitsQuestPrefab);
            QuestingDictionary.Instance.pause_TMP_Texts[activeQuestsIndex.Count].text = QuestingDictionary.Instance.questTextDictionary[levelName + "_hits"];
            QuestingDictionary.Instance.winning_TMP_Texts[activeQuestsIndex.Count].text = QuestingDictionary.Instance.questTextDictionary[levelName + "_hits"];
            QuestingDictionary.Instance.losing_TMP_Texts[activeQuestsIndex.Count].text = QuestingDictionary.Instance.questTextDictionary[levelName + "_hits"];
            activeQuestsIndex.Add(activeQuestsIndex.Count + 1);
        }
        if (QuestingDictionary.Instance.questDictionary.ContainsKey(levelName + "_lives"))
        {
            GameObject livesQuestPrefab = Instantiate(questGameObjects[2], gameObject.transform);
            activeQuests.Add(livesQuestPrefab);
            QuestingDictionary.Instance.pause_TMP_Texts[activeQuestsIndex.Count].text = QuestingDictionary.Instance.questTextDictionary[levelName + "_lives"];
            QuestingDictionary.Instance.winning_TMP_Texts[activeQuestsIndex.Count].text = QuestingDictionary.Instance.questTextDictionary[levelName + "_lives"];
            QuestingDictionary.Instance.losing_TMP_Texts[activeQuestsIndex.Count].text = QuestingDictionary.Instance.questTextDictionary[levelName + "_lives"];
            activeQuestsIndex.Add(activeQuestsIndex.Count + 1);
        }
       
        if (QuestingDictionary.Instance.questDictionary.ContainsKey(levelName + "_seconds"))
        {
            GameObject secondsQuestPrefab = Instantiate(questGameObjects[3], gameObject.transform);
            activeQuests.Add(secondsQuestPrefab);
            Debug.LogWarning("Awake");
            QuestingDictionary.Instance.pause_TMP_Texts[activeQuestsIndex.Count].text = QuestingDictionary.Instance.questTextDictionary[levelName + "_seconds"];
            QuestingDictionary.Instance.winning_TMP_Texts[activeQuestsIndex.Count].text = QuestingDictionary.Instance.questTextDictionary[levelName + "_seconds"];
            QuestingDictionary.Instance.losing_TMP_Texts[activeQuestsIndex.Count].text = QuestingDictionary.Instance.questTextDictionary[levelName + "_seconds"];
            activeQuestsIndex.Add(activeQuestsIndex.Count + 1);
        }

        foreach (var kvp in QuestingDictionary.Instance.questTextDictionary)
        {
            Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
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

        //StartCoroutine(ColorQuests());
    }
    public void CheckQuests()
    {
        if(PlayerPrefs.GetInt(levelName + "_completed") == 0)
                PlayerPrefs.SetInt(levelName + "_completed", 1);
        for (int i = 0; i < activeQuests.Count; i++)
        {
            if (activeQuests[i].gameObject.GetComponent<IQuestSettable>().CompletedQuest(levelName, activeQuestsIndex[i]))
            {
                QuestingDictionary.Instance.ColorQuest(i, 1);
                HUDCanvasMenu.instance.UpdateStarSprite(i, 1);
                print(questsCompleted);
                // Call the function to green up / highlight the UI Elements
            }
            else
            {
                // Call the function to red up / highlight the UI Elements
            }
        }

        AwardThePlayer();
        GrantAchievement();

        Debug.Log("The player has completed: " + questsCompleted + " quests.");
        // Call the function to show the Stars to the hud depending on the number of stars
        if (PlayerPrefs.GetInt(levelName + "_maxStars") < questsCompleted)
            PlayerPrefs.SetInt(levelName + "_maxStars", questsCompleted);
        GameManager.instance.pauseCanvas.PressStarsButton
            (PlayerPrefs.GetInt(levelName + "_maxStars"));
        
        //StartCoroutine(GrabTheRewards());
    }

    public void CheckCompletedQuestsForColor()
    {
        for (int i = 0;i < activeQuests.Count;i++)
        {
            if (PlayerPrefs.GetInt(levelName + "_" + (i+1)) == 1)
            {
                QuestingDictionary.Instance.ColorQuest(i, 1);
            }
        }
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

    private void GrantAchievement()
    {
        if(PlayerPrefs.GetInt(levelName + "_1rewarded") == 1 &&
            PlayerPrefs.GetInt(levelName + "_2rewarded") == 1 &&
            PlayerPrefs.GetInt(levelName + "_3rewarded") == 1 &&
            PlayerPrefs.GetInt(levelName + "_achieved") == 0)
        {
            PlayerPrefs.SetInt(levelName + "_achieved", 1);
            mIQ.IncrementalMapQuestAchievement(true);
            AchievementsManager.instance.UnlockAchievement(achivementToUnlock);
        }
    }

    IEnumerator ColorQuests()
    {
        yield return new WaitForSeconds(0.25f);
        CheckCompletedQuestsForColor();
    }
}


