using System.Collections;
using System.Collections.Generic;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;


[System.Serializable]
public class InventoryItem
{
    public string Name;
    public string ID;
    public string Rarity;
    public bool Available;
    public bool Owned;
}

[System.Serializable]
public class CustomData
{
    public string Rarity;
    public bool Available;
    public bool Owned;
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance = null;
    public List<PlayersInventoryItem> playerInventory;



    private Dictionary<string, InventoryItem> inventory = new Dictionary<string, InventoryItem>();
    private Dictionary<string, PlayersInventoryItem> inventoryDictionary = new Dictionary<string, PlayersInventoryItem>();

    private void Awake()
    {
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;
        }
        // If instance already exists and it's not this:
        else if (instance != this)
        {
            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
    }
    public async void FetchInventoryItems()
    {
        GetInventoryResult inventoryResult = await EconomyService.Instance.PlayerInventory.GetInventoryAsync(null);
        List<PlayersInventoryItem> playerInventoryItems = inventoryResult.PlayersInventoryItems;
        playerInventory = playerInventoryItems;
        foreach (PlayersInventoryItem item in playerInventoryItems)
        {
            Debug.Log("Item: " + item.InventoryItemId + " - ");
            Debug.Log("Item: " + item.PlayersInventoryItemId + " - ");
        }
        if (playerInventoryItems.Count == 0)
        {
            Debug.Log("No items in inventory");
        }
    }

    public void SaveInventory()
    {
        string json = JsonUtility.ToJson(new List<InventoryItem>(inventory.Values));
        PlayerPrefs.SetString("Inventory", json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        string json = PlayerPrefs.GetString("Inventory", "{}");
        List<InventoryItem> itemList = JsonUtility.FromJson<List<InventoryItem>>(json);
        inventory.Clear();
        foreach (var item in itemList)
        {
            inventory.Add(item.ID, item);
        }
    }

    public List<InventoryItem> GetOwnedItems()
    {
        List<InventoryItem> ownedItems = new List<InventoryItem>();
        foreach (var item in inventory.Values)
        {
            if (item.Owned)
            {
                ownedItems.Add(item);
            }
        }
        return ownedItems;
    }
}
