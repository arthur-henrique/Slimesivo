using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Economy;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Economy.Model;
using UnityEngine.SceneManagement;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance = null;
    [SerializeField]
    private TMP_Text coinText;
    [SerializeField]
    private TMP_Text tutorialCoinText;
    [SerializeField]
    private TMP_Text tokenText;

    // Currency Related Variables
    public long currentCoinAmount; // The amount of coinCurrency the player currently holds in a match
    public long questCoinAmount;
    public long currentCurrency; // The quantity of coinCurrency the player current has
    // Trocar pelo ID da moeda desejada
    public string coinCurrencyID = "TESTCURRENCY";

    public long levelCoinAmount;
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

    public async void SetBalance()
    {
        if (!IsAuthenticationSignedIn())
        {
            return;
        };

        //FetchCoinBalance();
        currentCurrency += currentCoinAmount;
        currentCurrency += questCoinAmount;
        levelCoinAmount = currentCoinAmount;
        PlayerBalance playerBalance = await EconomyService.Instance.PlayerBalances.SetBalanceAsync(coinCurrencyID, currentCurrency);
        //Debug.Log(currentCurrency);
        currentCoinAmount = 0;
        questCoinAmount = 0;
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
        coinText.text = currentCoinAmount.ToString();
        tutorialCoinText.text = currentCoinAmount.ToString();
        Debug.Log("Current Coin Amount is: " +currentCoinAmount);
    }
    public void QuestCoinReward(int coinAmount)
    {
        questCoinAmount += coinAmount;
        Debug.Log("Quest Coin reward is: " + questCoinAmount);
    }

    public void AdCoinMultiplier()
    {
        currentCoinAmount = levelCoinAmount;
        SetBalance();
        GameManagerMainMenuCanvasScript.Instance.UpdateCoins();
        levelCoinAmount = 0;
    }
}
