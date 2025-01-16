using UnityEngine;
using static System.Net.WebRequestMethods;

public class PoseidonProjectile : BaseProjectile
{

    public GameObject hitPS; 

    void FixedUpdate() {
        Move();
    }

    public override void DamageCalculation(GameObject enemy) {
        if (enemy != null) {
            Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(transform.position, aoeRadius);
            foreach (Collider2D enemyCollider in enemiesColliders) {
                if (enemyCollider.gameObject.CompareTag("Enemy")) {
                    EnemyManager enemyManager = enemyCollider.gameObject.GetComponent<EnemyManager>();
                    enemyManager.TakeDamage(damage);
                }
            }
        } else {
            Debug.Log("Enemy is null!");
        }
    }

    private void OnDestroy()
    {
        Instantiate(hitPS, this.transform.position, Quaternion.identity);
    }

}
