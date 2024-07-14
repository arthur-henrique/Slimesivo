using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections;
using Newtonsoft.Json;

public static class LocalCacheManager
{
    public static int GetLocalData(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static void SetLocalData(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }
}

public class GameData
{
    public Dictionary<string, int> levels { get; set; }
}

public class CloudSaveManager : MonoBehaviour
{
    public static CloudSaveManager Instance { get; private set; }
    public int numberOfLevels;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        EnsureSignedInAsync();
    }

    public bool EnsureSignedInAsync()
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            return true;
        }
        else
        {
            return false;
        }

        //try
        //{
        //    await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //    return true;
        //}
        //catch (Exception e)
        //{
        //    Debug.LogError("Sign-in failed: " + e.Message);
        //    return false;
        //}
    }

    public async Task SaveGameData(string key, int value)
    {
        if (!EnsureSignedInAsync())
        {
            Debug.LogError("Cannot save data: Player is not signed in.");
            return;
        }

        try
        {
            var data = new Dictionary<string, object> { { key, value } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log("Data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving data: " + e.Message);
        }
    }

    public async Task<int> GetGameData(string key)
    {
        if (!EnsureSignedInAsync())
        {
            Debug.LogError("Cannot load data: Player is not signed in.");
            return 0;
        }

        try
        {
            var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });

            if (data.ContainsKey(key))
            {
                var item = data[key];
                return item.Value.GetAs<int>();
            }
            else
            {
                // Key does not exist, initialize value
                await SaveGameData(key, 0);
                return 0;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading data: " + e.Message);
            return 0;
        }
    }




    //public async Task SyncLocalDataToCloud()
    //{
    //    if (!EnsureSignedInAsync())
    //    {
    //        Debug.LogError("Cannot sync data: Player is not signed in.");
    //        return;
    //    }

    //    try
    //    {
    //        // Generate keys to check
    //        List<string> keysToCheck = new List<string>();
    //        for (int i = 1; i <= numberOfLevels; i++)
    //        {
    //            string levelPrefix = i < 10 ? $"Level_00{i}" : $"Level_0{i}";
    //            keysToCheck.Add($"{levelPrefix}");
    //            keysToCheck.Add($"{levelPrefix}_1");
    //            keysToCheck.Add($"{levelPrefix}_2");
    //            keysToCheck.Add($"{levelPrefix}_3");
    //            keysToCheck.Add($"{levelPrefix}_1_rewarded");
    //            keysToCheck.Add($"{levelPrefix}_2_rewarded");
    //            keysToCheck.Add($"{levelPrefix}_3_rewarded");
    //            keysToCheck.Add($"{levelPrefix}_completed");
    //            keysToCheck.Add($"{levelPrefix}_achieved");
    //            keysToCheck.Add($"{levelPrefix}_maxStars");
    //        }

    //        // Load current cloud data
    //        var cloudData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>(keysToCheck));

    //        // Prepare data to sync
    //        Dictionary<string, object> dataToSync = new Dictionary<string, object>();

    //        foreach (var key in keysToCheck)
    //        {
    //            object localValue = LocalCacheManager.GetLocalData(key);
    //            string localValueStr = localValue?.ToString();
    //            string cloudValue = cloudData.ContainsKey(key) ? cloudData[key].Value.GetAsString() : null;

    //            if (localValueStr != null && localValueStr != cloudValue)
    //            {
    //                dataToSync[key] = localValue;
    //            }
    //        }

    //        // Save only the changed data to the cloud
    //        if (dataToSync.Count > 0)
    //        {
    //            await CloudSaveService.Instance.Data.Player.SaveAsync(dataToSync);
    //            Debug.Log("Local data synced with Cloud Save.");
    //        }
    //        else
    //        {
    //            Debug.Log("No data changes to sync.");
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError("Error syncing data with Cloud Save: " + e.Message);
    //    }
    //}

    //public async Task LoadDataFromCloud()
    //{
    //    if (!EnsureSignedInAsync())
    //    {
    //        Debug.LogError("Cannot load data: Player is not signed in.");
    //        return;
    //    }

    //    try
    //    {
    //        // Generate keys to load
    //        List<string> keysToLoad = new List<string>();

    //        for (int i = 1; i <= numberOfLevels; i++)
    //        {
    //            string levelPrefix = i < 10 ? $"Level_00{i}" : $"Level_0{i}";

    //            keysToLoad.Add($"{levelPrefix}");
    //            keysToLoad.Add($"{levelPrefix}_1");
    //            keysToLoad.Add($"{levelPrefix}_2");
    //            keysToLoad.Add($"{levelPrefix}_3");
    //            keysToLoad.Add($"{levelPrefix}_1_rewarded");
    //            keysToLoad.Add($"{levelPrefix}_2_rewarded");
    //            keysToLoad.Add($"{levelPrefix}_3_rewarded");
    //            keysToLoad.Add($"{levelPrefix}_completed");
    //            keysToLoad.Add($"{levelPrefix}_achieved");
    //            keysToLoad.Add($"{levelPrefix}_maxStars");
    //        }

    //        Debug.Log($"Total keys to load: {keysToLoad.Count}");

    //        // Load data from cloud
    //        var cloudData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>(keysToLoad));
    //        Debug.Log($"Total keys retrieved from cloud: {cloudData.Count}");

    //        foreach (var key in keysToLoad)
    //        {
    //            Debug.Log($"Processing key: {key}");
    //            if (cloudData.ContainsKey(key))
    //            {
    //                var cloudItem = cloudData[key];
    //                Debug.Log($"Key {key} found in cloud data.");
    //                if (cloudItem.Value != null)
    //                {
    //                    Debug.Log($"Value for key {key} is not null.");
    //                    // Handle different data types
    //                    string valueStr = cloudItem.Value.GetAsString();
    //                    Debug.Log($"Retrieved value for key {key}: {valueStr}");
    //                    if (int.TryParse(valueStr, out int intValue))
    //                    {
    //                        LocalCacheManager.SetLocalData(key, intValue);
    //                        Debug.Log($"SetLocalData: Key = {key}, Value = {intValue}");
    //                    }
    //                    else
    //                    {
    //                        LocalCacheManager.SetLocalData(key, cloudItem.Value.GetAs<int>());
    //                        Debug.Log($"SetLocalData: Key = {key}, Value = {valueStr}");
    //                    }

    //                    // Print the value to confirm
    //                    var localValue = LocalCacheManager.GetLocalData(key);
    //                    Debug.Log($"GetLocalData: Key = {key}, Value = {localValue}");
    //                }
    //                else
    //                {
    //                    Debug.LogWarning($"Value for key {key} is null in cloud data.");
    //                }
    //            }
    //            else
    //            {
    //                Debug.LogWarning($"Key {key} not found in cloud data.");
    //            }
    //        }

    //        Debug.Log("Cloud data loaded into local cache.");
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError("Error loading data from Cloud Save: " + e.Message);
    //    }
    //}

    public async Task SyncGameDataToCloud()
    {
        if (!EnsureSignedInAsync())
        {
            Debug.LogError("Cannot sync data: Player is not signed in.");
            return;
        }

        try
        {
            var gameData = new Dictionary<string, object>();

            for (int i = 1; i <= numberOfLevels; i++)
            {
                string levelPrefix = i < 10 ? $"Level_00{i}" : $"Level_0{i}";

                gameData[levelPrefix] = LocalCacheManager.GetLocalData(levelPrefix);
                gameData[$"{levelPrefix}_1"] = LocalCacheManager.GetLocalData($"{levelPrefix}_1");
                gameData[$"{levelPrefix}_2"] = LocalCacheManager.GetLocalData($"{levelPrefix}_2");
                gameData[$"{levelPrefix}_3"] = LocalCacheManager.GetLocalData($"{levelPrefix}_3");
                gameData[$"{levelPrefix}_1_rewarded"] = LocalCacheManager.GetLocalData($"{levelPrefix}_1_rewarded");
                gameData[$"{levelPrefix}_2_rewarded"] = LocalCacheManager.GetLocalData($"{levelPrefix}_2_rewarded");
                gameData[$"{levelPrefix}_3_rewarded"] = LocalCacheManager.GetLocalData($"{levelPrefix}_3_rewarded");
                gameData[$"{levelPrefix}_completed"] = LocalCacheManager.GetLocalData($"{levelPrefix}_completed");
                gameData[$"{levelPrefix}_achieved"] = LocalCacheManager.GetLocalData($"{levelPrefix}_achieved");
                gameData[$"{levelPrefix}_maxStars"] = LocalCacheManager.GetLocalData($"{levelPrefix}_maxStars");
            }
            gameData["Level_Teste_completed"] = LocalCacheManager.GetLocalData("Level_Teste_completed");

            // Convert to JSON string
            string jsonData = JsonConvert.SerializeObject(gameData);
            await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object> { { "GameData", jsonData } });

            Debug.Log("Game data synced with Cloud Save.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error syncing data with Cloud Save: " + e.Message);
        }
    }

    public async Task LoadGameDataFromCloud()
    {
        if (!EnsureSignedInAsync())
        {
            Debug.LogError("Cannot load data: Player is not signed in.");
            return;
        }

        try
        {
            var cloudData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "GameData" });

            if (cloudData.ContainsKey("GameData"))
            {
                string jsonData = cloudData["GameData"].Value.GetAsString();
                var gameData = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData); // Change to int

                foreach (var key in gameData.Keys)
                {
                    LocalCacheManager.SetLocalData(key, gameData[key]); // Directly set the int value
                }

                Debug.Log("Game data loaded into local cache.");
            }
            else
            {
                Debug.LogWarning("GameData not found in cloud.");
                await SyncGameDataToCloud();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading data from Cloud Save: " + e.Message);
        }
    }

}
