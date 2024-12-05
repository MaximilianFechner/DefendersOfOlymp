using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Space(10)]
    [Header("UI Elements: Ingame")]
    public Text playerLifeText;
    public Text enemiesKilledText;
    public Text waveNumberText;
    public Text remainingEnemiesText;
    public Button nextWaveButton;
    public Button drawCardButton;

    [Space(10)]
    [Header("UI Elements: Active Skills")]
    public Text zeusSkillCooldown;

    [Space(10)]
    [Header("UI Elements: Game Over Panel")]
    public GameObject gameOverPanel;
    public Text endWaveCounter;
    public Text endEnemiesKilled;

    [Space(10)]
    [Header("UI Elements: Wave Finish Panel")]
    public GameObject waveFinPanel;
    public Text waveFinishedText;
    public Text nextWaveEnemiesText;
    public Text waveEnemiesKilledText;
    public Text waveDurationText;

    [Space(10)]
    [Header("UI Elements: First Wave Panel")]
    public GameObject prepareFirstWavePanel;

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
        waveNumberText.text = $"{GameManager.Instance.waveNumber.ToString()}";
        playerLifeText.text = GameManager.Instance.RemainingLives.ToString();
        enemiesKilledText.text = $"{GameManager.Instance.EnemiesKilled.ToString()}";
        remainingEnemiesText.text = $"Remaining Enemies: {GameManager.Instance.RemainingEnemies.ToString()}";
    }

    public void ShowEndResults()
    {
        endWaveCounter.text = $"Waves finished: {(GameManager.Instance.waveNumber - 1).ToString()}";
        endEnemiesKilled.text = $"Total enemies killed: {GameManager.Instance.EnemiesKilled.ToString()}";
    }

    public void ShowWaveResults()
    {
        waveFinishedText.text = $"Wave {GameManager.Instance.waveNumber.ToString()} finished!";
        nextWaveEnemiesText.text = $"Enemies next wave: {GameManager.Instance.firstWaveEnemies + GameManager.Instance.waveNumber + 1}";
        waveEnemiesKilledText.text = $"Enemies killed this wave: {GameManager.Instance.EnemiesKilled}";
        waveDurationText.text = $"Time needed for this wave:\n{GameManager.Instance.thisWaveDuration:F1} seconds";
        waveFinPanel.SetActive(true);
    }

}
