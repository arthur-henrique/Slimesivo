using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIncrementalQuest : MonoBehaviour
{
    [SerializeField] private int mapIndex;
    
    public void IncrementalMapQuestAchievement(bool condition)
    {
        if (condition && mapIndex == 1)
        {
            AchievementsManager.instance.IncrementAchievement(GPGSIds.achievement_first_world_master, 1);
        }
    }
}
