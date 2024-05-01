using System.Collections;
using System.Collections.Generic;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;

public class SkinInfo
{
    // a simple data structure to hold information about each skin, such as whether it’s unlocked,
    // its display name, and any other relevant attributes.
    public bool Available { get; set; }
    public string DisplayName { get; set; }
    public bool Owned { get; set; }
    public string Rarity { get; set; }
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
    public static SkinSystemChecker Instance;
    public Dictionary<string, SkinInfo> allSkins = new Dictionary<string, SkinInfo>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        GetSkinInformation();
    }

    //List<Skin> availableSkins = 

    public void GetSkinInformation()
    {
        List<InventoryItemDefinition> iventoryOptions = EconomyService.Instance.Configuration.GetInventoryItems();
        if (iventoryOptions.Count > 0)
        {
            foreach (InventoryItemDefinition item in iventoryOptions)
            {
                SkinInfo skinInfo = item.CustomDataDeserializable.GetAs<SkinInfo>();
                print(skinInfo.Available);
                print(skinInfo.DisplayName);
                print(skinInfo.Owned);
                print(skinInfo.Rarity);
                allSkins.Add(item.Id, skinInfo);
            }
        }

        
    }



}
