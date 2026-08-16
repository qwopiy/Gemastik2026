using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] public AudioMixer audioMixer;

    [Header("Icons GameObjects")]
    [SerializeField] private GameObject BGMOn;
    [SerializeField] private GameObject BGMOff;
    [SerializeField] private GameObject SFXOn;
    [SerializeField] private GameObject SFXOff;
    [SerializeField] private GameObject AmbienceOn;
    [SerializeField] private GameObject AmbienceOff;




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
            BGMOn.SetActive(!BGMMuted);
            BGMOff.SetActive(BGMMuted);
        }
        else
        {
            audioMixer.SetFloat("BGMVolume", -20f);
            BGMOn.SetActive(!BGMMuted);
            BGMOff.SetActive(BGMMuted);
        }
    }

    public void ApplySFX()
    {
        if (SFXMuted)
        {
            audioMixer.SetFloat("SFXVolume", -80f);
            SFXOn.SetActive(!SFXMuted);
            SFXOff.SetActive(SFXMuted);
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", 0f);
            SFXOn.SetActive(!SFXMuted);
            SFXOff.SetActive(SFXMuted);
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
            AmbienceOn.SetActive(!AmbienceMuted);
            AmbienceOff.SetActive(AmbienceMuted);
        }
        else
        {
            audioMixer.SetFloat("AmbienceVolume", 0f);
            AmbienceOn.SetActive(!AmbienceMuted);
            AmbienceOff.SetActive(AmbienceMuted);
        }
    }
}