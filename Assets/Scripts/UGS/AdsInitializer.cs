using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    public static AdsInitializer Instance;
    public InterstitialAdExample adIntertial;


    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;

    void Awake()
    {

        Debug.Log("AdsInit Awake");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("AdsInit moved to DDoL");
        }
        else
        {
            Debug.Log("AdsInit destroyed (duplicate)");
            Destroy(gameObject);
        }

        InitializeAds();
    }

    private void OnDestroy()
    {
        Debug.Log("AdsInit OnDestroy called");
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
        Debug.Log("Unity Ads initialization complete.");
        adIntertial.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void ShowAd()
    {
        Invoke("TimedAd", 3);
    }

    public void TimedAd()
    {
        adIntertial.ShowAd();
    }
}
