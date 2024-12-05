using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Runtime.CompilerServices;

public class CredtsPathfinding : MonoBehaviour
{
    [Header("Credits Prefabs")]
    public GameObject andreasPrefab;
    public GameObject christinaPrefab;
    public GameObject elisabethPrefab;
    public GameObject stevenPrefab;
    public GameObject iliaPrefab;
    public GameObject elijaPrefab;
    public GameObject janPrefab;
    public GameObject jensPrefab;
    public GameObject maxPrefab;

    [Space(10)]
    [Header("Spawn Points")]
    public GameObject[] spawnPoints;

    private IEnumerator Start()
    {
        while (true)
        {
            Instantiate(andreasPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(christinaPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(elisabethPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(stevenPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(iliaPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(elijaPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(janPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(jensPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(maxPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6); 

        }
    }
}
