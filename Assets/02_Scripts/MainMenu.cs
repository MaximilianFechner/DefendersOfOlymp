using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEditor;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{

    public AudioSource AudioSource;
    private float _musicVolume;
    [SerializeField] private AudioMixer _audioMixer;

    //Resolution
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    private GameObject _resolution;

    private Resolution[] _resolutions;

    

     void Start()
    {

        AudioSource.Play();

        _resolutionDropdown.ClearOptions();

        var options = new List<string>();
        _resolutions = Screen.resolutions;
        var currentResolutionIndex = 0;
        for (var i = 0; i < _resolutions.Length; i++)
        {
            var option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);
            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //Play
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    //Quit
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

    //Fullscreen
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionPreference", _resolutionDropdown.value);
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        _resolutionDropdown.value = PlayerPrefs.HasKey("ResolutionPreference") ? PlayerPrefs.GetInt("ResolutionPreference") : currentResolutionIndex;
    }

    //Music



    public void updateVolume(float volume)
    {
        _audioMixer.SetFloat("AUD_Master", volume);
        _musicVolume = volume;
    }

}






