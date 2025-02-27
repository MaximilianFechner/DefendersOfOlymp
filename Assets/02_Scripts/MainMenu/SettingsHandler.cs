using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    public Toggle damageNumbersToggle;
    public Toggle tooltipsToggle;

    private void Awake()
    {
            // Lade gespeicherte Werte direkt in Awake
            bool showDamageNumbers = PlayerPrefs.GetInt("ShowDamageNumbers", 1) == 1;
            bool showTooltips = PlayerPrefs.GetInt("ShowTooltips", 1) == 1;

            damageNumbersToggle.isOn = showDamageNumbers;
            tooltipsToggle.isOn = showTooltips;
    }
    

    public void OnToggleDamageNumbers(bool value)
    {
        PlayerPrefs.SetInt("ShowDamageNumbers", value ? 1 : 0);
        PlayerPrefs.Save();

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            GameManager gameManager = FindAnyObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.ToggleDamageNumbers();
            }
        }
    }

    public void OnToggleTooltips(bool value)
    {
        PlayerPrefs.SetInt("ShowTooltips", value ? 1 : 0);
        PlayerPrefs.Save();

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            GameManager gameManager = FindAnyObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.ToggleTooltips();
            }
        }
    }
}
