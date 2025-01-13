using UnityEngine;

public class HephaistosProjectile : BaseProjectile
{

    void FixedUpdate() {
        Move();
    }

    public override void DamageCalculation(GameObject enemy) {
        if (enemy != null) {
            EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();
            enemyManager.TakeDamage(damage);
        } else {
            Debug.Log("Enemy is null!");
        }
    }

}
