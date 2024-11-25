using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int RemainingLives { get; private set; }
    public int RemainingEnemies { get; private set; }
    public int EnemiesKilled { get; private set; }
    public int waveNumber { get; private set; }

    public Text playerLifeText;
    public Text enemiesKilledText;
    public Text waveNumberText;
    public Text remainingEnemiesText;


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
        RemainingLives = 5;
        EnemiesKilled = 0;
        waveNumber = 0;
        waveNumberText.text = $"Current wave: {waveNumber.ToString()}";
        playerLifeText.text = RemainingLives.ToString();
        enemiesKilledText.text = $"Hades' minions slayed: {EnemiesKilled.ToString()}";
    }

    public void NewGame()
    {
        EnemiesKilled = 0;
        RemainingLives = 5;
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


}
