using System.Collections.Generic;
using UnityEngine;

public enum TargetType {
    LOWEST_HEALTH,
    HIGHEST_HEALTH
}

public enum TowerType {
    NORMAL,
    SLOW,
    AOE,
    SUPPORT
}

public class BaseTower : MonoBehaviour
{

    //TOWER
    [Header("Game Design Values: Tower")]
    [Tooltip("The base attack speed for the tower")]
    [SerializeField] public float attackSpeed;

    [Tooltip("The base attack radius for the tower")]
    [Min(1)]
    [SerializeField] public float attackRadius;


    [Tooltip("The attack pattern/target type for the tower")]
    [SerializeField] public TargetType targetType;

    [Space(5)]

    //PROJECTILE

    [Header("Game Design Values: Projectile")]
    [Tooltip("The base attack speed for the projectile")]
    [Min(1)]
    [SerializeField] public float movementSpeed;

    [Tooltip("The base attack damage for the projectile")]
    [Min(1)]
    [SerializeField] public float damage;

    [Tooltip("The base aoe radius for the projectile")]
    [SerializeField] public float aoeRadius;

    [Tooltip("The base slow Value deducted from the base speed of the enemy")]
    [SerializeField] public float slowValue;

    [Tooltip("The base time in seconds for the slow effect")]
    [SerializeField] public float timeSlowed;

    [Tooltip("The max value for the damage jump counter")]
    [Min(0)]
    [SerializeField] public int maxDamageJump;

    [Space(10)]

    //TOWER
    [Header("Technical Values: Tower")]
    [SerializeField] public LayerMask enemyLayerMask;
    [SerializeField] public float timer;
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public GameObject targetEnemy;    
    [SerializeField] public GameObject rangeVisual;
    [SerializeField] public GameObject towerMenu;
    [SerializeField] public Animator animator;
    [SerializeField] public int towerLevel = 1;

    //PROJECTILE
    [Space(5)]
    [Header("Technical Values: Projectile")]
    [SerializeField] public Dictionary<GameObject, float> enemyBonusMalusTable;
    public GameObject projectilePrefab;
    [SerializeField] public TowerType towerType  { get; set; }


    private void Awake() {
        animator = GetComponent<Animator>();

        rangeVisual.transform.localScale = new Vector3(attackRadius * 2, attackRadius * 2, 1);

        timer = 0;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public GameObject GetTargetEnemy() {
        Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayerMask);
        targetEnemy = null;
        if (enemiesColliders.Length > 0) {
            EnemyManager targetEnemyManager = enemiesColliders[0].GetComponent<EnemyManager>();

            switch (targetType) {
                case TargetType.LOWEST_HEALTH:
                    foreach (Collider2D collider2d in enemiesColliders) {
                        EnemyManager currentEnemy = collider2d.GetComponent<EnemyManager>();
                        if (currentEnemy.GetCurrentHP() < targetEnemyManager.GetCurrentHP()) {
                            targetEnemyManager = currentEnemy;
                        }
                    }
                    break;
                case TargetType.HIGHEST_HEALTH:
                    foreach (Collider2D collider2d in enemiesColliders) {
                        EnemyManager currentEnemy = collider2d.GetComponent<EnemyManager>();
                        if (currentEnemy.GetCurrentHP() > targetEnemyManager.GetCurrentHP()) {
                            targetEnemyManager = currentEnemy;
                        }
                    }
                    break;
                default:
                    break;
            }
            targetEnemy = targetEnemyManager.gameObject;
        }
        return targetEnemy;
    }

    public void AddBonusToAttackDamage(float amount) {
        this.damage *= amount;
    }

    public void AddBonusToAttackSpeed(float amount) {
        this.attackSpeed *= amount;
    }

    public void SetRangeVisual() {
        rangeVisual.SetActive(!rangeVisual.activeSelf);
    }

    public void SetTowerMenu() {
        towerMenu.SetActive(!towerMenu.activeSelf);
    }

}
