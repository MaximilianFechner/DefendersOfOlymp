using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsHandler : MonoBehaviour
{

    private MainMenu menu;
    public GameObject creditsUI;

    private void Start()
    {
        menu = FindAnyObjectByType<MainMenu>();
    }

    public void BackToMainMenu()
    {
        creditsUI.SetActive(false);
        menu.LeaveGame();
    }
}
