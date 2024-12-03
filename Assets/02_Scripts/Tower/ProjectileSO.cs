using System.Collections.Generic;
using UnityEngine;

public enum TowerType {
    NORMAL,
    SLOW,
    AOE,
    SUPPORT
}

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "Scriptable Objects/ProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    public GameObject prefab;

    [SerializeField] float defaultMovementSpeed;
    [SerializeField] float defaultDamage;
    [SerializeField] float defaultAoeRadius;
    [SerializeField] float defaultSlowValue;
    [SerializeField] TowerType defaultTowerType;
    [SerializeField] LayerMask defaultEnemyLayerMask;
    [SerializeField] Dictionary<GameObject, float> defaultEnemyBonusMalusTable;

    [HideInInspector] public float movementSpeed;
    [HideInInspector] public float damage;
    [HideInInspector] public float aoeRadius;
    [HideInInspector] public float slowValue;
    [HideInInspector] public TowerType towerType;
    [HideInInspector] public LayerMask enemyLayerMask;
    [HideInInspector] public Dictionary<GameObject, float> enemyBonusMalusTable;

    public void ResetData() {
        movementSpeed = defaultMovementSpeed;
        damage = defaultDamage;
        aoeRadius = defaultAoeRadius;
        slowValue = defaultSlowValue;
        towerType = defaultTowerType;
        enemyLayerMask = defaultEnemyLayerMask;
        enemyBonusMalusTable = defaultEnemyBonusMalusTable;
    }

}
