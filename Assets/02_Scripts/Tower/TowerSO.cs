using UnityEngine;

public enum TargetType {
    LOWEST_HEALTH,
    HIGHEST_HEALTH
}

[CreateAssetMenu(fileName = "new TowerSO", menuName = "Scriptable Objects/TowerSO")]
public class TowerSO : ScriptableObject
{

    public GameObject prefab;
    public ProjectileSO projectileSO;

    [SerializeField] float defaultAttackSpeed;
    [SerializeField] float defaultAttackRadius;
    [SerializeField] LayerMask defaultEnemyLayerMask;
    [SerializeField] TargetType defaultTargetType;

    [HideInInspector] public float attackSpeed;
    [HideInInspector] public float attackRadius;
    [HideInInspector] public LayerMask enemyLayerMask;
    [HideInInspector] public TargetType targetType;

    public void ResetData() {
        attackSpeed = defaultAttackSpeed;
        attackRadius = defaultAttackRadius;
        enemyLayerMask = defaultEnemyLayerMask;
        targetType = defaultTargetType;
    }

}
