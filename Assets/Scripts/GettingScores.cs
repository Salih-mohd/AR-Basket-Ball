using TMPro;
using UnityEngine;

public class GettingScores : MonoBehaviour
{

    [Header("score texts")]
    [SerializeField] private TMP_Text score1;
    [SerializeField] private TMP_Text score2;
    [SerializeField] private TMP_Text score3;

    private void Start()
    {
        score1.text = $"1-> {PlayerPrefs.GetInt("HighScore1")}";
        score2.text = $"1-> {PlayerPrefs.GetInt("HighScore2")}";
        score3.text = $"1-> {PlayerPrefs.GetInt("HighScore3")}";
    }
}
