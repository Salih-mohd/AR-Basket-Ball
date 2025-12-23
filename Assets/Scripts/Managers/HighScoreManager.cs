using UnityEngine;

public class HighScoreManager : MonoBehaviour
{

    private const string highScore1 = "HighScore1";
    private const string highScore2 = "HighScore2";
    private const string highScore3 = "HighScore3";

    public int[] scores = new int[3];


    private void Start()
    {
        scores[0] = PlayerPrefs.GetInt(highScore1, 0);
        scores[1] = PlayerPrefs.GetInt(highScore2, 0);
        scores[2] = PlayerPrefs.GetInt(highScore3, 0);

        Debug.Log($"high score is {scores[0]} middle score is {scores[1]} low score is {scores[2]}");

        
    }

    private void OnEnable()
    {
        ScoreManager.Instance.OnGameOver += UpdateScore;
    }

    private void OnDisable()
    {
        ScoreManager.Instance.OnGameOver -= UpdateScore;
    }


    public void UpdateScore(int newScore)
    {

        Debug.Log(" updating score");
        for (int i = 0; i < scores.Length; i++)
        {
            if (newScore > scores[i])
            {
                // shift scores down
                for (int j = scores.Length - 1; j > i; j--)
                {
                    scores[j] = scores[j - 1];
                }

                scores[i] = newScore;
                SaveScores();
                PrintScores();
                return;
            }
        }

       
    }

    private void SaveScores()
    {
        PlayerPrefs.SetInt(highScore1, scores[0]);
        PlayerPrefs.SetInt(highScore2, scores[1]);
        PlayerPrefs.SetInt (highScore3, scores[2]);

        PlayerPrefs.Save();
    }

    private void PrintScores()
    {
        foreach(var a in scores)
        {
            Debug.Log(a.ToString());
        }
    }

}
