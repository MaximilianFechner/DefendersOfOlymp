using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "Scriptable Objects/ProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    public GameObject prefab;

    public float movementSpeed;
    public float damage;
    public float aoeRadius;
    public float slowValue;
    public Dictionary<GameObject, float> enemyBonusMalusTable;

}
