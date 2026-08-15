using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] public AudioMixer audioMixer;

    [Header("Icons")]
    [SerializeField] public Image BGMIcon;
    [SerializeField] public Image SFXIcon;
    [SerializeField] public Image AmbienceIcon;

    [SerializeField] public Sprite soundOn;
    [SerializeField] public Sprite soundOff;



    public bool BGMMuted;
    public bool SFXMuted;
    public bool AmbienceMuted;

    public void Start()
    {
        // Load setting sebelumnya
        BGMMuted = PlayerPrefs.GetInt("BGMMuted", 0) == 1;
        SFXMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        AmbienceMuted = PlayerPrefs.GetInt("AmbienceMuted", 0) == 1;    

        ApplyBGM();
        ApplySFX();
        ApplyAmbience();
    }

    public void ToggleBGM()
    {
        BGMMuted = !BGMMuted;

        ApplyBGM();

        PlayerPrefs.SetInt("BGMMuted", BGMMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleSFX()
    {
        SFXMuted = !SFXMuted;

        ApplySFX();

        PlayerPrefs.SetInt("SFXMuted", SFXMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ApplyBGM()
    {
        if (BGMMuted)
        {
            audioMixer.SetFloat("BGMVolume", -80f);
            BGMIcon.sprite = soundOff;
        }
        else
        {
            audioMixer.SetFloat("BGMVolume", -20f);
            BGMIcon.sprite = soundOn;
        }
    }

    public void ApplySFX()
    {
        if (SFXMuted)
        {
            audioMixer.SetFloat("SFXVolume", -80f);
            SFXIcon.sprite = soundOff;
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", 0f);
            SFXIcon.sprite = soundOn;
        }
    }

    public void ToggleAmbience()
    {
        AmbienceMuted = !AmbienceMuted;
        ApplyAmbience();
        PlayerPrefs.SetInt("AmbienceMuted", AmbienceMuted ? 1 : 0);
        PlayerPrefs.Save();
    }   

    public void ApplyAmbience()
    {
        if (AmbienceMuted)
        {
            audioMixer.SetFloat("AmbienceVolume", -80f);
            AmbienceIcon.sprite = soundOff;
        }
        else
        {
            audioMixer.SetFloat("AmbienceVolume", 0f);
            AmbienceIcon.sprite = soundOn;
        }
    }
}