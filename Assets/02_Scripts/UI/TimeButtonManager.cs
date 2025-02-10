using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeButtonManager : MonoBehaviour
{
    private TimeButton activeButton;
    public TimeButton defaultButton;

    private void Start()
    {
        if (defaultButton != null)
        {
            SetActiveButton(defaultButton);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (defaultButton != null)
        {
            SetActiveButton(defaultButton);
        }
    }

    public void SetActiveButton(TimeButton button)
    {
        if (activeButton != null)
        {
            activeButton.SetPressedState(false);
        }

        activeButton = button;
        activeButton.SetPressedState(true);

        HandleButtonAction(button);
    }

    private void HandleButtonAction(TimeButton button)
    {
        if (button.name == "BTNTimeX0")
        {
            Time.timeScale = 0f;
        }
        else if (button.name == "BTNTimeX1")
        {
            Time.timeScale = 1f;
        }
        else if (button.name == "BTNTimeX2")
        {
            Time.timeScale = 2f;
        }
    }
}
