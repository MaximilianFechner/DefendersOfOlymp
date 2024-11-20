using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;
public class MainMenu : MonoBehaviour
{

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

    //Resolution
    public Dropdown _resolutionDropdown;
    public GameObject resolution;

    Resolution[] _resolutions;

    void Start()
    {
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

        /*
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
        */
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

}





