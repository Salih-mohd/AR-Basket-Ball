using UnityEngine;

public class UITweanMainMenu : MonoBehaviour
{

    [Header("centre panel")]
    [SerializeField] private GameObject text;
    [SerializeField] private Vector3 textFinalPosition;
    [SerializeField] private Vector3 textStartPosition;
    [SerializeField] private Vector3 textStartScale ;
    [SerializeField] private Vector3 textEndScale ;
    [SerializeField] private GameObject playButton;

    [Header("top Panel")]
    [SerializeField] private GameObject coinImage;
    [SerializeField] private GameObject scoreButton;
    [SerializeField] private GameObject settingsButton;

    [Header("Bottom panel")]
    [SerializeField] private GameObject shopButton;
    [SerializeField] private GameObject duooButton;

    [Header("ball and hoop")]
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject hoop;
    private void Start()
    {
        TweanBallAndHoop();
        FirstTweanIn();
    }

    private void FirstTweanIn()
    {
        LeanTween.moveLocal(text, textFinalPosition, 1f)
            .setEaseInOutQuint()
            .setOnComplete(() => 
            {
                LeanTween.moveLocal(playButton, new Vector3(0, -200, 0), 1f)
                .setEaseInOutQuint()
                .setOnComplete(() =>
                {
                    LeanTween.scale(playButton, new Vector3(1.1f, 1.1f, 1.1f), .5f)
                    .setEaseInOutQuint();
                    TweanBottomPanel();
                    TweanTopPanel();
                });
            });
             
    }

    private void TweanBottomPanel()
    {
        LeanTween.moveLocal(coinImage, new Vector3(-246.4342f, 0.00012398f, 0), 1f)
            .setEaseOutElastic();
            
            
        LeanTween.moveLocal(scoreButton,new Vector3(248f, 2.0981e-05f,0),.8f)
            .setEaseOutElastic();

        LeanTween.moveLocal(settingsButton,new Vector3(408, 2.0981e-05f,0),.7f)
            .setEaseOutElastic();

    }
    private void TweanTopPanel()
    {
        LeanTween.moveLocal(shopButton, new Vector3(-200f, 3.8147e-06f, 0), 1f)
            .setEaseOutElastic();


        LeanTween.moveLocal(duooButton, new Vector3(198, -1.9073e-06f, 0), .8f)
            .setEaseOutElastic();


    }

    private void TweanBallAndHoop()
    {
        LeanTween.moveLocal(ball, new Vector3(-200f,450,0), 5f)
         .setLoopClamp()
         .setEaseShake();

        LeanTween.moveLocal(hoop, new Vector3(500,-500,0), 5f)
         .setLoopClamp()
         .setEaseShake();
    }


    // twean out

    public void TweanTopPanelOut()
    {
        LeanTween.moveLocal(shopButton, new Vector3(-200f, -376, 0), .5f)
            .setEaseOutElastic();


        LeanTween.moveLocal(duooButton, new Vector3(198, -375.9999f, 0), .4f)
            .setEaseOutElastic();
             


    }

    public void TweanBottomPanelOut()
    {
        LeanTween.moveLocal(coinImage, new Vector3(-246.4342f, 261.0002f, 0), .4f)
            .setEaseOutElastic();


        LeanTween.moveLocal(scoreButton, new Vector3(248f, 261.0001f, 0), .4f)
            .setEaseOutElastic();

        LeanTween.moveLocal(settingsButton, new Vector3(408, 261.0001f, 0), .4f)
            .setEaseOutElastic();
             

    }

    public void FirstTweanInOut()
    {
        LeanTween.moveLocal(text, new Vector3(0, -1168f,0), .4f)
            .setEaseInOutQuint()
            .setOnComplete(() =>
            {
                LeanTween.moveLocal(playButton, new Vector3(0, -1156, 0), .4f)
                .setEaseInOutQuint();
                
            });

    }

}
