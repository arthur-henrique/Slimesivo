using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    public static AdsInitializer instance;

    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = false;
    private string _gameId;

    [SerializeField]
    RewardedAdExample _example;
    [SerializeField]
    InterstitialAdExample _interstitialAdExample;
    void Awake()
    {

        // Check if instance already exists
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
        InitializeAds();
    }
    private void Start()
    {
        StartCoroutine(LoadTheAds());
    }

    public void LoadAds()
    {
        StartCoroutine(LoadTheAds());
    }
    public void InitializeAds()
    {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
            _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }


    public void OnInitializationComplete()
    {
        Debug.LogWarning("Unity Ads initialization complete.");
        _example.LoadAd();
        _interstitialAdExample.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    IEnumerator LoadTheAds()
    {
        yield return new WaitForSeconds(0.5f);
        _example.LoadAd();
        _interstitialAdExample.LoadAd();
    }
}