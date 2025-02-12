using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
//using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int RemainingLives { get; private set; }
    public int RemainingEnemies { get; private set; }
    public int TotalEnemiesKilled { get; private set; }
    public int WaveEnemiesKilled { get; private set; }
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

    [Tooltip("Enemies in the first wave + 1 enemy (when u choose 2, then 3 enemies will spawn in the first wave)")]
    [Min(0)]
    public int firstWaveEnemies = 2;

    [Tooltip("every X waves add an extra enemy (when 5, then every 5 waves you spawn a additional enemy")]
    [Min(0)]
    public int extraEnemiesPerXWaves = 10; // NEW PROGRESS SPAWN - Ab welcher Welle immer ein zusätzlicher Gegner spawnt

    [Tooltip("How many cards allowed to draw between the waves")]
    [Min(1)]
    private int _cardsToDraw = 1;

    [Tooltip("Chance to crit: 2x damage")]
    [Min(0)]
    public float critChance = 5;

    [Space(10)]
    [Header("ONLY DISPLAYED FOR TESTING - DONT CHANGE")]
    public int nextWaveEnemies = 0; // NEW PROGRESS SPAWN - Gegner in der nächsten Welle
    public int plusEnemies = 0; // NEW PROGRESS SPAWN - Wieviele Gegner auf die Startgegner hinzugerechnet werden für die nächste Welle
    public int plusMultiplikator = 1; // NEW PROGRESS SPAWN - Modifikator für die plusEnemies Variable

    [Space(10)]
    public int zeusTower = 0;
    public int poseidonTower = 0;
    public int heraTower = 0;
    public int hephaistosTower = 0;

    [Space(10)]
    [Header("Wave Management")]
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    private bool isSpawning = false;

    public bool isInWave = false;
    
    [HideInInspector] public int score = 0;
    [HideInInspector] public int highscore = 0;

    [HideInInspector] public int cerberusKills = 0;
    [HideInInspector] public int cyclopKills = 0;
    [HideInInspector] public int centaurKills = 0;

    [HideInInspector] public int enemyScore = 0;
    [HideInInspector] public int waveScore = 0;
    [HideInInspector] public int healthScore = 0;

    [HideInInspector] public bool showDamageNumbers = true; //default activated, change for show damageNumbers or to disable them
    [HideInInspector] public bool showTooltips = true; //default activated, change for show Tooltips of Enemies and Tower or to disable them

    public ZeusBolt zeusBolt;
    public PoseidonWave poseidonWave;
    public HeraStun heraStun;
    public HephaistosQuake hephQuake;
    public Hermes hermes;
    public GameOverManager gameOverManager;

    public bool isCardDrawable = false;
    public bool isGameOver = false;
    public bool highscoreReached = false;

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
        AudioManager.Instance.PlayLevelBackgroundMusic();
        AudioManager.Instance.PlayLevelAmbienteSFX();

        //highscore = PlayerPrefs.GetInt("highscore", 0);
        //if (highscore == 0) return;

        LoadHighscore();
        UIManager.Instance.highscore.text = highscore.ToString();

        isCardDrawable = true;
        isGameOver = false;
        isInWave = false;

        gameOverManager.StopGameOverEffects();

        UIManager.Instance.nextWaveEnemiesText.text = $"<b><color=#8E0000>{PredictNextWaveEnemies()}</color></b> expected minions of\nHades in the next wave!";
        hermes.ShowHermes();
    }

    public void NewGame()
    {
        ResetStats();
        UIManager.Instance.UpdateUITexts();
        AudioManager.Instance.PlayLevelBackgroundMusic();
        AudioManager.Instance.PlayLevelAmbienteSFX();

        //highscore = PlayerPrefs.GetInt("highscore", 0);
        //if (highscore == 0) return;

        LoadHighscore();
        UIManager.Instance.highscore.text = highscore.ToString();

        isCardDrawable = true;
        isGameOver = false;
        isInWave = false;

        gameOverManager.StopGameOverEffects();

        UIManager.Instance.nextWaveEnemiesText.text = $"<b><color=#8E0000>{PredictNextWaveEnemies()}</color></b> expected minions of\nHades in the next wave!";
        hermes.ShowHermes();
    }

    public void CloseGame()
    {
        if (score > highscore)
        {
            highscore = score;
            SaveHighscore();
        }

        Application.Quit();
    }

    public void TryAgain()
    {
        FindFirstObjectByType<CardManager>().ResetGrid();

        ResetStats();
        EndOfWave();

        BloodManager bloodPools = FindFirstObjectByType<BloodManager>();
        if (bloodPools != null)
        {
            bloodPools.ClearBloodPools();
        }

        CorpseManager corpses = FindFirstObjectByType<CorpseManager>();
        if (corpses != null)
        {
            corpses.ClearCorpses();
        }

        UIManager.Instance.InitializeLives(_playerStartLives);
        UIManager.Instance.UpdateUITexts();
        UIManager.Instance.HideHighscoreVisual();
        //UIManager.Instance.nextWaveEnemiesText.text = $"<b><color=#8E0000>{PredictNextWaveEnemies()}</color></b> expected minions of\nHades in the next wave!";
        UIManager.Instance.gameOverPanel.SetActive(false);

        if (zeusBolt != null) zeusBolt.ResetCooldown();
        if (poseidonWave != null) poseidonWave.ResetCooldown();
        if (heraStun != null) heraStun.ResetCooldown();
        if (hephQuake != null) hephQuake.ResetCooldown();

        isCardDrawable = true;
        isGameOver = false;
        isInWave = false;

        gameOverManager.StopGameOverEffects();
        SceneManager.LoadScene("Level1");

        UIParticlesystem part = FindFirstObjectByType<UIParticlesystem>();
        if (part != null)
        {
            part.ResetPosParticleSystem();
        }
    }

    public void LoseLife(int damage)
    {
        if (!isGameOver)
        {
            RemainingLives -= damage;
            healthScore--;

            score--;
            if (score > highscore)
            {
                highscore = score;
                SaveHighscore();

                //PlayerPrefs.SetInt("highscore", highscore);
                //PlayerPrefs.Save();
            }

            UIManager.Instance.UpdateLives(RemainingLives);
            UIManager.Instance.UpdateScoreCalculating();
            AudioManager.Instance.PlayLostLifeSFX();

            if (RemainingLives == 0)
            {
                GameOver();
            }
        }
    }

    public void AddEnemyKilled()
    {
        enemyScore++;
        score++;

        if (score > highscore)
        {
            if (!highscoreReached)
            {
                UIManager.Instance.ShowHighscoreVisual();
                AudioManager.Instance.PlayHighscoreSFX();
                highscoreReached = true;
            }

            highscore = score;
            SaveHighscore();

            //PlayerPrefs.SetInt("highscore", highscore);
            //PlayerPrefs.Save();
        }

        TotalEnemiesKilled++;
        WaveEnemiesKilled++;

        UIManager.Instance.UpdateKilledEnemies();
        UIManager.Instance.UpdateScoreCalculating();
    }

    public void AddRemainingEnemy(int enemies)
    {
        RemainingEnemies += enemies;
        UIManager.Instance.remainingEnemiesText.text = $"{RemainingEnemies.ToString()}";
    }

    public void SubRemainingEnemy()
    {
        RemainingEnemies--;
        UIManager.Instance.remainingEnemiesText.text = $"{RemainingEnemies.ToString()}";

        if (RemainingEnemies == 0 && RemainingLives > 0)
        {
            EndOfWave();
        }
    }

    public void AddWaveCounter()
    {
        waveNumber++;
        UIManager.Instance.waveNumberText.text = $"{waveNumber.ToString()}";
    }

    public void ResetStats()
    {
        RemainingEnemies = 0;
        TotalEnemiesKilled = 0;
        RemainingLives = _playerStartLives;
        waveNumber = 0;
        score = 0;
        cerberusKills = 0;
        cyclopKills = 0;
        centaurKills = 0;
        enemyScore = 0;
        waveScore = 0;
        healthScore = 0;
        nextWaveEnemies = 0;
        plusEnemies = 0;
        plusMultiplikator = 1;

        highscoreReached = false;
        isGameOver = false;
        isInWave = false;

        zeusTower = 0;
        poseidonTower = 0;
        heraTower = 0;
        hephaistosTower = 0;
}

    private void GameOver()
    {
        if (score > highscore)
        {
            highscore = score;
            SaveHighscore();
        }

        isGameOver  = true;
        //Time.timeScale = 0;
        AudioManager.Instance.PlayGameOverSFX();
        gameOverManager.TriggerGameOver();
        //UIManager.Instance.gameOverPanel.SetActive(true);
        //UIManager.Instance.ShowEndResults();
    }

    private void EndOfWave()
    {
        zeusBolt.CancelZeusSkill();
        poseidonWave.CancelPoseidonSkill();
        heraStun.CancelHeraSkill();

        isCardDrawable = true;

        int remainingCardsToDraw = _cardsToDraw;

        waveScore += waveNumber;
        score += waveNumber;
        if (score > highscore)
        {
            if (!highscoreReached)
            {
                UIManager.Instance.ShowHighscoreVisual();
                AudioManager.Instance.PlayHighscoreSFX();
                highscoreReached = true;
            }

            highscore = score;
            SaveHighscore();

            //PlayerPrefs.SetInt("highscore", highscore);
            //PlayerPrefs.Save();
        }

        UIManager.Instance.UpdateScoreCalculating();
        UIManager.Instance.nextWaveEnemiesText.text = $"<b><color=#8E0000>{PredictNextWaveEnemies()}</color></b> expected minions of\nHades in the next wave!";
        AudioManager.Instance.PlayWaveEndMusic();
        hermes.ShowHermes();

        if (remainingCardsToDraw > 0)
        {
            UIManager.Instance.drawCardButton.gameObject.SetActive(true);
            remainingCardsToDraw--;
        }

        isInWave = false;
    }

    // Enemy Wave Spawn Methods ++
    public void StartNextWave()
    {
        if (!isSpawning)
        {
            AddWaveCounter();
            CalculateNextWaveEnemies();
            AddRemainingEnemy(nextWaveEnemies);
            StartCoroutine(SpawnWave(nextWaveEnemies));

            WaveEnemiesKilled = 0;

            UIManager.Instance.nextWaveButton.gameObject.SetActive(false);
            hermes.HideHermes();

            isInWave = true;
        }
    }

    private IEnumerator SpawnWave(int enemyCount)
    {
        isSpawning = true;

        for (int i = 0; i < enemyCount; i++)
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
    // Enemy Wave Spawn Methods --

    public int ReturnLives()
    {
        return _playerStartLives;
    }

    public void ToggleDamageNumbers()
    {
        showDamageNumbers = !showDamageNumbers;
    }

    public void ToggleTooltips()
    {
        showTooltips = !showTooltips;

#pragma warning disable CS0618 // Typ oder Element ist veraltet
        GridCells[] allCells = FindObjectsOfType<GridCells>();
#pragma warning restore CS0618 // Typ oder Element ist veraltet

        foreach (GridCells cell in allCells)
        {
            if (cell.towerLevelText != null)
            {
                cell.towerLevelText.gameObject.SetActive(showTooltips && cell.towerLevel > 0);
                cell.UpdateTowerLevelText();
            }
        }
    }

    public void CalculateNextWaveEnemies()
    {
        if (waveNumber % extraEnemiesPerXWaves == 0) plusMultiplikator++; // Wenn eine gewisse Welle erreicht wurde, dann erhöhe den Multiplilkator um 1

        if (waveNumber == 1)
        {
            nextWaveEnemies = firstWaveEnemies;
        }
        else
        {
            plusEnemies += plusMultiplikator; // 0 += 1 = 1 StartValues
            nextWaveEnemies = firstWaveEnemies + plusEnemies;
        }

        //Debug.Log($"CalculateNextWaveEnemies\n" +
        //    $"Wave: {waveNumber}\n" +
        //    $"WaveEnemies: {nextWaveEnemies}\n" +
        //    $"plusEnemies: {plusEnemies} \n" +
        //    $"plusMultiplikator: {plusMultiplikator}");

    }

    public int PredictNextWaveEnemies()
    {
        int predictedPlusMultiplikator = plusMultiplikator;
        int predictedPlusEnemies = plusEnemies;

        if ((waveNumber + 1) % extraEnemiesPerXWaves == 0)
            predictedPlusMultiplikator++;

        if (waveNumber + 1 == 1)
        {
            return firstWaveEnemies;
        }
        else
        {
            predictedPlusEnemies += predictedPlusMultiplikator;
            return firstWaveEnemies + predictedPlusEnemies;
        }
    }

    public void SaveHighscore()
    {
        string path = Application.persistentDataPath + "/highscore.txt"; // .txt-Erweiterung für Textdatei
        try
        {
            File.WriteAllText(path, highscore.ToString()); // Highscore als String speichern
            Debug.Log("Highscore gespeichert: " + highscore);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Fehler beim Speichern: {e.Message}");
        }
    }

    public void LoadHighscore()
    {
        string path = Application.persistentDataPath + "/highscore.txt";

        if (File.Exists(path))
        {
            try
            {
                string highscoreString = File.ReadAllText(path); // Highscore als String lesen

                if (int.TryParse(highscoreString, out int loadedHighscore)) // Versuchen, in Integer zu konvertieren
                {
                    highscore = loadedHighscore;
                    Debug.Log($"Highscore geladen: {highscore}");
                }
                else
                {
                    Debug.LogError($"Fehler: Highscore-Datei beschädigt! Inhalt: {highscoreString}");
                    highscore = 0; // Standardwert, falls Konvertierung fehlschlägt
                    SaveHighscore(); // Datei neu erstellen mit default wert
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Fehler beim Laden: {e.Message}");
                highscore = 0; // Standardwert bei Fehler
                SaveHighscore(); // Datei neu erstellen mit default wert
            }
        }
        else
        {
            Debug.LogWarning($"Keine Highscore-Datei gefunden. Erstelle neue mit Highscore 0.");
            highscore = 0; // Standardwert, wenn Datei nicht existiert.
            SaveHighscore(); // Datei neu erstellen mit default wert
        }
    }
    
    public void DestroyManager()
    {
        var Manager = this.GameObject();
        GameObject.Destroy	(Manager);
    }
}
