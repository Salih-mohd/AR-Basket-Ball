using UnityEngine;
using TMPro;

public class ColorUnlockButton : MonoBehaviour
{
    [Header("Unlock Settings")]
    [SerializeField] private int coinNeeded = 100;
    [SerializeField] private string hexColor = "#FF5733";
    [SerializeField] private GameObject ballPrefab;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI statusText;

    private string prefKey;

    private void Awake()
    {
        prefKey = "COLOR_UNLOCKED_" + hexColor.Replace("#", "");
        LoadState();
    }

    public void OnButtonClicked()
    {
        if (IsUnlocked())
        {
            ApplyColor();
            return;
        }

        // Try to unlock
        if (CoinManager.Instance.GetCoins() < coinNeeded)
        {
            Debug.Log("Not enough coins");
            return;
        }

        Unlock();
    }

    // ------------------------
    // Core Logic
    // ------------------------

    private void Unlock()
    {
        CoinManager.Instance.SpendCoins(coinNeeded);
        SaveUnlocked();
        ApplyColor();
        UpdateUI(true);
    }

    private void ApplyColor()
    {
        if (!ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            Debug.LogError("Invalid hex color");
            return;
        }

        MeshRenderer renderer = ballPrefab.GetComponent<MeshRenderer>();
        renderer.sharedMaterial.color = newColor;
    }

    // ------------------------
    // Persistence
    // ------------------------

    private bool IsUnlocked()
    {
        return PlayerPrefs.GetInt(prefKey, 0) == 1;
    }

    private void SaveUnlocked()
    {
        PlayerPrefs.SetInt(prefKey, 1);
        PlayerPrefs.Save();
    }

    private void LoadState()
    {
        bool unlocked = IsUnlocked();
        UpdateUI(unlocked);
    }

    // ------------------------
    // UI
    // ------------------------

    private void UpdateUI(bool unlocked)
    {
        if (statusText == null) return;

        statusText.text = unlocked ? "OWNED" : coinNeeded.ToString();
    }
}
