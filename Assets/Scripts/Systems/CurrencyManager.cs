using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Economy;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Economy.Model;

public class CurrencyManager : MonoBehaviour
{
    long currentCoinAmount;
    // Trocar pelo ID da moeda desejada
    public string coinCurrencyID = "TESTCURRENCY";

    
    // Gets the updated currentCoinAmount
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
                    currentCoinAmount = balance.Balance;
                    Debug.Log(currentCoinAmount);
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
        currentCoinAmount += balanceToAdd;
        PlayerBalance playerBalance = await EconomyService.Instance.PlayerBalances.SetBalanceAsync(coinCurrencyID, currentCoinAmount);
        Debug.Log(currentCoinAmount);
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
}
