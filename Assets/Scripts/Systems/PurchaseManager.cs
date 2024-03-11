using System.Collections;
using System.Collections.Generic;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    public async void MakePurchase(string purchaseId)
    {
        MakeVirtualPurchaseResult result = await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync(purchaseId);
    }
}
