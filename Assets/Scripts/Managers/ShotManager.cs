using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class ShotManager : MonoBehaviour
{
    public static ShotManager instance;

    [Header("Shot settings")]

    [SerializeField] private float shotTimeOut = 3.5f;
    [SerializeField] private int maxThrows = 9;
    [SerializeField] private Vector3 ballResetLocalPosition;

    // events

    public event Action OnShotStarted;
    public event Action<GameObject> OnShotScored;
    public event Action OnShotMissed;
    public event Action OnShotFinished;
    public event Action OnGameOver;
     




    private int throwsUsed;
    private bool shotActive;
    private bool isGameOver;
    private Coroutine shotCoroutine;
    private GameObject currentBall;

    public bool IsGameOver => isGameOver;
    public bool ShotActive=> shotActive;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    // called from ball thrower

    public void StartShot(GameObject ball)
    {
        if (IsGameOver || shotActive) return;

        throwsUsed++;
       // Debug.Log($"Throw {throwsUsed}/{maxThrows}");

        currentBall = ball;
        shotActive = true;

        OnShotStarted?.Invoke();
        SoundCoordinator.Instance.Swiped();


        if (shotCoroutine != null)
            StopCoroutine(shotCoroutine);

        shotCoroutine = StartCoroutine(ShotTimeOutCoroutine());
    }


    // called from hoop trigger

    public void RegisterScore(GameObject pos)
    {

        //Debug.Log("calling register score method on shotmanager");
        if(ShotActive==false || isGameOver) return;

        shotActive = false;

        if(shotCoroutine != null)  
            StopCoroutine(shotCoroutine );

        OnShotScored?.Invoke(pos);
        SoundCoordinator.Instance.Scored();
        FinishShot();
    }

    // calling with vector3 




    private IEnumerator ShotTimeOutCoroutine()
    {
        yield return new WaitForSeconds(shotTimeOut);

        if (shotActive)
        {
            HandleMiss();
        }
    }

    public void HandleMiss()
    {
        shotActive=false;


        ResetBall();

        OnShotMissed?.Invoke();
        SoundCoordinator.Instance.MissedSound();


        FinishShot();
    }

    private void ResetBall()
    {
        if(currentBall != null)
        {
            var rb=currentBall.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.isKinematic = true;

            }

            currentBall.transform.localPosition = ballResetLocalPosition;
        }
    }

    private void FinishShot()
    {
        CheckGameOver();
        OnShotFinished?.Invoke();
    }

    private void CheckGameOver()
    {
        if (throwsUsed >= maxThrows)
        {
            isGameOver = true;
            OnGameOver?.Invoke();
            GameManager.instance.SetState(GameState.GameOver);
            ScoreManager.Instance.InvokingGameOverWithScore();
        }
    }


}
