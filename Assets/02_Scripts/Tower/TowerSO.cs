using UnityEngine;

public enum TargetType {
    lowestHealth,
    highestHealth
}

[CreateAssetMenu(fileName = "TowerSO", menuName = "Scriptable Objects/TowerSO")]
public class TowerSO : ScriptableObject
{

    public GameObject prefab;
    public ProjectileSO projectileSO;

    public float attackSpeed;
    public float attackRadius;
    public LayerMask enemyLayerMask;
    public TargetType targetType;

}
