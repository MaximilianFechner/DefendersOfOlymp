using UnityEngine;

public class RandomDecorationSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] decorationPrefabs;

    void Start()
    {
        SpawnDecorations();
    }

    private void SpawnDecorations()
    {
        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (decorationPrefabs.Length == 0) return;

            int randomIndex = Random.Range(0, decorationPrefabs.Length);
            GameObject randomPrefab = decorationPrefabs[randomIndex];

            Instantiate(randomPrefab, spawnPoint.transform.position, Quaternion.identity, spawnPoint.transform);
        }
    }
}
