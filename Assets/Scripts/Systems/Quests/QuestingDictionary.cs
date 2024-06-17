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

    public Color[] questColors;

    private void Awake()
    {
        Instance = this;
    }

    public void ColorQuest(int textIndex, int colorIndex)
    {
        for (int i = 0; i < winning_TMP_Texts.Length; i++)
        {
            if (i == textIndex)
            {
                winning_TMP_Texts[i].color = questColors[colorIndex];
                losing_TMP_Texts[i].color = questColors[colorIndex];
                pause_TMP_Texts[i].color = questColors[colorIndex];
            }
        }
    }

    public void InitialColorSet()
    {
        for (int i = 0; i < winning_TMP_Texts.Length; i++)
        {
                winning_TMP_Texts[i].color = questColors[0];
                losing_TMP_Texts[i].color = questColors[0];
                pause_TMP_Texts[i].color = questColors[0];
        }
    }
    
}
