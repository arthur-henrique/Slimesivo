using System.Collections;
using System.Collections.Generic;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance = null;
    public List<PlayersInventoryItem> playerInventory;

    private void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;
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
}
