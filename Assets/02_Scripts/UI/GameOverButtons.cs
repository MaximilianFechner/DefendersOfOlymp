using UnityEditor;
using UnityEngine;

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
        MainMenu.Instance.LeaveGame();
    }
}
