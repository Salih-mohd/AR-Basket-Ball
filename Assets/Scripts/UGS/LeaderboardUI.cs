using UnityEngine;
using Unity.Services.Leaderboards;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    [Header("Leaderboard")]
    [SerializeField] private string leaderboardId = "YOUR_LEADERBOARD_ID";

    [Header("UI")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject rowPrefab;

    public async void ShowTop3()
    {
        // Clear old entries
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        try
        {
            // Request ONLY top 3 scores
            var scores = await LeaderboardsService.Instance.GetScoresAsync(
                leaderboardId,
                new GetScoresOptions { Limit = 3 }
            );

            int rank = 1;

            foreach (var entry in scores.Results)
            {
                GameObject row = Instantiate(rowPrefab, contentParent);

                TMP_Text text = row.GetComponentInChildren<TMP_Text>();

                if (text != null)
                {
                    string playerName = string.IsNullOrEmpty(entry.PlayerName)
                        ? entry.PlayerId.Substring(0, 6)
                        : entry.PlayerName;

                    text.text = $"{rank}. {playerName} - {entry.Score}";
                }

                rank++;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load leaderboard: " + e.Message);
        }
    }
}
