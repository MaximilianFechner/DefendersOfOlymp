using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    private bool isSpawning = false;

    [Header("Game Design Values")]
    [Tooltip("The time delay in seconds until the next enemy after the last spawn")]
    [Min(0)]
    public float spawnDelay = 1f;

    [Tooltip("Enemies in the first wave + 1 enemy (when u choose 2, then 3 enemies will spawn)")]
    [Min(0)]
    public int startEnemies = 2;

    [Space(10)]
    [Header("UI Elements")]
    public Button nextWaveButton;

    public void StartNextWave()
    {
        if (!isSpawning)
        {
            GameManager.Instance.AddRemainingEnemy(startEnemies + GameManager.Instance.waveNumber + 1);
            GameManager.Instance.AddWaveCounter();
            StartCoroutine(SpawnWave(GameManager.Instance.waveNumber));
            nextWaveButton.gameObject.SetActive(false);
        }
    }

    private IEnumerator SpawnWave(int enemyCount)
    {
        isSpawning = true;

        for (int i = 0; i < startEnemies + enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawning = false;
    }

    private void SpawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        int randomSpawnerIndex = Random.Range(0, spawnPoints.Length);
        GameObject enemyPrefab = enemyPrefabs[randomEnemyIndex];
        Instantiate(enemyPrefab, spawnPoints[randomSpawnerIndex].position, Quaternion.identity, transform);
    }
}
