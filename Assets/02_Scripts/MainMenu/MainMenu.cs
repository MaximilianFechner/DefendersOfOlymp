using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject _menuAudio;
    
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
            PauseGame();
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
        ButtonSFX	();
        _menuAudio.GetComponent	<AudioSource>().Stop();
        _startSubMenu.SetActive	(false);
        _BackgroundImage.SetActive(false);
        _uiMainElements.SetActive(false);
        SceneManager.LoadScene(1);
    }

    //Interactive Manual
    public void PlayManual()
    {
        ButtonSFX	();
        _menuAudio.GetComponent	<AudioSource>().Stop();
        _startSubMenu.SetActive	(false);
        _BackgroundImage.SetActive(false);
        _uiMainElements.SetActive(false);
        SceneManager.LoadScene(2);
    }

    //Return to Main Menu
    public void LeaveGame()
    {
        ButtonSFX	();
        if (Time.timeScale != 1)
            Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        _BackgroundImage.SetActive(true);
        _uiMainElements.SetActive(true);
        SceneManager.LoadScene(0);
        GameManager.Instance.DestroyManager	();
        UIManager.Instance.DestroyManager	();
        AudioManager.Instance.DestroyManager	();
        TooltipManager.Instance	.DestroyManager	();
    }

    //Quit
    public void QuitGame()
    {
        ButtonSFX	();
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

    public void PauseGame()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void ContinueGame()
    {
        if (Time.timeScale != 1)
            Time.timeScale = 1;
        _pauseMenu.SetActive(false);
    }

    //Fullscreen
    public void SetFullscreen(bool isFullscreen)
    {
        ButtonSFX	();
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

    private void ButtonSFX()
    {
        AudioManager.Instance.PlayButtonSFX	();
    }
}






