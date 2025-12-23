using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UITweanGamePlay : MonoBehaviour
{
    [Header("scanning info UI")]
    [SerializeField] private GameObject scan_Image;


    [Header("swipe info UI")]
    [SerializeField] private GameObject swipe_image;

    [Header("pool info")]
    [SerializeField] private ScorePopPool scorePool;

    [Header("pause ui info")]
    [SerializeField] private Button homeButton;
    [SerializeField] private GameObject pauseImage;
    [SerializeField] private GameObject pausePanel;


    [Header("Gameo over reference ")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject ballImage;
    [SerializeField] private GameObject hoopImage;

    [SerializeField] private GameObject gameOverUI;


    // events
    public event Action GamePaused;
    public event Action GameContinue;

    private void OnEnable()
    {
        GameManager.instance.ScanningEvent += ShowScanInfoUI;
        GameManager.instance.PlayingEvent += ShowThrowInfoUI;
        GameManager.instance.GameOverEvent += TweanBallAndHoop;

        // managing button events
        homeButton.onClick.AddListener(GoHome);
    }

    private void OnDisable()
    {
        GameManager.instance.ScanningEvent -= ShowScanInfoUI;
        GameManager.instance.PlayingEvent -= ShowThrowInfoUI;

        GameManager.instance.GameOverEvent -= TweanBallAndHoop;

        // managing button events
        homeButton.onClick.RemoveListener(GoHome);
    }


    private void ShowScanInfoUI()
    {
        StartCoroutine(ShowScanInfoCoro());
    }

    IEnumerator ShowScanInfoCoro()
    {
        LeanTween.moveLocal(scan_Image, new Vector3(110, 551, 0), 2f)
            .setEaseOutBack();


        yield return new WaitForSeconds(10);
        LeanTween.moveLocal(scan_Image, new Vector3(-890, 551, 0), 4f)
            .setEaseOutBack();
    }

    private void ShowThrowInfoUI()
    {
        StartCoroutine(ShowThrowUICoro());
    }

    IEnumerator ShowThrowUICoro()
    {
        LeanTween.moveLocal(swipe_image, new Vector3(110, 551, 0), 2f)
            .setEaseOutBack();
        yield return new WaitForSeconds(10);
        LeanTween.moveLocal(swipe_image, new Vector3(-890, 551, 0), 4f)
            .setEaseOutBack();
    }

    public void AnimateScorePop(GameObject obj,CanvasGroup grp)
    {

        //Debug.Log("called animation of pop score ui");
        LeanTween.moveLocal(obj, new Vector3(0, 90, 0), 1)
            .setEaseOutQuint();

        LeanTween.alphaCanvas(grp, 0f, 3f)
                 .setEaseOutQuint()
                 .setOnComplete(() =>
                 {
                     scorePool.Return(obj);
                     grp.alpha = 1f;
                     //Debug.Log("obj is returned");

                 });
    }


    public void TweanPauseImage(bool isPaused)
    {
        if(isPaused)
        {
            pausePanel.SetActive(true);
            LeanTween.scale(pauseImage, Vector3.one*10, .5f)
                .setEaseOutQuint();
        }
        else
        {
            LeanTween.scale(pauseImage, Vector3.one, .3f)
                .setEaseOutQuint()
                .setOnComplete(() => 
                {
                    pausePanel.SetActive(false);
                     

                });
        }
        
    }

    // ball and hoop tweanign
    private void TweanBallAndHoop()
    {
        gameOverPanel.SetActive(true);
        LeanTween.moveLocal(ballImage, new Vector3(-261, -443, 0), 5f)
         .setLoopClamp()
         .setEaseShake();

        LeanTween.moveLocal(hoopImage, new Vector3(584, 398, 0), 5f)
         .setLoopClamp()
         .setEaseShake();

        LeanTween.scale(gameOverUI, Vector3.one, 1f)
            .setEaseOutBack();
    }



    // for buttons
    public void GoHome()
    {
        gameOverPanel.SetActive(false);
        ManagerScene.instance.GoHome();
    }

    
}
