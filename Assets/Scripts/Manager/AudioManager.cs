using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource BGMSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource AmbienceSource;


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
    public AudioClip Footsteps;
    public AudioClip Stamp;
    public AudioClip Click;
    public AudioClip DrawerOpen;
    public AudioClip DrawerClose;
    public AudioClip PageFlip;
    public AudioClip MagnifierSound;


    [Header("Ambience")]
    public AudioClip CrowdSound;


    public static AudioManager Instance;
    public event Action<AudioClip> PlaySoundEffect;

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
        PlaySoundEffect += PlaySFX;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TriggerClick();
        }
    }

    private void OnDisable()
    {
        PlaySoundEffect -= PlaySFX;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
            return;

        if (BGMSource.clip == clip && BGMSource.isPlaying)
            return;

        BGMSource.Stop();
        BGMSource.clip = clip;
        BGMSource.loop = true;
        BGMSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        SFXSource.PlayOneShot(clip);
    }

    public void PlayAmbience(AudioClip clip)
    {
        if (clip == null)
            return;
        if (AmbienceSource.clip == clip && AmbienceSource.isPlaying)
            return;
        AmbienceSource.Stop();
        AmbienceSource.clip = clip;
        AmbienceSource.loop = true;
        AmbienceSource.Play();
    }
    public void TriggerPlaySoundEffect(AudioClip clip)
    {
        PlaySoundEffect?.Invoke(clip);
    }

    public void TriggerClick()
    {
        TriggerPlaySoundEffect(Click);
    }
    public void TriggerDrag()
    {
        TriggerPlaySoundEffect(Drag);
    }
    public void TriggerDrop()
    {
        TriggerPlaySoundEffect(Drop);
    }
    public void TriggerTrashbin()
    {
        TriggerPlaySoundEffect(Trashbin);
    }
    public void TriggerFootsteps()
    {
        TriggerPlaySoundEffect(Footsteps);
    }
    public void TriggerStamp()
    {
        TriggerPlaySoundEffect(Stamp);
    }
    public void TriggerDrawerOpen()
    {
        TriggerPlaySoundEffect(DrawerOpen);
    }
    public void TriggerDrawerClose()
    {
        TriggerPlaySoundEffect(DrawerClose);
    }
    public void TriggerPageFlip()
    {
        TriggerPlaySoundEffect(PageFlip);
    }
    public void TriggerMagnifierSound()
    {
        TriggerPlaySoundEffect(MagnifierSound);
    }
}