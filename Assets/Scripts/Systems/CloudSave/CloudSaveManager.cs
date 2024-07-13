using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;

public class CloudSaveManager : MonoBehaviour
{
    public static CloudSaveManager Instance { get; private set; }

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
}
