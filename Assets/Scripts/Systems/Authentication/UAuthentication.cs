using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Threading.Tasks;
using Unity.Services.Economy;
using Unity.Services.Leaderboards;
using Newtonsoft.Json;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;

public class UAuthentication : MonoBehaviour
{
    public static UAuthentication Instance;
    // Login Buttons
    public GameObject playOfflineButton;
    public GameObject anonymousLoginButton;
    public GameObject facebookLoginButton;

    // Second Step 
    public GameObject facebookStuff;




    private async void Awake()
    {
        // Check if instance already exists
        if (Instance == null)
        {
            // If not, set instance to this
            Instance = this;
        }
        // UnityServices.InitializeAsync() will initialize all services that are subscribed to Core
        await UnityServices.InitializeAsync();
        Vibration.Init();
        Debug.Log(UnityServices.State);

        SetupEvents();
        //await SignInAnonymouslyAsync();
        //SyncConfigurationAsync();

    }
    public async void SignIn()
    {
        await SignInAnonymouslyAsync();
        SyncConfigurationAsync();
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

    public async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign-in Anonymously succeeded!");

            // Shows how to get player ID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            //InventoryManager.instance.FetchInventoryItems();
            LogIn();
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

    public async Task SignInWithFacebook(string token)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithFacebookAsync(token);
            print("SignIn with FB Success");
        }
        catch (AuthenticationException ex)
        {

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
    public void LogIn()
    {
        SyncConfigurationAsync();
        StartCoroutine(LoginIntoTheGame(.25f));
    }

    public void OfflineLogin()
    {
        AuthenticationService.Instance.SignOut();
        StartCoroutine(LoginIntoTheGame(.25f));

    }
    IEnumerator LoginIntoTheGame(float time)
    {
        //while (!AuthenticationService.Instance.IsSignedIn)
        //{
        //    print("WaitingToSignIn");
        //    yield return null;
        //}
        yield return new WaitForSeconds(time);

        if(PlayerPrefs.GetInt("Level_Teste_completed") != 1)
        {
            SceneManager.LoadScene("Level_Teste");
        }
        else
        {
            SceneManager.LoadScene("1 - Main Menu");
        }
    }
}
