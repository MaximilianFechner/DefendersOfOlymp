using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{

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
        GameManager.Instance.BackToMainMenu();
        //MainMenu.Instance.LeaveGame();
        if (Time.timeScale != 1)
            Time.timeScale = 1;
        SceneManager.LoadScene(0);
        GameManager.Instance.DestroyManager();
        UIManager.Instance.DestroyManager();
        TooltipManager.Instance.DestroyManager();
        AudioManager.Instance.PlayMainMenuMusic();
    }
}
