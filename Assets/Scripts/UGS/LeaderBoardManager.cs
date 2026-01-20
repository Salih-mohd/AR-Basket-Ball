using UnityEngine;
using Unity.Services.Leaderboards;
using System.Threading.Tasks;

public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager Instance;

    [SerializeField]
    private string leaderboardId = "BB_LeaderBoard";

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

    public async void SubmitScore(int score)
    {
        try
        {
            await LeaderboardsService.Instance
                .AddPlayerScoreAsync(leaderboardId, score);

            Debug.Log("Score submitted: " + score);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to submit score: " + e.Message);
        }
    }
}
