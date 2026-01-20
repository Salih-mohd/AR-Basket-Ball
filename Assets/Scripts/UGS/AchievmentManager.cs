using UnityEngine;
using TMPro;

public class AchievementManager : MonoBehaviour
{

    public static AchievementManager Instance;

    [Header("UI")]
    public GameObject achievementPopupPrefab;
    public Transform popupParent;

    private const string SCORE_10_KEY = "ACH_SCORE_10";
    private const string SCORE_30_KEY = "ACH_SCORE_30";

    void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //ResetAllAchievements();
    }

    public void CheckAchievements(int finalScore)
    {
        if (finalScore >= 10 && !IsUnlocked(SCORE_10_KEY))
        {
            UnlockAchievement(SCORE_10_KEY, "Score 10 Reached!");
        }

        if (finalScore >= 30 && !IsUnlocked(SCORE_30_KEY))
        {
            UnlockAchievement(SCORE_30_KEY, "Score 30 Reached!");
        }
    }

    bool IsUnlocked(string key)
    {
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    void UnlockAchievement(string key, string message)
    {
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();

        ShowPopup(message);

        Debug.Log("Achievement unlocked: " + message);
    }

    void ShowPopup(string message)
    {
        GameObject popup = Instantiate(achievementPopupPrefab, popupParent);

        TMP_Text text = popup.GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.text = message;
        }

        Destroy(popup, 2.5f);
    }

    public void ResetAllAchievements()
    {
        PlayerPrefs.DeleteKey(SCORE_10_KEY);
        PlayerPrefs.DeleteKey(SCORE_30_KEY);
        PlayerPrefs.Save();

        Debug.Log("All achievements reset");
    }
}
