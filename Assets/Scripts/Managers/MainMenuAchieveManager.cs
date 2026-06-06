using UnityEngine;
using TMPro;

public class MainMenuAchieveManager : MonoBehaviour
{
    public static MainMenuAchieveManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject achievementPopupPrefab;
    [SerializeField] private Transform popupParent;

    private const string COIN_200_KEY = "ACH_MM_COIN_200";
    private const string COIN_500_KEY = "ACH_MM_COIN_500";

    private void Awake()
    {
        Instance = this; 
        //ResetCoinAchievements();
    }

    

     
    // Coin Achievements
     

    public void CheckCoinAchievements(int coins)
    {
        if (coins >= 200 && !IsUnlocked(COIN_200_KEY))
        {
            UnlockAchievement(COIN_200_KEY, "Collected 200 Coins!");
        }

        if (coins >= 500 && !IsUnlocked(COIN_500_KEY))
        {
            UnlockAchievement(COIN_500_KEY, "Collected 500 Coins!");
        }
    }

    
    // Helpers
     

    private bool IsUnlocked(string key)
    {
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    private void UnlockAchievement(string key, string message)
    {
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();

        ShowPopup(message);

        Debug.Log("Main Menu Achievement unlocked: " + message);
    }

    private void ShowPopup(string message)
    {
        if (achievementPopupPrefab == null || popupParent == null)
            return;

        GameObject popup = Instantiate(achievementPopupPrefab, popupParent);

        TMP_Text text = popup.GetComponentInChildren<TMP_Text>();
        if (text != null)
            text.text = message;

        Destroy(popup, 2.5f);
    }

     
    // Reset (Testing)
    

    public void ResetCoinAchievements()
    {
        PlayerPrefs.DeleteKey(COIN_200_KEY);
        PlayerPrefs.DeleteKey(COIN_500_KEY);
        PlayerPrefs.Save();

        Debug.Log("Main menu coin achievements reset");
    }
}
