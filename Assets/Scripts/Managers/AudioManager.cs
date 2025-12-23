using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip swipeSFX;
    [SerializeField] private AudioClip netSound;
    [SerializeField] private AudioClip missSound;

    [Header("Volumes")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMenuMusic();
    }

    public void PlayMenuMusic()
    {
        musicSource.clip = menuMusic;
        if(!musicSource.isPlaying )
            musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // -----SFX-----

    public void PlayButtonSFX()
    {
        //sfxSource.clip=buttonClick;
        sfxSource.PlayOneShot(buttonClick, 1f);
    }

    public void PlaySwipeSFX()
    {
        sfxSource.PlayOneShot(swipeSFX, .8f);
    }

    public void PlayMissSFX()
    {
        sfxSource.PlayOneShot(missSound, .5f);
    }

    public void PlayScoreSound()
    {
        sfxSource.PlayOneShot(netSound,1f);
    }
}
