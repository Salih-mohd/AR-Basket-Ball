using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [SerializeField] private TMP_Text coinText;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private string sceneName;
    private const string COIN_KEY = "COINS";
    private int coins;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        coinText = GameObject.FindWithTag("CoinText").GetComponent<TMP_Text>();
        LoadCoins();
        UpdateUI();

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    
    private void LoadCoins()
    {
        coins = PlayerPrefs.GetInt(COIN_KEY, 0);
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt(COIN_KEY, coins);
        PlayerPrefs.Save();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveCoins();
        UpdateUI();
    }

    public int GetCoins()
    {
        return coins;
    }

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = coins.ToString();
        else Debug.Log("coin text is null");
    }

    public void UnlockColor(int coinNeeded, string hexColor)
    {
        if (coins < coinNeeded)
        {
            Debug.Log("Not enough coins");
            return;
        }

        if (!ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            Debug.LogError("Invalid hex color: " + hexColor);
            return;
        }

        // Apply color safely
        MeshRenderer renderer = ballPrefab.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            Debug.LogError("MeshRenderer not found on ball prefab");
            return;
        }

        renderer.material.color = newColor;

        // Deduct coins
        SpendCoins(coinNeeded);
    }


    public void SpendCoins(int amount)
    {
        coins -= amount;
        coins = Mathf.Max(coins, 0);
        SaveCoins();
        UpdateUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneName)
        {
            GameObject coinObj = GameObject.FindWithTag("CoinText");

            if (coinObj != null)
            {
                coinText = coinObj.GetComponent<TMP_Text>();
                LoadCoins();
                UpdateUI();
            }
            else
            {
                Debug.LogWarning("CoinText not found in scene: " + scene.name);
            }
        }
        
    }


}
