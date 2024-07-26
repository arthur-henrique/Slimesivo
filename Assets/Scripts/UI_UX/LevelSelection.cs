using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Security;
using UnityEngine.SocialPlatforms.Impl;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private bool unlocked = false;
    [SerializeField] private bool isTutorial = false;
    [SerializeField] private TMP_Text [] levelTextName;
    [SerializeField] private GameObject popUpPanel;
    [NamedArray(new string[] { "Star Coins", "Star Hits", "Star Lives", "Star Seconds" })]
    [SerializeField] GameObject[] starEmptySprites;
    [SerializeField] private Image lockImage, levelCompletedIndicator;
    [NamedArray(new string[] { "Left Star", "Middle Star", "Right Star" })]
    [SerializeField] private GameObject[] starsShownInMap;
    [SerializeField] private GameObject[] starsPopUp;
    private string previousLevelName;

    [NamedArray(new string[] { "CoinsText", "HitsText", "LivesText", "SecondsText"})]
    [SerializeField] private TMP_Text[] conditionsTexts;
    //public float conditionSeconds;
    //[SerializeField] private int conditionLivesLeft, conditionCoins, conditionSeconds;
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


    [SerializeField] private Sprite starFullSprite;
    private GameObject[] objectsWithTag;

    // 1st Quest Localization
    public LocalizedString allCoinsText;
    public LocalizedString someCoinsText;

    // 2nd Quest Localization
    public LocalizedString noDamageText;
    public LocalizedString oneDamageText;
    public LocalizedString someDamageText;

    // 3rd Quest Localization
    public LocalizedString threeLivesText;
    public LocalizedString twoLivesText;
    public LocalizedString oneLifeText;

    // 4th Quest Localization
    public LocalizedString timeTotalText;

    public TMP_Text highScoreText;
    public LocalizedString scoreText;

    private void Start()
    {
        objectsWithTag = GameObject.FindGameObjectsWithTag("UI_Hide_Element_For_Pop_Up"); //TODO: pensar como fazer isso de uma maneira mais otimizada, pois isto esta sendo chamado para todas as instancias

        if(!isTutorial)
        {
            CheckActiveQuests();
            UpdateLevelStatus();
            UpdateLevelImage();
            SetConditions();
        }
        
        popUpPanel.SetActive(false);
        
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
            print(gameObject.name + questStrings[i]);
            
        }

        print(QuestingDictionary.Instance.questDictionary.Values);
        // QuestCompleteToPlayerPrefs
        // PlayerPrefs(String, [0 = false, 1 = true])
        if (!PlayerPrefs.HasKey(gameObject.name + "_completed"))
            PlayerPrefs.SetInt(gameObject.name + "_completed", 0);
        if(!PlayerPrefs.HasKey(gameObject.name + "_maxStars"))
            PlayerPrefs.SetInt(gameObject.name + "_maxStars", 0);
        if (!PlayerPrefs.HasKey(gameObject.name + "_score"))
            PlayerPrefs.SetInt(gameObject.name + "_score", 0);
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
            // Fetch and format the localized "all_coins" string
            allCoinsText.Arguments = new object[] { questValues[0] };
            allCoinsText.StringChanged += (localizedString) =>
            {
                conditionsTexts[0].text = localizedString;
            };
        }
        else
        {
            // Fetch and format the localized "some_coins" string
            someCoinsText.Arguments = new object[] { questValues[0] };
            someCoinsText.StringChanged += (localizedString) =>
            {
                conditionsTexts[0].text = localizedString;
            };
        }
        //if (isItAllCoins)
        //{
        //    conditionsTexts[0].text = "Complete the level collecting all " + questValues[0] + " coins"; //"all"
        //}
        //else
        //{
        //    conditionsTexts[0].text = "Complete the level collecting " + questValues[0] + " coins";
        //}

        // Second Quest = Hits
        if (questValues[1] == 0)
        {
            conditionsTexts[1].text = noDamageText.GetLocalizedString();
        }
        else if (questValues[1] == 1)
        {
            // Fetch and format the localized "some_coins" string
            conditionsTexts[1].text = oneDamageText.GetLocalizedString();
        }
        else
        {
            someDamageText.Arguments = new object[] { questValues[1] };
            someDamageText.StringChanged += (localizedString) =>
            {
                conditionsTexts[1].text = localizedString;
            };
        }

        //if (questValues[1] == 0)
        //{
        //    conditionsTexts[1].text = "Complete the level without taking damage"; //"without"
        //}
        //else if (questValues[1] == 1)
        //{
        //    conditionsTexts[1].text = "Complete the level without taking more than 1 hit"; //"singular"
        //}
        //else
        //{
        //    conditionsTexts[1].text = "Complete the level without taking more than " + questValues[1] + "hits"; //"plural"
        //}

        // Third Quest = Lives
        if (questValues[2] >= 3)
        {
            conditionsTexts[2].text = threeLivesText.GetLocalizedString(); //"full"
        }
        else if (questValues[2] == 2)
        {
            conditionsTexts[2].text = twoLivesText.GetLocalizedString(); //"plural"
        }
        else if (questValues[2] == 1)
        {
            conditionsTexts[2].text = oneLifeText.GetLocalizedString(); //"singular"
        }

        // Forth Quest = Seconds
        timeTotalText.Arguments = new object[] { questValues[3] };
        timeTotalText.StringChanged += (localizedString) =>
        {
            conditionsTexts[3].text = localizedString;
        };

        for (int i = 0; i < questsHandles.Length; i++)
        {
            if (!QuestingDictionary.Instance.questTextDictionary.ContainsKey(gameObject.name + questsHandles[i]))
            {
                QuestingDictionary.Instance.questTextDictionary.Add(gameObject.name + questsHandles[i], conditionsTexts[i].text);
                print(conditionsTexts[i].text);
            }
        }

        //highScoreText.text = PlayerPrefs.GetInt(gameObject.name + "_score").ToString();
        scoreText.Arguments = new object[] { PlayerPrefs.GetInt(gameObject.name + "_score") };
        scoreText.StringChanged += (localizedString) =>
        {
            highScoreText.text = localizedString;
        };


        // PlayerPrefs.SetInt(gameObject.name + "_coins", conditionCoins);
        // PlayerPrefs.SetFloat(gameObject.name + "_seconds", conditionSeconds);
    }

    /// <summary>
    /// Status of the locked/unlocked level
    /// </summary>
    private void UpdateLevelStatus()
    {
        #region Get the name of the previous level based on the name of this gameObject

        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            string[] objectNameNumber = gameObject.name.Split('_');
            int previousLevelIndex = int.Parse(objectNameNumber[1]) - 1;
            previousLevelName = objectNameNumber[0] + "_" + previousLevelIndex.ToString("000");
            //levelTextName[0].text = objectNameNumber[0] + " " + int.Parse(objectNameNumber[1]);
            levelTextName[0].text = objectNameNumber[1].TrimStart('0'); //numero do nivel que aparece no mapa
            levelTextName[1].text = objectNameNumber[0] + " " + int.Parse(objectNameNumber[1]); //nome e numero do nivel que aparecem no pop up

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
        

    }

    /// <summary>
    /// Shows if the level is locked or not
    /// </summary>
    private void UpdateLevelImage()
    {
        if (unlocked == false)
        {
            lockImage.gameObject.SetActive(true);

            for (int i = 0; i < starsShownInMap.Length; i++)
            {
                starsShownInMap[i].gameObject.SetActive(false);
            }
        }
        else
        {
            lockImage.gameObject.SetActive(false);

            for (int i = 0; i < starsShownInMap.Length; i++)
            {
                starsShownInMap[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < PlayerPrefs.GetInt(gameObject.name); i++)
            {
                starsShownInMap[i].gameObject.GetComponent<Image>().sprite = starFullSprite;
                starsPopUp[i].gameObject.GetComponent<Image>().sprite = starFullSprite;
            }
        }

        #region Para o Pop Up

        if (PlayerPrefs.GetInt(gameObject.name + "_completed") == 1)
        {
            levelCompletedIndicator.GetComponent<Image>().color = new Color(1, 1, 1, 1); //(Red, Green, Blue, Alpha) ou new Color32(255,255,255,100)
        }
        //if (PlayerPrefs.GetInt(gameObject.name + "_Completed") == 1)

        for (int i = 0; i < questIndex.Count; i++)
        {
            int extra = i + 1;
            if (PlayerPrefs.GetInt(gameObject.name + "_" + extra) > 0)
            {
                print(questIndex[i]);
                starEmptySprites[questIndex[i]].gameObject.GetComponent<Image>().sprite = starFullSprite;
            }

        }

        #endregion
    }


    public void ShowOrHideLevelStatistics()
    {
        VibrationManager.instance.VibeUI();
        if (unlocked)
        {
            if (popUpPanel.activeSelf == false) //Show
            {
                popUpPanel.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
                gameObject.GetComponent<RectTransform>().SetAsLastSibling();
                popUpPanel.SetActive(true);
                foreach (GameObject obj in objectsWithTag)
                {
                    obj.SetActive(false);
                }
            }
            else //Hide
            {
                popUpPanel.SetActive(false);
                foreach (GameObject obj in objectsWithTag)
                {
                    obj.SetActive(true);
                }
            }
        }



        //PlayerPrefs.DeleteKey(gameObject.name + "_Completed"); //TODO: ATIVAR ISSO, DEPOIS APAGAR
        print(PlayerPrefs.GetInt(gameObject.name + "_Completed"));
    }

    public void GoToLevel()
    {
        VibrationManager.instance.VibeUI();
        if (unlocked)
        {
            if(EnergyManager.Instance.UseEnergy(0))
            {
                //LeanTween.scale(gameObject, gameObject.transform.localScale * 1.2f, 0.5f);
                //LeanTween.scale(gameObject, gameObject.transform.localScale, 0.1f).setDelay(0.5f).setOnComplete(GoToLevelTweenFinished);
                LeanTween.scale(popUpPanel, popUpPanel.transform.localScale * 1.2f, 0.5f);
                LeanTween.scale(popUpPanel, popUpPanel.transform.localScale, 0.5f).setDelay(0.2f).setOnComplete(GoToLevelTweenFinished);
            }
            else
            {
                Debug.LogWarning("No Energy in Sight");
            }
            
        }
    }
    private void GoToLevelTweenFinished()
    {
            SceneManager.LoadScene(gameObject.name);
    }
 }

//AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA