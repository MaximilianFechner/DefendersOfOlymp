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

    [SerializeField] public float attackSpeed;
    [SerializeField] public float attackRadius;
    [SerializeField] public LayerMask enemyLayerMask;
    [SerializeField] public TargetType targetType;    
    [SerializeField] public float timer;
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public GameObject targetEnemy;    
    [SerializeField] public GameObject rangeVisual;
    [SerializeField] public Animator animator;
    
    //PROJECTILE
    public GameObject projectilePrefab;
    [SerializeField] public float movementSpeed;
    [SerializeField] public float damage;
    [SerializeField] public float aoeRadius;
    [SerializeField] public float slowValue;
    [SerializeField] public float timeSlowed;
    [SerializeField] public Dictionary<GameObject, float> enemyBonusMalusTable;

    [SerializeField] public TowerType towerType  { get; set; }


    private void Awake() {

        CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = attackRadius;

        rangeVisual.transform.localScale = new Vector3(attackRadius * 2, attackRadius * 2, 1);

        timer = 0;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }


    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("Enemy")) {
            targetEnemy = GetTargetEnemy();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        targetEnemy = null;
    }

    private GameObject GetTargetEnemy() {
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

}
