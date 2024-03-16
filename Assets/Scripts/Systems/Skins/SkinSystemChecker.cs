using System.Collections;
using System.Collections.Generic;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;

public class SkinInfo
{
    // a simple data structure to hold information about each skin, such as whether it’s unlocked,
    // its display name, and any other relevant attributes.
    public bool IsUnlocked { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
}

public class Skin
{
    public string ID { get; private set; } // Unique identifier for the skin
    public string Name { get; private set; } // Display name of the skin
    public string Description { get; private set; } // Description of the skin
    public Sprite Image { get; private set; } // Image associated with the skin
    public int Cost { get; private set; } // Cost of the skin in in-game currency
}
public class SkinSystemChecker : MonoBehaviour
{
    /*
    Dictionary<string, SkinInfo> allSkins = new Dictionary<string, SkinInfo>();
    //List<Skin> availableSkins = 
    List<GetInventoryResult> iventoryOptions = EconomyService.Instance.PlayerInventory.GetInventoryAsync();
    
    public void GetSkinInformation()
    {
        List<GetInventoryResult> iventoryOptions = EconomyService.Instance.Configuration.GetInventoryItems();
        if(iventoryOptions.Count > 0)
        {
            foreach(GetInventoryResult item in iventoryOptions)
            {
                allSkins.Add(item.Id, item.)
            }
        }
    }
    */
}
