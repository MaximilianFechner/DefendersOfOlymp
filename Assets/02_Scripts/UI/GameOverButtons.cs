using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
    MainMenu menu;

    private void Start()
    {
        menu = FindAnyObjectByType<MainMenu>();
    }

    public void QuitBtn()
    {
        GameManager.Instance.CloseGame();
        AudioManager.Instance.PlayButtonSFX	();
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

    public void BackToMainMenuBtn()
    {
        menu.LeaveGame();

        //GameManager.Instance.BackToMainMenu();
        ////MainMenu.Instance.LeaveGame();
        //if (Time.timeScale != 1)
        //    Time.timeScale = 1;
        //SceneManager.LoadScene(0);
        //GameManager.Instance.DestroyManager();
        //UIManager.Instance.DestroyManager();
        //TooltipManager.Instance.DestroyManager();
        //AudioManager.Instance.PlayMainMenuMusic();
    }
}
