using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEditor;
public class MainMenu : MonoBehaviour
{
    //Resolution
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    private GameObject _resolution;

    private Resolution[] _resolutions;

    private void Awake()
    {
        LoadSettings();
    }

    void Start()
    {

        AudioSource.Play();

        /* 
         resolutionDropdown = resolution.GetComponent<Dropdown>();
         resolutions = Screen.resolutions;

         resolutionDropdown.ClearOptions();

         List<string> options = new List<string>();

         int currentResolutionIndex = 0;
         for (int i = 0; i < resolutions.Length; i++)
         {
             string option = resolutions[i].width + "x" + resolutions[i].height;
             options.Add(option);

             if (resolutions[i].width == Screen.currentResolution.width &&
                 resolutions[i].height == Screen.currentResolution.height)
             {
                 currentResolutionIndex = i;
             }
         }

         resolutionDropdown.AddOptions(options);
         resolutionDropdown.value = currentResolutionIndex;
         resolutionDropdown.RefreshShownValue();
        */

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
        /*
        _resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
        */
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
    public void SetResolution(int resolutionIndex)
    {
        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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

    public void LoadSettings()
    {

    }

    //Music

    public AudioSource AudioSource;

    private float musicVolume = 1.0f;

     void Update()
    {
        AudioSource.volume = musicVolume;
    }
    public void updateVolume (float volume)
    {
        musicVolume = volume;
    }

}






