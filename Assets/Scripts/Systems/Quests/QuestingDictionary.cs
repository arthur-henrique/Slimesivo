using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestingDictionary : MonoBehaviour
{
    public static QuestingDictionary Instance;

    [SerializeField]
    public Dictionary<string, int> questDictionary = new Dictionary<string, int>();

    private void Awake()
    {
        Instance = this;
    }

    
}
