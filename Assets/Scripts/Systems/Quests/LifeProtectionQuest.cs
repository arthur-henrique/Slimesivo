using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeProtectionQuest : MonoBehaviour, IQuestSettable
{
    [SerializeField]
    int livesToHave;

    void Start()
    {
        livesToHave = QuestingDictionary.Instance.questDictionary.TryGetValue(SceneManager.GetActiveScene().name + "_lives", out livesToHave) ? livesToHave : 0;
        print(livesToHave);
    }

    public bool CompletedQuest(string dicKey, int keyInt)
    {
        if (livesToHave <= GameManager.instance.livesAmount)
        {
            print("SucceedLives");
            print(GameManager.instance.livesAmount);
            SetQuestCompleteToPrefs(dicKey, keyInt);
            //QuestingDictionary.Instance.clearedQuestDictionary[dicKey + "_" + keyInt] = true;
            return true;
        }
        else
        {
            print("FailedLives");
            return false;
        }
    }

    public void SetQuestCompleteToPrefs(string dicKey, int keyInt)
    {
        PlayerPrefs.SetInt(dicKey + "_" + keyInt, 1);
    }
}
