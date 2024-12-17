using System;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{

    [SerializeField] private TowerSO _towerSO;
    [SerializeField] private float _timer;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _targetEnemy;

    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;

    [SerializeField] private GameObject rangeVisual;
    [SerializeField] private Animator _animator;

    private void Awake() {

        CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = _towerSO.attackRadius;
        attackDamage = _towerSO.projectileSO.damage;
        attackSpeed = _towerSO.attackSpeed;

        rangeVisual.transform.localScale = new Vector3(_towerSO.attackRadius * 2, _towerSO.attackRadius * 2, 1);

        _timer = 0;
    }

    private void OnEnable() {
        _towerSO.ResetData();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _towerSO.attackRadius);
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        //TODO Hier müsste die Überprüfung kommen, ob der Gegner noch ein Projektil aushält,
        //Wenn nicht, soll ein anderer Enemy ausgewählt werden.
        if (_towerSO.projectileSO.towerType.Equals(TowerType.SUPPORT) && _timer >= attackSpeed) {
            SupportAOEDamageCalculation();
            _timer = 0;
        }else if (_targetEnemy != null && _timer >= attackSpeed) {
            SpawnProjectile(_targetEnemy);
            _timer = 0;
        }
    }

    private void SupportAOEDamageCalculation() {
        if (_animator != null) {
            _animator.SetTrigger("attackTrigger");
        }
        Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(transform.position, _towerSO.attackRadius);
        foreach (Collider2D enemyCollider in enemiesColliders) {
            if (enemyCollider.gameObject.CompareTag("Enemy")) {
                EnemyManager enemy = enemyCollider.gameObject.GetComponent<EnemyManager>();
                enemy.TakeDamage(attackDamage);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("Enemy")) {
            _targetEnemy = GetTargetEnemy();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        _targetEnemy = null;
    }

    private GameObject GetTargetEnemy() {
        Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(transform.position, _towerSO.attackRadius, _towerSO.enemyLayerMask);
        _targetEnemy = null;
        if (enemiesColliders.Length > 0) {
            EnemyManager targetEnemyManager = enemiesColliders[0].GetComponent<EnemyManager>();

            switch (_towerSO.targetType) {
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
            _targetEnemy = targetEnemyManager.gameObject;
        }
        return _targetEnemy;
    }

    private void SpawnProjectile(GameObject target) {
        if (_animator != null) {
            _animator.SetTrigger("attackTrigger");
        }
        GameObject projectile = Instantiate(_towerSO.projectileSO.prefab, _spawnPoint);
        projectile.transform.SetParent(null);
        ProjectileBehaviour projectileBehaviour = projectile.GetComponent<ProjectileBehaviour>();
        projectileBehaviour.SetTargetEnemy(target);
        projectileBehaviour.SetDamage(attackDamage);
    }


    private void OnDisable() {
        _towerSO.ResetData();
    }

    public TowerSO GetTowerSO() { return _towerSO; }

    public float GetAttackDamage() {
        return this.attackDamage;
    }

    public void SetAttackDamage(float attackSpeed) {
        this.attackSpeed = attackSpeed;
    }
    public void AddBonusToAttackDamage(float amount) {
        this.attackDamage *= amount;
    }
    public float GetAttackSpeed() {
        return this.attackDamage;
    }

    public void SetAttackSpeed(float attackSpeed) {
        this.attackSpeed = attackSpeed;
    }


    public void AddBonusToAttackSpeed(float amount) {
        this.attackSpeed *= amount;
    }

    public void SetRangeVisual() {
        rangeVisual.SetActive(!rangeVisual.activeSelf);
    }

}
