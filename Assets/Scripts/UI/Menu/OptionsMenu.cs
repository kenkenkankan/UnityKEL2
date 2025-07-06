using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : Menu
{
    // [SerializeField] private TMP_Dropdown resolutionDropdown;
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;

    [Serializable]
    private class Audio
    {
        public string Name;
        public TMP_Text audioValue;
        public Slider audioSlider;
    }

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private List<Audio> audios;

    private int calledIndex;

    private Resolution[] resolutions;

    // void Awake()
    // {   
    //     resolutions = Screen.resolutions;
    //     List<string> options = new();
    //     resolutionDropdown.ClearOptions();

    //     int currentResolutionIndex = 0;

    //     // Read pc resolution list
    //     foreach(var resolution in resolutions.
    //     Select((value, index) => new {value, index})) {
    //         string option = resolution.value.width + " x " + resolution.value.height;
    //         options.Add(option);

    //         if(resolution.value.width == Screen.currentResolution.width &&
    //         resolution.value.height == Screen.currentResolution.height) {
    //             currentResolutionIndex = resolution.index;
    //         }
    //     }

    //     if(!PlayerPrefs.HasKey("resolution")) {
    //         SetResolution(currentResolutionIndex);
    //     } else {
    //         currentResolutionIndex = PlayerPrefs.GetInt("resolution");
    //         SetResolution(currentResolutionIndex);
    //     }

    //     resolutionDropdown.AddOptions(options);
    //     resolutionDropdown.value = currentResolutionIndex;
    //     resolutionDropdown.RefreshShownValue();
    //     if(!PlayerPrefs.HasKey("fullscreen")) {
    //         SetFullscreen(true);
    //     } else {
    //         bool fullscreen = PlayerPrefs.GetInt("fullscreen") == 1;
    //         SetFullscreen(fullscreen);
    //     }

    //     gameObject.SetActive(false);
    // 

    void Start()
    {
        for (int i = 0; i < audios.Count; i++)
        {
            SetIndex(i);
            SetAudio(0);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", resolutionIndex);
    }
    /// <summary>
    /// Audio Sliders' use this method
    /// </summary>
    /// <param name="val"></param>
    public void SetAudio(float val)
    {
        if (val == 0)
            val = PlayerPrefs.GetFloat(audios[calledIndex].Name, 0);
        audioMixer.SetFloat(audios[calledIndex].Name, val);
        audios[calledIndex].audioValue.text = ((int)val + 80f).ToString();
        audios[calledIndex].audioSlider.value = val;
        PlayerPrefs.SetFloat(audios[calledIndex].Name, val);
    }

    public void SetIndex(int index)
    {
        calledIndex = index;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        FindAnyObjectByType<Toggle>().isOn = isFullscreen;
        PlayerPrefs.SetInt("fullscreen", isFullscreen == true ? 1 : 0);
    }

    public override void DeactivateMenu()
    {
        base.DeactivateMenu();
        mainMenu.ActivateMenu();
    }
}
