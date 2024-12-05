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
            Instantiate(andreasPrefab, spawnPoints[1].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(christinaPrefab, spawnPoints[1].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(elisabethPrefab, spawnPoints[1].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(stevenPrefab, spawnPoints[1].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(iliaPrefab, spawnPoints[1].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(elijaPrefab, spawnPoints[1].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(janPrefab, spawnPoints[1].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(jensPrefab, spawnPoints[1].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6);
            Instantiate(maxPrefab, spawnPoints[1].gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(6); 

        }
    }
}
