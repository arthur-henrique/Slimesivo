using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private bool unlocked = false;
    public Image lockImage;
    public GameObject[] stars;
    private string previousLevelName;

    public Sprite starSprite;

    private void Update()
    {
        UpdateLevelImage(); //TODO: Move this later
        UpdateLevelStatus(); //TODO: Move this later
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

        #endregion

        if (PlayerPrefs.GetInt(previousLevelName) > 0)
        {
            unlocked = true;
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
            }
        }
    }

    public void GoToLevel()
    {
        if (unlocked)
        {
            SceneManager.LoadScene(gameObject.name);
        }
    }
}
