using UnityEngine;

public class PoseidonProjectile : BaseProjectile
{

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

}
