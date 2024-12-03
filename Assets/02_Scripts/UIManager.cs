using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Space(10)]
    [Header("UI Elements")]
    public Text playerLifeText;
    public Text enemiesKilledText;
    public Text waveNumberText;
    public Text remainingEnemiesText;
    public Button nextWaveButton;
    public Button drawCardButton;
    public GameObject gameOverPanel;
    public Text endWaveCounter;
    public Text endEnemiesKilled;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateUITexts()
    {
        waveNumberText.text = $"Current wave: {GameManager.Instance.waveNumber.ToString()}";
        playerLifeText.text = GameManager.Instance.RemainingLives.ToString();
        enemiesKilledText.text = $"Hades' minions slayed: {GameManager.Instance.EnemiesKilled.ToString()}";
        remainingEnemiesText.text = $"Remaining Enemies: {GameManager.Instance.RemainingEnemies.ToString()}";
    }

    public void ShowEndResults()
    {
        endWaveCounter.text = $"Waves finished: {(GameManager.Instance.waveNumber - 1).ToString()}";
        endEnemiesKilled.text = $"Total enemies killed: {GameManager.Instance.EnemiesKilled.ToString()}";
    }
}
