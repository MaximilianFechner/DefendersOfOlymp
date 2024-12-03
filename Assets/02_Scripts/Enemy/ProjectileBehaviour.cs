using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{

    [SerializeField] private GameObject _targetEnemy;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _damage;

    private Rigidbody2D rb2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_targetEnemy != null) {
            Vector2 direction = (_targetEnemy.transform.position - rb2D.transform.position).normalized;
            rb2D.MovePosition(rb2D.position + direction * _movementSpeed * Time.fixedDeltaTime);
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
        return _damage; 
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag.Equals("Enemy") && collider.gameObject.Equals(_targetEnemy)) {
            EnemyManager enemy = collider.gameObject.GetComponent<EnemyManager>();
            enemy.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
