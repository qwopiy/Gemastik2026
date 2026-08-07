using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("BGM")]
    public AudioClip MainMenu;
    public AudioClip CFD;
    public AudioClip Kantin;
    public AudioClip EventCosplay;
    public AudioClip Kantor;

    [Header("SFX")]
    public AudioClip Drag;
    public AudioClip Drop;
    public AudioClip Trashbin;
    public AudioClip PaperFold;
    public AudioClip Footsteps;
    public AudioClip Stamps;
    public AudioClip Click;



    [Header("Ambience")]
    public AudioClip CrowdSound;


    public static AudioManager Instance;

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
        PlayMusic(MainMenu);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
            return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }
}