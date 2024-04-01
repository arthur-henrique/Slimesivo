using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Economy;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Economy.Model;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance = null;

    // Currency Related Variables
    long currentCoinAmount; // The amount of coinCurrency the player currently holds in a match
    long currentCurrency; // The quantity of coinCurrency the player current has
    // Trocar pelo ID da moeda desejada
    public string coinCurrencyID = "TESTCURRENCY";

    private void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;
        }
    }

    // Gets the updated currentCurrency
    public async void FetchCoinBalance()
    {
        if (!IsAuthenticationSignedIn())
        {
            return;
        }

        GetBalancesResult result = await EconomyService.Instance.PlayerBalances.GetBalancesAsync();
        if (result.Balances.Count == 0)
        {
            Debug.Log("No balances");
            return;
        }
        else
        {
            foreach (var balance in result.Balances)
            {
                CurrencyDefinition currency = EconomyService.Instance.Configuration.GetCurrency(balance.CurrencyId);
                if (currency.Id == coinCurrencyID)
                {
                    currentCurrency = balance.Balance;
                    Debug.Log(currentCurrency);
                    Debug.Log($"{currency.Id}: {balance.Balance} ");
                    break;
                }
            }
            
        }

        
    }

    public async void SetBalance(int balanceToAdd)
    {
        if (!IsAuthenticationSignedIn())
        {
            return;
        }
        ;
        currentCurrency += balanceToAdd;
        PlayerBalance playerBalance = await EconomyService.Instance.PlayerBalances.SetBalanceAsync(coinCurrencyID, currentCurrency);
        Debug.Log(currentCurrency);
    }

    static bool IsAuthenticationSignedIn()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log("Wait until sign in is done");
            return false;
        }

        return true;
    }

    public void UpdateCoinAmount(int coinAmount)
    {
        currentCoinAmount += coinAmount;
        Debug.Log("Current Coin Amount is: " +currentCoinAmount);
    }
}
