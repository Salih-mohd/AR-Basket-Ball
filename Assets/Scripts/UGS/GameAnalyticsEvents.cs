using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.UnityConsent;

public class GameAnalyticsEvents : MonoBehaviour
{
    public static GameAnalyticsEvents Instance;

    void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        
    }

    public void GameHasStarted()
    {
        AnalyticsService.Instance.RecordEvent("GameStarted");
        Debug.Log("Game started event sent");
    }


    private void OnApplicationQuit()
    {
        AnalyticsService.Instance.RecordEvent("GameEnded");
        Debug.Log("GameEnded event sent");
    }
}
