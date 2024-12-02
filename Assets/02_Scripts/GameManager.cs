using UnityEngine;
using UnityEngine.UI;

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

    [Space(10)]
    [Header("UI Elements")]
    public Text playerLifeText;
    public Text enemiesKilledText;
    public Text waveNumberText;
    public Text remainingEnemiesText;
    public Button nextWaveButton;
    public Button drawCardButton;

    private int cardsToDraw = 1;

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
        RemainingLives = _playerStartLives;
        EnemiesKilled = 0;
        waveNumber = 0;
        waveNumberText.text = $"Current wave: {waveNumber.ToString()}";
        playerLifeText.text = RemainingLives.ToString();
        enemiesKilledText.text = $"Hades' minions slayed: {EnemiesKilled.ToString()}";
    }

    public void NewGame()
    {
        EnemiesKilled = 0;
        RemainingLives = _playerStartLives;
        waveNumber = 0;
        waveNumberText.text = $"Current wave: {waveNumber.ToString()}";
        playerLifeText.text = RemainingLives.ToString();
        enemiesKilledText.text = $"Hades' minions slayed: {EnemiesKilled.ToString()}";
    }

    public void LoseLife(int damage)
    {
        RemainingLives -= damage;
        playerLifeText.text = RemainingLives.ToString();
    }

    public void AddEnemyKilled()
    {
        EnemiesKilled++;
        enemiesKilledText.text = $"Hades' minions slayed: {EnemiesKilled.ToString()}";
    }

    public void AddRemainingEnemy(int enemies)
    {
        RemainingEnemies += enemies;
        remainingEnemiesText.text = $"Remaining Enemies: {RemainingEnemies.ToString()}";
    }

    public void SubRemainingEnemy()
    {
        RemainingEnemies--;
        remainingEnemiesText.text = $"Remaining Enemies: {RemainingEnemies.ToString()}";

        if (RemainingEnemies == 0)
        {
            EndOfWave();
        }
    }

    public void AddWaveCounter()
    {
        waveNumber++;
        waveNumberText.text = $"Current wave: {waveNumber.ToString()}";
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
    }

    private void EndOfWave()
    {
        int remainingCardsToDraw = cardsToDraw;
        nextWaveButton.gameObject.SetActive(true);
        if (remainingCardsToDraw > 0)
        {
            drawCardButton.gameObject.SetActive(true);
            remainingCardsToDraw--;
        }
    }


}
