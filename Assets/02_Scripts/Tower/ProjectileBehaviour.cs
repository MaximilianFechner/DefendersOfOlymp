using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{

    [SerializeField] private GameObject _targetEnemy;
    [SerializeField] private ProjectileSO _projectileSO;

    private Rigidbody2D rb2D;


    private void OnEnable() {
        _projectileSO.ResetData();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        Move();
    }

    private void Move() {
        if (_targetEnemy != null) {
            Vector2 direction = (_targetEnemy.transform.position - rb2D.transform.position).normalized;
            rb2D.MovePosition(rb2D.position + direction * _projectileSO.movementSpeed * Time.fixedDeltaTime);
        } else {
            //TODO Diese Lösung ist nur temporär und muss entfernt werden,
            //wenn Tower erkennt, dass keine Projektile mehr gespawned werden sollen, wenn Life unter 0
            Destroy(gameObject);
        }
    }

    public void SetTargetEnemy(GameObject targetEnemy) {
        this._targetEnemy = targetEnemy;    
    }

    public float GetDamage() {
        return _projectileSO.damage; 
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _projectileSO.aoeRadius);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag.Equals("Enemy") && collider.gameObject.Equals(_targetEnemy)) {
            switch (_projectileSO.towerType) {
                case TowerType.NORMAL:
                    EnemyManager enemy = collider.gameObject.GetComponent<EnemyManager>();
                    enemy.TakeDamage(_projectileSO.damage);
                    break;
                case TowerType.SLOW:
                    EnemyManager enemySlow = collider.gameObject.GetComponent<EnemyManager>();
                    enemySlow.TakeDamage(_projectileSO.damage);
                    //enemySlow.SlowMovement(_projectileSO.slowValue);
                    break;
                case TowerType.AOE:
                    AOEDamageCalculation();
                    break;
            }
            Destroy(gameObject);
        }
    }

    private void AOEDamageCalculation() {
        Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(transform.position, _projectileSO.aoeRadius);
        foreach (Collider2D enemyCollider in enemiesColliders) {
            if (enemyCollider.gameObject.CompareTag("Enemy")) {
                EnemyManager enemy = enemyCollider.gameObject.GetComponent<EnemyManager>();
                enemy.TakeDamage(_projectileSO.damage);
            }
        }
    }

    private void OnDisable() {
        _projectileSO.ResetData();
    }
}
