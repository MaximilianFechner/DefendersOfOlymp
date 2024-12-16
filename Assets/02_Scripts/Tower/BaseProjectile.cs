using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{

    public GameObject prefab;

    [SerializeField] public float movementSpeed { get; set; }
    [SerializeField] public float damage { get; set; }
    [SerializeField] public float aoeRadius { get; set; }
    [SerializeField] public float slowValue { get; set; }
    [SerializeField] public float timeSlowed { get; set; }
    [SerializeField] public TowerType towerType { get; set; }
    [SerializeField] public LayerMask enemyLayerMask { get; set; }
    [SerializeField] public Dictionary<GameObject, float> enemyBonusMalusTable { get; set; }

    [SerializeField] public GameObject targetEnemy { get; set; }
    public Rigidbody2D rb2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Move() {
        if (targetEnemy != null) {
            Vector2 direction = (targetEnemy.transform.position - rb2D.transform.position).normalized;
            rb2D.MovePosition(rb2D.position + direction * movementSpeed * Time.fixedDeltaTime);
        } else {
            //TODO Diese Lösung ist nur temporär und muss entfernt werden,
            //wenn Tower erkennt, dass keine Projektile mehr gespawned werden sollen, wenn Life unter 0
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aoeRadius);
    }

    
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag.Equals("Enemy") && collider.gameObject.Equals(targetEnemy)) {
            DamageCalculation(collider.gameObject);
            Destroy(gameObject);
        }
    }

    public abstract void DamageCalculation(GameObject enemy);
    

}
