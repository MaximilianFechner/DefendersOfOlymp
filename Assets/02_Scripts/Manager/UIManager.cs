using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Space(10)]
    [Header("UI Elements: Ingame")]
    public Text enemiesKilledText;
    public Text waveNumberText;
    public Text remainingEnemiesText;
    public Button nextWaveButton;
    public Button drawCardButton;
    public Transform PlayerLife;
    public GameObject heartPrefab;

    private List<GameObject> hearts = new List<GameObject>();

    [Space(10)]
    [Header("UI Elements: Active Skills")]
    public Text zeusSkillCooldown;
    public Text poseidonSkillCooldown;
    public Text hephaistosSkillCooldown;
    public Text heraSkillCooldown;

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
    public Text firstWaveEnemiesText;

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

    private void Start()
    {
        firstWaveEnemiesText.text = $"Enemies in the first wave: {GameManager.Instance.firstWaveEnemies + (GameManager.Instance.waveNumber + 1) + GameManager.Instance.addExtraEnemiesEveryWave}";
        prepareFirstWavePanel.SetActive(true);
        InitializeLives(GameManager.Instance.ReturnLives());
    }
    public void UpdateUITexts()
    {
        waveNumberText.text = $"{GameManager.Instance.waveNumber.ToString()}";
        enemiesKilledText.text = $"{GameManager.Instance.TotalEnemiesKilled.ToString()}";
        remainingEnemiesText.text = $"Remaining Enemies: {GameManager.Instance.RemainingEnemies.ToString()}";
    }

    public void ShowEndResults()
    {
        endWaveCounter.text = $"Waves finished: {(GameManager.Instance.waveNumber - 1).ToString()}";
        endEnemiesKilled.text = $"Total enemies killed: {GameManager.Instance.TotalEnemiesKilled.ToString()}";
    }

    public void ShowWaveResults()
    {
        waveFinishedText.text = $"Wave {GameManager.Instance.waveNumber.ToString()} finished!";
        nextWaveEnemiesText.text = $"Enemies next wave: {GameManager.Instance.firstWaveEnemies + (GameManager.Instance.waveNumber + 1) + GameManager.Instance.addExtraEnemiesEveryWave}";
        waveEnemiesKilledText.text = $"Enemies killed this wave: {GameManager.Instance.WaveEnemiesKilled}";
        waveDurationText.text = $"Time needed for this wave:\n{GameManager.Instance.thisWaveDuration:F1} seconds";
        waveFinPanel.SetActive(true);
    }

    // Player Life Methods ++
    public void InitializeLives(int lives)
    {
        ClearHearts();
        for (int i = 0; i < lives; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, PlayerLife);
            hearts.Add(newHeart);
        }
    }
    public void UpdateLives(int currentLives)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < currentLives);
        }
    }

    private void ClearHearts()
    {
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();
    }
    // Player Life Methods --
}
