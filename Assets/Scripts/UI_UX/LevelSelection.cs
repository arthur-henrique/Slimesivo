using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private bool unlocked = false;
    [SerializeField] private TMP_Text [] levelTextName;
    [SerializeField] private GameObject popUpPanel;
    public Image lockImage;
    public GameObject[] stars;
    public GameObject[] starsPopUp;
    private string previousLevelName;

    [SerializeField] private TMP_Text conditionSecondsText, conditionLivesLeftText, conditionCoinsText;
    public float conditionSeconds;
    public int conditionLivesLeft, conditionCoins;
    [SerializeField] private bool isItAllCoins;


    public Sprite starSprite;

    private void Start()
    {
        UpdateLevelStatus();
        UpdateLevelImage();
        popUpPanel.SetActive(false);
        SetConditions();
    }

    private void SetConditions()
    {
        if (conditionLivesLeft >= 3)
        {
            conditionLivesLeftText.text = "Complete the level with full life";
        }
        else if (conditionLivesLeft == 2)
        {
            conditionLivesLeftText.text = "Complete the level with at least 2 lives";
        }
        else if (conditionLivesLeft == 1)
        {
            conditionLivesLeftText.text = "Complete the level with at least 1 life";
        }

        if(isItAllCoins)
        {
            conditionCoinsText.text = "Complete the level collecting all " + conditionCoins + " coins";
        }
        else
        {
            conditionCoinsText.text = "Complete the level collecting " + conditionCoins + " coins";
        }

        conditionSecondsText.text = "Complete the level in " + conditionSeconds + " seconds";

       // PlayerPrefs.SetInt(gameObject.name + "_livesLeft", conditionLivesLeft);
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