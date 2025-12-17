using System;
using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Score Settings")]
    public int baseScore = 5;
    public int streakBonusAt = 3;
    public int streakBonusScore = 15;
    public int onFireScore = 20;

    [Header("Shot settings")]
    public float shotTimeOut = 3.5f;
    public Vector3 ballPosition;


    //events
    public event Action OnShotStart;
    public event Action OnShotFinish;

    public event Action GameOver;


    // Throw limits
    [SerializeField] private int maxThrows = 9;    
    private int throwsUsed = 0;                    
    public bool IsGameOver { get; private set; }






    public int TotalScore { get; private set; }
    public int CurrentStreak { get; private set; }

    private bool isOnFire;
    private bool shotActive;
    private Coroutine shotCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }



    // called from BallThrower

    public void OnShotStarted(GameObject obj)
    {

        if (IsGameOver) return;

        throwsUsed++;
        Debug.Log($"Throw {throwsUsed}/{maxThrows}");

        // invoking event
        OnShotStart?.Invoke();


        if(shotCoroutine != null)
            StopCoroutine(shotCoroutine);

        shotActive = true;
        shotCoroutine = StartCoroutine(ShotTimer(obj));
    }

    private IEnumerator ShotTimer(GameObject obj)
    {
         yield return new WaitForSeconds(shotTimeOut);

        if (shotActive)
        {

            // consider as miss

            obj.GetComponent<Rigidbody>().isKinematic = true;
            obj.transform.localPosition = ballPosition;
            shotActive=false;
            Debug.Log("time out missing");
            
            OnMiss();

            //invoking event
            OnShotFinish?.Invoke();
        }
    }



    // Call this when a basket is scored
    public void OnBasketScored()
    {

        shotActive = false;

        if(shotCoroutine!=null)
            StopCoroutine (shotCoroutine);

        CurrentStreak++;

        int pointsToAdd = baseScore;

        // Enter streak bonus
        if (CurrentStreak == streakBonusAt)
        {
            pointsToAdd += streakBonusScore;
            isOnFire = true;
        }
        // On-fire mode
        else if (CurrentStreak > streakBonusAt)
        {
            pointsToAdd = onFireScore;
        }

        TotalScore += pointsToAdd;


        CheckGameOver();
        OnShotFinish?.Invoke();

        Debug.Log($"SCORE: +{pointsToAdd} | Total: {TotalScore} | Streak: {CurrentStreak}");
    }

    // Call this on a miss
    public void OnMiss()
    {
        CurrentStreak = 0;
        isOnFire = false;

        Debug.Log("Streak reset");


        CheckGameOver();
    }


    private void CheckGameOver()
    {
        if (throwsUsed >= maxThrows)
        {
            IsGameOver = true;
            shotActive = false;

            Debug.Log("Game over");
            GameOver?.Invoke();
        }
    }
}
