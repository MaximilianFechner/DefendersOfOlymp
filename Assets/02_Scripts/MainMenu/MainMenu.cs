using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public InputField inputFPS;
    public Text selectedFPS;
    public AudioSource audioSource;
    private float _musicVolume;
    [SerializeField] private AudioMixer _audioMixer;
    
    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    private GameObject _resolution;
    private Resolution[] _resolutions;

    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _startSubMenu;
    [SerializeField] private GameObject _BackgroundImage;
    [SerializeField] private GameObject _uiMainElements;
    
    //Framerate Limit
    public int targetFPS;

    private static MainMenu singleton;//int.Parse(selectedFPS.text);

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        } else if(singleton != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        //Audio
        audioSource.Play();
        
        #region Resolution Dropdown
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
        #endregion
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0;
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
            return;

        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //Play
    public void PlayGame()
    {
        _startSubMenu.SetActive	(false);
        _BackgroundImage.SetActive(false);
        _uiMainElements.SetActive(false);
        SceneManager.LoadScene(1);
    }

    //Interactive Manual
    public void PlayManual()
    {
        SceneManager.LoadScene(2);
    }

    //Return to Main Menu
    public void LeaveGame()
    {
        if (Time.timeScale != 1)
            Time.timeScale = 1;
        if(_pauseMenu) _pauseMenu.SetActive(false);
        SceneManager.LoadScene(0);
        if(!_BackgroundImage) _BackgroundImage.SetActive(true);
        if(!_uiMainElements) _uiMainElements.SetActive(true);
    }

    //Quit
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

    public void PauseGame()
    {
        if (Time.timeScale != 1)
            Time.timeScale = 1;
        _pauseMenu.SetActive(false);
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






