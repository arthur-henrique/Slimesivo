using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestingDictionary : MonoBehaviour
{
    public static QuestingDictionary Instance;

    [SerializeField]
    public Dictionary<string, int> questDictionary = new Dictionary<string, int>();
    [SerializeField]
    public Dictionary<string, string> questTextDictionary = new Dictionary<string, string>();
    [SerializeField]
    public Dictionary<string, bool> clearedQuestDictionary = new Dictionary<string, bool>();
    public TMP_Text[] winning_TMP_Texts;
    public TMP_Text[] losing_TMP_Texts;
    public TMP_Text[] pause_TMP_Texts;
    private void Awake()
    {
        Instance = this;
    }

    
}
