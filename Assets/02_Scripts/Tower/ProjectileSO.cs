using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "Scriptable Objects/ProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    public GameObject prefab;

    [SerializeField] float defaultMovementSpeed;
    [SerializeField] float defaultDamage;
    [SerializeField] float defaultAoeRadius;
    [SerializeField] float defaultSlowValue;
    [SerializeField] Dictionary<GameObject, float> defaultEnemyBonusMalusTable;

    [HideInInspector] public float movementSpeed;
    [HideInInspector] public float damage;
    [HideInInspector] public float aoeRadius;
    [HideInInspector] public float slowValue;
    [HideInInspector] public Dictionary<GameObject, float> enemyBonusMalusTable;

    public void ResetData() {
        movementSpeed = defaultMovementSpeed;
        damage = defaultDamage;
        aoeRadius = defaultAoeRadius;
        slowValue = defaultSlowValue;
        enemyBonusMalusTable = defaultEnemyBonusMalusTable;
    }

}
