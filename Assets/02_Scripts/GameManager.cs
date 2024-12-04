using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int RemainingLives { get; private set; }
    public int RemainingEnemies { get; private set; }
    public int EnemiesKilled { get; private set; }
    public int waveNumber { get; private set; }

    [Header("Game Design Values")]
    [Tooltip("The lifes the player starts with")]
    [Min(1)]
    [SerializeField]
    private int _playerStartLives = 5;

    [Tooltip("The time delay in seconds until the next enemy after the last spawn")]
    [Min(0)]
    [SerializeField]
    private float _waveSpawnDelay = 1f;

    [Tooltip("Enemies in the first wave + 1 enemy (when u choose 2, then 3 enemies will spawn)")]
    [Min(0)]
    public int firstWaveEnemies = 2;

    [Tooltip("How many cards allowed to draw between the waves")]
    [Min(1)]
    [SerializeField]
    private int _cardsToDraw = 1;

    [Space(10)]
    [Header("Wave Management")]
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    private bool isSpawning = false;

    [HideInInspector]
    public float thisWaveDuration;
    [HideInInspector]
    public float totalWaveDurations;

    private float _waveStartTime;
    private float _waveEndTime;

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

    // only for tests in Unity, implement the NewGame()-Method for build game
    private void Start()
    {
        ResetStats();
        UIManager.Instance.UpdateUITexts();
    }

    public void NewGame()
    {
        ResetStats();
        UIManager.Instance.UpdateUITexts();
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void TryAgain()
    {
        ResetStats();
        EndOfWave();
        UIManager.Instance.UpdateUITexts();
        UIManager.Instance.gameOverPanel.SetActive(false);
        UIManager.Instance.waveFinPanel.SetActive(false);
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void LoseLife(int damage)
    {
        RemainingLives -= damage;
        UIManager.Instance.playerLifeText.text = RemainingLives.ToString();

        if (RemainingLives == 0)
        {
            GameOver();
        }
    }

    public void AddEnemyKilled()
    {
        EnemiesKilled++;
        UIManager.Instance.enemiesKilledText.text = $"Hades' minions slayed: {EnemiesKilled.ToString()}";
    }

    public void AddRemainingEnemy(int enemies)
    {
        RemainingEnemies += enemies;
        UIManager.Instance.remainingEnemiesText.text = $"Remaining Enemies: {RemainingEnemies.ToString()}";
    }

    public void SubRemainingEnemy()
    {
        RemainingEnemies--;
        UIManager.Instance.remainingEnemiesText.text = $"Remaining Enemies: {RemainingEnemies.ToString()}";

        if (RemainingEnemies == 0)
        {
            EndOfWave();
        }
    }

    public void AddWaveCounter()
    {
        waveNumber++;
        UIManager.Instance.waveNumberText.text = $"Current wave: {waveNumber.ToString()}";
    }

    public void ResetStats()
    {
        RemainingEnemies = 0;
        EnemiesKilled = 0;
        RemainingLives = _playerStartLives;
        waveNumber = 0;
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        UIManager.Instance.gameOverPanel.SetActive(true);
        UIManager.Instance.ShowEndResults();
    }

    private void EndOfWave()
    {
        int remainingCardsToDraw = _cardsToDraw;

        _waveEndTime = Time.time;
        thisWaveDuration = _waveEndTime - _waveStartTime;
        totalWaveDurations += thisWaveDuration;

        UIManager.Instance.ShowWaveFinResults();

        UIManager.Instance.nextWaveButton.gameObject.SetActive(true);
        if (remainingCardsToDraw > 0)
        {
            UIManager.Instance.drawCardButton.gameObject.SetActive(true);
            remainingCardsToDraw--;
        }


    }

    // Enemy - Wave Spawn Methods
    public void StartNextWave()
    {
        if (!isSpawning)
        {
            AddRemainingEnemy(firstWaveEnemies + waveNumber + 1);
            AddWaveCounter();
            StartCoroutine(SpawnWave(waveNumber));

            _waveStartTime = Time.time;

            UIManager.Instance.nextWaveButton.gameObject.SetActive(false);
        }
    }

    private IEnumerator SpawnWave(int enemyCount)
    {
        isSpawning = true;

        for (int i = 0; i < firstWaveEnemies + enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_waveSpawnDelay);
        }

        isSpawning = false;
    }

    private void SpawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        int randomSpawnerIndex = Random.Range(0, spawnPoints.Length);
        GameObject enemyPrefab = enemyPrefabs[randomEnemyIndex];
        Instantiate(enemyPrefab, spawnPoints[randomSpawnerIndex].position, Quaternion.identity);
    }



}
