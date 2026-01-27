using System.Threading.Tasks;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UnityConsent;

public class AnalyticsInit : MonoBehaviour
{

    public static AnalyticsInit Instance;
    async void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        await UnityServices.InitializeAsync();
        Debug.Log("UGS Initialized");

        EndUserConsent.SetConsentState(new ConsentState
        {
            AnalyticsIntent = ConsentStatus.Granted,
            AdsIntent = ConsentStatus.Granted
        });

        // Safe to send event now
        GameAnalyticsEvents.Instance.GameHasStarted();
    }


    

}
