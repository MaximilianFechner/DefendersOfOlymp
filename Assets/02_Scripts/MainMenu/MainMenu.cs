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
using Unity.VisualScripting;


public class MainMenu : MonoBehaviour
{
    public InputField inputFPS;
    public Text selectedFPS;



    public AudioSource AudioSource;

    private float _musicVolume;
    [SerializeField] private AudioMixer _audioMixer;

    //Resolution
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    private GameObject _resolution;

    private Resolution[] _resolutions;

    [SerializeField] private GameObject _optionsMenu;


    //Framerate Limit

    public int targetFPS; //int.Parse(selectedFPS.text);



    void Start()
    {

        //Audio

        AudioSource.Play();

        _resolutionDropdown.ClearOptions();

        //Resolution Dropdown

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _optionsMenu.SetActive(true);
        }

        // Framerate Limit
        string input = selectedFPS.text;
        if (int.TryParse(input, out int fps))
        {
            targetFPS = fps;
        }
        else
        {
            targetFPS = 60; // Default FPS

        }

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
    }

    //Resolution Dropdown

    public void SetResolution(int resolutionIndex)
    {
        if (_resolutions == null || _resolutions.Length == 0)
        {

            return;
        }

        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //Play
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    //Interactive Manual

    public void PlayManual()
    {
        SceneManager.LoadSceneAsync(2);
    }

    //Return to Main Menu
    public void LeaveGame()
    {
        SceneManager.LoadSceneAsync(0);
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






