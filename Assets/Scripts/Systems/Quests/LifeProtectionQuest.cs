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

    public bool CompletedQuest()
    {
        if (livesToHave <= GameManager.instance.livesAmount)
        {
            print("SucceedLives");
            print(GameManager.instance.livesAmount);
            return true;
        }
        else
        {
            print("FailedLives");
            return false;
        }
    }
}
