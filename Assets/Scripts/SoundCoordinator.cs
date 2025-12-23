using UnityEngine;

public class SoundCoordinator : MonoBehaviour
{

    public static SoundCoordinator Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void ButtonClick()
    {
        AudioManager.Instance.PlayButtonSFX();
    }

    public void Scored()
    {
        AudioManager.Instance.PlayScoreSound();
    }

    public void MissedSound()
    {
        AudioManager.Instance.PlayMissSFX();
    }

    public void Swiped()
    {
        AudioManager.Instance.PlaySwipeSFX();
    }

}
