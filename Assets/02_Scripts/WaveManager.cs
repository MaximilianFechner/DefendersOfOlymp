using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public float spawnDelay = 1f;
    public int startEnemies = 2;
    private bool isSpawning = false;

    public void StartNextWave()
    {
        if (!isSpawning)
        {
            GameManager.Instance.AddRemainingEnemy(startEnemies + GameManager.Instance.waveNumber + 1);
            GameManager.Instance.AddWaveCounter();
            StartCoroutine(SpawnWave(GameManager.Instance.waveNumber));
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
        Instantiate(enemyPrefab, spawnPoints[randomSpawnerIndex].position, Quaternion.identity);
    }
}
