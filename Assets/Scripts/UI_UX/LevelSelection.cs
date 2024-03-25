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
    private int previousLevelIndex;

    private void Update()
    {
        UpdateLevelImage();
        UpdateLevelStatus(); //TODO: Move this later
    }

    /*int GetSceneBuildIndex(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (sceneNameInBuild == sceneName)
            {
                return i;
            }
        }

        return -1; // Scene not found
    }*/

    /// <summary>
    /// Status of the locked/unlocked level
    /// </summary>
    private void UpdateLevelStatus()
    {
        #region Get the name and index of the previous level based on the name of this gameObject

        string[] objectNameNumber = gameObject.name.Split('_');
        int previousLevelIndex = int.Parse(objectNameNumber[1]) - 1;
        previousLevelName = objectNameNumber[0] + "_" + previousLevelIndex.ToString("000");

        //previousLevelIndex = GetSceneBuildIndex(previousLevelName);

        #endregion

        //if (PlayerPrefs.GetInt(previousLevelName) > 0)
        //if (PlayerPrefs.GetInt(SceneManager.previousLevelName.buildIndex) > 0)
        //if (PlayerPrefs.GetInt(GetBuildIndexByScenePath(string scenePath)) > 0)
        //{
        //    unlocked = true;
        //}
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
        }
    }

    public void GoToLevel(int levelIndex)
    {
        if (unlocked)
        {
            SceneManager.LoadScene("UI_Test_Level " + levelIndex); //TODO: MUDAR ESTE NOME DEPOIS
        }
    }
}
