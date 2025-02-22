using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsHandler : MonoBehaviour
{

    private MainMenu menu;

    private void Start()
    {
        menu = FindAnyObjectByType<MainMenu>();
    }

    public void BackToMainMenu()
    {
        menu.LeaveGame();
    }
}
