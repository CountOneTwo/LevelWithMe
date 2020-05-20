﻿using System.Collections;
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

    void Start()
    {
        culture = new System.Globalization.CultureInfo("en-US");
        InitializeVolumeSliders();
        InitializeQualityButtons();
        InitializeFullscreenToggle();
        InitializeResolutionsDropdown();
        InitializeSensitivity();
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
