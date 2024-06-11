using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Facebook.Unity;
using System;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Core;

public class FacebookLogin : MonoBehaviour
{
    public TextMeshProUGUI FB_userName;
    public RawImage rawImg;

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.LogError("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        Time.timeScale = isGameShown ? 1 : 0;
    }

    public void Facebook_LogIn()
    {
        List<string> permissions = new List<string> { "public_profile", "email" };
        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }

    private void AuthCallBack(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var accessToken = AccessToken.CurrentAccessToken.TokenString;
            Debug.Log("Access Token: " + accessToken);
            SignInWithUnityServices(accessToken);
            DealWithFbMenus(true);
        }
        else
        {
            Debug.LogError("Facebook login failed");
        }
    }


    private async void SignInWithUnityServices(string accessToken)
    {
        try
        {
            Debug.Log("Attempting to sign in with token: " + accessToken);
            await AuthenticationService.Instance.SignInWithFacebookAsync(accessToken);
            Debug.Log("Signed in with Unity Services using Facebook token.");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError($"SignInWithFacebook failed: {ex.Message}");
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError($"SignInWithFacebook failed: {ex.Message}");
        }
    }

    private void DealWithFbMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
            UAuthentication.Instance.facebookLoginButton.SetActive(false);
            UAuthentication.Instance.anonymousLoginButton.SetActive(false);
            UAuthentication.Instance.facebookStuff.SetActive(true);
        }
        else
        {
            Debug.Log("User not logged in");
        }
    }

    private void DisplayUsername(IResult result)
    {
        if (result.Error == null)
        {
            string name = result.ResultDictionary["first_name"].ToString();
            if (FB_userName != null) FB_userName.text = name;
            Debug.Log("Username: " + name);
        }
        else
        {
            Debug.LogError(result.Error);
        }
    }

    private void DisplayProfilePic(IGraphResult result)
    {
        if (result.Texture != null)
        {
            rawImg.texture = result.Texture;
            Debug.Log("Profile picture updated.");
        }
        else
        {
            Debug.LogError(result.Error);
        }
    }

    public void Facebook_LogOut()
    {
        StartCoroutine(LogOut());
    }

    private IEnumerator LogOut()
    {
        FB.LogOut();
        while (FB.IsLoggedIn)
        {
            Debug.Log("Logging Out");
            yield return null;
        }
        Debug.Log("Logout Successful");
        if (FB_userName != null) FB_userName.text = "";
        if (rawImg != null) rawImg.texture = null;
    }

    public void FacebookSharefeed()
    {
        string url = "https://developers.facebook.com/docs/unity/reference/current/FB.ShareLink";
        FB.ShareLink(
            new Uri(url),
            "Checkout COCO 3D channel",
            "I just watched " + "22" + " times of this channel",
            null,
            ShareCallback);
    }

    private static void ShareCallback(IShareResult result)
    {
        Debug.Log("ShareCallback");
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            return;
        }
        Debug.Log(result.RawResult);
    }
}
