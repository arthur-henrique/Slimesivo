using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestingDictionary : MonoBehaviour
{
    public static QuestingDictionary Instance;

    [SerializeField]
    public Dictionary<string, int> questDictionary = new Dictionary<string, int>();
    [SerializeField]
    public Dictionary<string, bool> clearedQuestDictionary = new Dictionary<string, bool>();

    private void Awake()
    {
        Instance = this;
    }

    
}
