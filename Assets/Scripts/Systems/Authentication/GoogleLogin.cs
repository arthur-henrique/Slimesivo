using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using TMPro;

public class GoogleLogin : MonoBehaviour
{
    public TextMeshProUGUI DetailsText;
    public TextMeshProUGUI TokenText;
    public string Token;
    public string Error;
    void Awake()
    {
        PlayGamesPlatform.Activate();
    }

    async void Start()
    {
        await LoginGooglePlayGames();
        //await SignInWithGooglePlayGamesAsync(Token);
    }

    public async void SignInWithGPG()
    {
        await SignInWithGooglePlayGamesAsync(Token);
    }
    //Fetch the Token / Auth code
    public Task LoginGooglePlayGames()
    {
        var tcs = new TaskCompletionSource<object>();
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play Games successful.");
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Authorization code: " + code);
                    Token = code;
                    TokenText.text = Token;
                    // This token serves as an example to be used for SignInWithGooglePlayGames
                    tcs.SetResult(null);
                });
            }
            else
            {
                Error = "Failed to retrieve Google Play Games authorization code";
                Debug.LogError("Login Unsuccessful");
                tcs.SetException(new Exception("Failed to retrieve Google Play Games authorization code"));
            }
        });
        return tcs.Task;
    }


    async Task SignInWithGooglePlayGamesAsync(string authCode)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); // Display the Unity Authentication PlayerID
            DetailsText.text = "SignIn is successful.";
            Debug.Log("SignIn is successful.");

            if (AuthenticationService.Instance.IsSignedIn)
            {
                // The player is authenticated
                // Proceed with the game
                UAuthentication.Instance.LogIn();
            }
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
            DetailsText.text = "SignIn is unsuccessful. " + ex.Message;
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
            DetailsText.text = "SignIn is unsuccessful. " + ex.Message;
        }
    }
}
