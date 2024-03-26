using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Economy;
using Unity.Services.Leaderboards;
using Newtonsoft.Json;

public class UAuthentication : MonoBehaviour
{
    private async void Awake()
    {
        // UnityServices.InitializeAsync() will initialize all services that are subscribed to Core
        await UnityServices.InitializeAsync();
        Debug.Log(UnityServices.State);

        SetupEvents();
        await SignInAnonymouslyAsync();
        SyncConfigurationAsync();

    }
    private async void Start()
    {
        
    }

    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () => {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };

    }

    private async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign-in Anonymously succeeded!");

            // Shows how to get player ID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            InventoryManager.instance.FetchInventoryItems();
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify de player with the proper error message
            Debug.LogException(ex);
        } 

    }

    public async void SyncConfigurationAsync()
    {
        await EconomyService.Instance.Configuration.SyncConfigurationAsync();
        var playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync("Pontuacoes_Mais_Altas", 0);
        Debug.Log(JsonConvert.SerializeObject(playerEntry));
        Debug.Log("Configuration sync finished");

    }
}
