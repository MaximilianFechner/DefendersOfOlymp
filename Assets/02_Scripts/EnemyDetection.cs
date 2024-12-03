using UnityEngine;

public class EnemyDetection : MonoBehaviour
{

    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private TargetType _targetType;

    [SerializeField] private float _timeToSpawn;
    [SerializeField] private float _timer;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _targetEnemy;

    private enum TargetType {
        lowestHealth,
        highestHealth
    }

    private void Awake() {
        CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = _radius;

        _timer = 0;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_targetEnemy != null && _timer >= _timeToSpawn) {
            //TODO Hier müsste die Überprüfung kommen, ob der Gegner noch ein Projektil aushält,
            //Wenn nicht, soll ein anderer Enemy ausgewählt werden.
            SpawnProjectile(_targetEnemy);
            _timer = 0;
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
        Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(transform.position, _radius, _enemyLayerMask);
        _targetEnemy = null;
        if (enemiesColliders.Length > 0) {
            EnemyManager targetEnemyManager = enemiesColliders[0].GetComponent<EnemyManager>();

            switch (_targetType) {
                case TargetType.lowestHealth:
                    foreach (Collider2D collider2d in enemiesColliders) {
                        EnemyManager currentEnemy = collider2d.GetComponent<EnemyManager>();
                        if (currentEnemy.GetCurrentHP() < targetEnemyManager.GetCurrentHP()) {
                            targetEnemyManager = currentEnemy;
                        }
                    }
                    break;
                case TargetType.highestHealth:
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
        GameObject projectile = Instantiate(_projectilePrefab, _spawnPoint);
        projectile.transform.SetParent(null);
        ProjectileBehaviour projectileBehaviour = projectile.GetComponent<ProjectileBehaviour>();
        projectileBehaviour.SetTargetEnemy(target);
    }
}
