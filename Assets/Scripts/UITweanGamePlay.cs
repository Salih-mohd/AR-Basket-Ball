using System.Collections;
using UnityEngine;

public class UITweanGamePlay : MonoBehaviour
{
    [Header("scanning info UI")]
    [SerializeField] private GameObject scan_Image;


    [Header("swipe info UI")]
    [SerializeField] private GameObject swipe_image;

    [Header("pool info")]
    [SerializeField] private ScorePopPool scorePool;

    private void OnEnable()
    {
        GameManager.instance.ScanningEvent += ShowScanInfoUI;
        GameManager.instance.PlayingEvent += ShowThrowInfoUI;
    }

    private void OnDisable()
    {
        GameManager.instance.ScanningEvent -= ShowScanInfoUI;
        GameManager.instance.PlayingEvent -= ShowThrowInfoUI;
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
}
