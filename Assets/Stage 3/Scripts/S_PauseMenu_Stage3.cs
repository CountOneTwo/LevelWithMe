using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class S_PauseMenu_Stage3 : MonoBehaviour
{
    public Slider sensSlider;
    public Text sensText;
    public Aiming_Stage3 aimingRef;

    public AudioMixer audioMixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider effectsSlider;
    public Toggle fullscreenToggle;
    public Dropdown resolutionsDropdown;

    public S_QualityButton[] qualityButtons = new S_QualityButton[3];

    Resolution[] resolutions;
    int currentResolutionIndex = 0;

    System.Globalization.CultureInfo culture;

    [Header("Sounds")]
    AudioSource soundSource;
    public AudioClip buttonClip;
    public AudioClip sliderClip;
    public AudioClip toggleMenuClip;

    [Header("Volume and Pitch")]
    public bool changeButtonVolume = false;
    [Range(0, 1)] public float buttonVolume = 1.0f;
    [Range(0, 1)] public float minButtonVolume = 0.7f;
    [Range(0, 1)] public float maxButtonVolume = 1.0f;
    public bool changeButtonPitch = false;
    [Range(-3, 3)] public float buttonPitch = 1.0f;
    [Range(-3, 3)] public float minButtonPitch = -1.0f;
    [Range(-3, 3)] public float maxButtonPitch = 2.0f;

    [Space]
    public bool changeSliderVolume = false;
    [Range(0, 1)] public float sliderVolume = 1.0f;
    [Range(0, 1)] public float minSliderVolume = 0.7f;
    [Range(0, 1)] public float maxSliderVolume = 1.0f;
    public bool changeSliderPitch = false;
    [Range(-3, 3)] public float sliderPitch = 1.0f;
    [Range(-3, 3)] public float minSliderPitch = -1.0f;
    [Range(-3, 3)] public float maxSliderPitch = 2.0f;

    [Space]
    public bool changeToggleVolume = false;
    [Range(0, 1)] public float toggleVolume = 1.0f;
    [Range(0, 1)] public float minToggleVolume = 0.7f;
    [Range(0, 1)] public float maxToggleVolume = 1.0f;
    public bool changeTogglePitch = false;
    [Range(-3, 3)] public float togglePitch = 1.0f;
    [Range(-3, 3)] public float minTogglePitch = -1.0f;
    [Range(-3, 3)] public float maxTogglePitch = 2.0f;

    void Start()
    {
        culture = new System.Globalization.CultureInfo("en-US");

        InitializeVolumeSliders();
        InitializeQualityButtons();
        InitializeFullscreenToggle();
        InitializeResolutionsDropdown();
        InitializeSensitivity();

        if (gameObject.GetComponent<AudioSource>() != null)
        {
            soundSource = gameObject.GetComponent<AudioSource>();
        }
    }

    public void PlayButtonSound()
    {
        if (soundSource != null)
        {
            if (changeButtonVolume)
            {
                soundSource.volume = Random.Range(minButtonVolume, maxButtonVolume);
            }
            else
            {
                soundSource.volume = buttonVolume;
            }

            if (changeButtonPitch)
            {
                soundSource.pitch = Random.Range(minButtonPitch, maxButtonPitch);
            }
            else
            {
                soundSource.pitch = buttonPitch;
            }

            soundSource.clip = buttonClip;
            soundSource.Play();
        }
    }

    public void PlayToggleSound()
    {
        if (soundSource != null)
        {
            if (changeToggleVolume)
            {
                soundSource.volume = Random.Range(minToggleVolume, maxToggleVolume);
            }
            else
            {
                soundSource.volume = toggleVolume;
            }

            if (changeTogglePitch)
            {
                soundSource.pitch = Random.Range(minTogglePitch, maxTogglePitch);
            }
            else
            {
                soundSource.pitch = togglePitch;
            }

            soundSource.clip = toggleMenuClip;
            soundSource.Play();
        }
    }

    public void PlaySliderSound()
    {
        if (soundSource != null)
        {
            if (!soundSource.isPlaying)
            {
                if (changeSliderVolume)
                {
                    soundSource.volume = Random.Range(minSliderVolume, maxSliderVolume);
                }
                else
                {
                    soundSource.volume = sliderVolume;
                }

                if (changeSliderPitch)
                {
                    soundSource.pitch = Random.Range(minSliderPitch, maxSliderPitch);
                }
                else
                {
                    soundSource.pitch = sliderPitch;
                }

                soundSource.clip = sliderClip;
                soundSource.Play();
            }
        }
    }

    void InitializeVolumeSliders()
    {
        if (audioMixer != null)
        {
            if (masterSlider != null)
            {
                masterSlider.value = GetMixerVolume("MasterVolume");
            }

            if (musicSlider != null)
            {
                musicSlider.value = GetMixerVolume("MusicVolume");
            }

            if (effectsSlider != null)
            {
                effectsSlider.value = GetMixerVolume("EffectsVolume");
            }
        }
    }

    float GetMixerVolume(string parameterName)
    {
        float value;
        bool result = audioMixer.GetFloat(parameterName, out value);

        if (result)
            return value;

        else
            return 0;
        
    }

    public void OnMasterVolumeChanged(float newValue)
    {
        audioMixer.SetFloat("MasterVolume", newValue);
    }

    public void OnMusicVolumeChanged(float newValue)
    {
        audioMixer.SetFloat("MusicVolume", newValue);
    }

    public void OnEffectsVolumeChanged(float newValue)
    {
        audioMixer.SetFloat("EffectsVolume", newValue);
    }

    public void OnQualityButtonPressed(S_QualityButton qButton)
    {
        QualitySettings.SetQualityLevel(qButton.qualityIndex);
        InitializeQualityButtons();
    }

    void InitializeQualityButtons()
    {
        foreach(S_QualityButton qb in qualityButtons)
        {
            if (qb != null)
            {
                qb.SetEnabled(!(qb.qualityIndex == QualitySettings.GetQualityLevel()));
            }
        }
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    void InitializeFullscreenToggle()
    {
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
        }
    }

    void InitializeResolutionsDropdown()
    {
        resolutions = Screen.resolutions;

        if (resolutionsDropdown != null)
        {
            resolutionsDropdown.ClearOptions();

            List<string> resolutionOptions = new List<string>();

            for (int i = (resolutions.Length-1); i >= 0; i--)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                resolutionOptions.Add(option);

                if (resolutions[i].height == Screen.currentResolution.height && resolutions[i].width == Screen.currentResolution.width)
                {
                    currentResolutionIndex = (resolutions.Length - 1) - i;
                }
            }

            resolutionsDropdown.AddOptions(resolutionOptions);
            resolutionsDropdown.value = currentResolutionIndex;
            resolutionsDropdown.RefreshShownValue();
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        currentResolutionIndex = (resolutions.Length - 1) - resolutionIndex;
        Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, Screen.fullScreen);
    }

    void InitializeSensitivity()
    {
        if (aimingRef != null)
        {
            if (sensSlider != null)
            {
                sensSlider.value = aimingRef.GetMouseSens();
            }

            if (sensText != null)
            {
                sensText.text = aimingRef.GetMouseSens().ToString("#.0", culture);

            }
        }
    }

    public void SetMouseSensitivity(float newValue)
    {
        aimingRef.SetMouseSens(newValue);
        sensText.text = aimingRef.GetMouseSens().ToString("#.0", culture);
    }
}
