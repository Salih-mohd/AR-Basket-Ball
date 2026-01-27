using System.Threading.Tasks;

using Unity.Services.Analytics;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UnityConsent;

public class UGSManager : MonoBehaviour
{
    public static UGSManager Instance;
    public GameObject loginButton;
    public string sceneName;

    private async void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        await InitializeUGS();
    }
    void Start()
    {
        
    }


    private void OnDestroy()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            PlayerAccountService.Instance.SignedIn -= OnPlayerAccountSignedIn;
        }
    }

    // UGS INITIALIZATION

    private async Task InitializeUGS()
    {
        await UnityServices.InitializeAsync();
        Debug.Log("UGS Initialized");

        EndUserConsent.SetConsentState(new ConsentState
        {
            AnalyticsIntent = ConsentStatus.Granted,
            AdsIntent = ConsentStatus.Granted
        });

        Debug.Log("Consent Granted");

        PlayerAccountService.Instance.SignedIn += OnPlayerAccountSignedIn;
    }

  
    // PLAYER ACCOUNT SIGN-IN
    
    public async void StartPlayerAccountsSignInAsync()
    {
        if (PlayerAccountService.Instance.IsSignedIn)
        {
            await SignInWithUnityAuth();
            return;
        }

        try
        {
            await PlayerAccountService.Instance.StartSignInAsync();
        }
        catch (PlayerAccountsException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    private async void OnPlayerAccountSignedIn()
    {
        await SignInWithUnityAuth();
    }

    private async Task SignInWithUnityAuth()
    {
        try
        {
            await AuthenticationService.Instance
                .SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);

            Debug.Log("Unity Authentication successful");

            loginButton?.SetActive(false);

            // Safe to send analytics AFTER auth + consent
            GameAnalyticsEvents.Instance.GameHasStarted();
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    private void UpdateLoginUI()
    {
        bool isLoggedIn = AuthenticationService.Instance.IsSignedIn;

        if (loginButton != null)
            loginButton.SetActive(!isLoggedIn);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name==sceneName)
        {
            loginButton = GameObject.FindWithTag("LoginButt");
            if (UnityServices.State == ServicesInitializationState.Initialized)
                UpdateLoginUI();
        }
    }
    
}
