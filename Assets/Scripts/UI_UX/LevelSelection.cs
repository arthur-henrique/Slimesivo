using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private bool unlocked = false;
    [SerializeField] private TMP_Text [] levelTextName;
    [SerializeField] private GameObject popUpPanel, starSeconds, starLives, starCoins;
    [SerializeField] private Image lockImage, levelCompletedIndicator;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private GameObject[] starsPopUp;
    private string previousLevelName;

    [NamedArray(new string[] { "CoinsText", "HitsText", "LivesText", "SecondsText" })]
    [SerializeField] private TMP_Text[] conditionsTexts;
    //public float conditionSeconds;
    [SerializeField] private int conditionLivesLeft, conditionCoins, conditionSeconds;
    [SerializeField] private bool isItAllCoins;
    // Para automatizar o sistema de instanciamento de quests e tornar a ordem das quests coerentes entre os sistemas
    // colocar no inspector de cada fase os strings que representam o nome de cada quest após um "_"
    // o ideal seria seguir a ordem por enquanto:"_coins", "_hits", "_lives", "_seconds"
    private string[] questsHandles = { "_coins", "_hits", "_lives", "_seconds" };
    // Exemplo padrão: "_coins", "_hits", "_lives"
    // - ainda que algum possa ser removido: "_coins", "_lives", "_seconds"

    [NamedArray(new string[] { "Coins", "Hits", "Lives", "Seconds" })]
    public bool[] questsThatAreActive;

    // Lista de inteiros que representam a ordem das quests ativas
    private List<int> questIndex = new List<int>();
    private List<int> activeQuestValue = new List<int>();


    private List<string> questStrings = new List<string>();
    // Da mesma forma, o ideal seria padronizar os "condition" ints para se tornar os questValues abaixo - seguindo a mesma
    // ordem do modelo de ordem de quests
    [NamedArray(new string[] { "Coins to Get", "Max hits to take", "Lives to keep", "Seconds to beat" })]
    [SerializeField] private int[] questValues;


    [SerializeField] private Sprite starSprite;

    private void Start()
    {
        CheckActiveQuests();
        UpdateLevelStatus();
        UpdateLevelImage();
        popUpPanel.SetActive(false);
        SetConditions();
    }

    private void CheckActiveQuests()
    {
        for (int i = 0; i < questsThatAreActive.Length; i++)
        {
            if (questsThatAreActive[i])
            {
                questIndex.Add(i);
                activeQuestValue.Add(questValues[i]);
                conditionsTexts[i].transform.parent.gameObject.SetActive(true);
            }
        }
    }
    private void SetConditions()
    {

        //if (!QuestingDictionary.Instance.questDictionary.ContainsKey(gameObject.name + "_lives"))
        //    QuestingDictionary.Instance.questDictionary.Add(gameObject.name + "_lives", conditionLivesLeft);
        //if (!QuestingDictionary.Instance.questDictionary.ContainsKey(gameObject.name + "_coins"))
        //    QuestingDictionary.Instance.questDictionary.Add(gameObject.name + "_coins", conditionCoins);
        //if (!QuestingDictionary.Instance.questDictionary.ContainsKey(gameObject.name + "_seconds"))
        //    QuestingDictionary.Instance.questDictionary.Add(gameObject.name + "_seconds", conditionSeconds);
        for (int i = 0; i < questIndex.Count; i++)
        {
            questStrings.Add(questsHandles[questIndex[i]]);
            if (!QuestingDictionary.Instance.questDictionary.ContainsKey(gameObject.name + questStrings[i]))
                QuestingDictionary.Instance.questDictionary.Add(gameObject.name + questStrings[i], activeQuestValue[i]);
        }

        // QuestCompleteToPlayerPrefs
        // PlayerPrefs(String, [0 = false, 1 = true])
        if (!PlayerPrefs.HasKey(gameObject.name + "_completed"))
            PlayerPrefs.SetInt(gameObject.name + "_completed", 0);
        if(!PlayerPrefs.HasKey(gameObject.name + "_maxStars"))
            PlayerPrefs.SetInt(gameObject.name + "_maxStars", 0);
        if (!PlayerPrefs.HasKey(gameObject.name + "_1"))
            PlayerPrefs.SetInt(gameObject.name + "_1", 0);
        if (!PlayerPrefs.HasKey(gameObject.name + "_2"))
            PlayerPrefs.SetInt(gameObject.name + "_2", 0);
        if (!PlayerPrefs.HasKey(gameObject.name + "_3"))
            PlayerPrefs.SetInt(gameObject.name + "_3", 0);


        // Texts Aspects
        // First Quest = Coins
        if (isItAllCoins)
        {
            conditionsTexts[0].text = "Complete the level collecting all " + questValues[0] + " coins";
        }
        else
        {
            conditionsTexts[0].text = "Complete the level collecting " + questValues[0] + " coins";
        }
        // Second Quest = Hits
        // Third Quest = Lives
        if (conditionLivesLeft >= 3)
        {
            conditionsTexts[2].text = "Complete the level with full life";
        }
        else if (conditionLivesLeft == 2)
        {
            conditionsTexts[2].text = "Complete the level with at least 2 lives";
        }
        else if (conditionLivesLeft == 1)
        {
            conditionsTexts[2].text = "Complete the level with at least 1 life";
        }
        // Forth Quest = Seconds
        conditionsTexts[3].text = "Complete the level in " + questValues[3] + " seconds";

        

        // PlayerPrefs.SetInt(gameObject.name + "_coins", conditionCoins);
        // PlayerPrefs.SetFloat(gameObject.name + "_seconds", conditionSeconds);
    }

    /// <summary>
    /// Status of the locked/unlocked level
    /// </summary>
    private void UpdateLevelStatus()
    {
        #region Get the name of the previous level based on the name of this gameObject

        string[] objectNameNumber = gameObject.name.Split('_');
        int previousLevelIndex = int.Parse(objectNameNumber[1]) - 1;
        previousLevelName = objectNameNumber[0] + "_" + previousLevelIndex.ToString("000");
        levelTextName[0].text = objectNameNumber[0] + " " + int.Parse(objectNameNumber[1]);
        levelTextName[1].text = objectNameNumber[0] + " " + int.Parse(objectNameNumber[1]);

        #endregion

        if (PlayerPrefs.GetInt(previousLevelName) > 0 || previousLevelName == "Level_000") //o OR eh para o nivel 1 somente
        {
            unlocked = true;
        }
        else
        {
            unlocked = false;
        }

    }

    /// <summary>
    /// Shows if the level is locked or not
    /// </summary>
    private void UpdateLevelImage()
    {
        if (unlocked == false)
        {
            lockImage.gameObject.SetActive(true);

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(false);
            }
        }
        else
        {
            lockImage.gameObject.SetActive(false);

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < PlayerPrefs.GetInt(gameObject.name); i++)
            {
                stars[i].gameObject.GetComponent<Image>().sprite = starSprite;
                starsPopUp[i].gameObject.GetComponent<Image>().sprite = starSprite;
            }
        }

        if (PlayerPrefs.GetInt(gameObject.name + "_completed") == 1)
        {
            levelCompletedIndicator.GetComponent<Image>().color = new Color(1, 1, 1, 1); //(Red, Green, Blue, Alpha) ou new Color32(255,255,225,100)
        }
        //if (PlayerPrefs.GetInt(gameObject.name + "_Completed") == 1)
    }

    public void ShowOrHideLevelStatistics()
    {
        if(unlocked)
        {
            if (popUpPanel.activeSelf == false)
            {
                popUpPanel.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
                gameObject.GetComponent<RectTransform>().SetAsLastSibling();
                popUpPanel.SetActive(true);
            }
            else
            {
                popUpPanel.SetActive(false);
            }
        }



        //PlayerPrefs.DeleteKey(gameObject.name + "_Completed"); //TODO: ATIVAR ISSO, DEPOIS APAGAR
        print(PlayerPrefs.GetInt(gameObject.name + "_Completed"));
    }

    public void GoToLevel()
    {
        if (unlocked)
        {
            LeanTween.scale(gameObject, gameObject.transform.localScale * 1.2f, 0.5f);
            LeanTween.scale(gameObject, gameObject.transform.localScale, 0.1f).setDelay(0.5f).setOnComplete(GoToLevelTweenFinished);
        }
    }
    private void GoToLevelTweenFinished()
    {
            SceneManager.LoadScene(gameObject.name);
    }
 }

//AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA