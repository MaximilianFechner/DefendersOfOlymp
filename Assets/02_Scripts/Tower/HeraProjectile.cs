using UnityEngine;

public class HeraProjectile : BaseProjectile
{

    void FixedUpdate() {
        Move();
    }

    public override void DamageCalculation(GameObject enemy) {
        if (enemy != null) {
            EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();
            enemyManager.TakeDamage(damage);
            EnemyPathfinding enemyPathfinding = enemy.GetComponent<EnemyPathfinding>();
            float calculatedSlowValue = (100 - slowValue) / 100;
            enemyPathfinding.SlowMovement(calculatedSlowValue, timeSlowed);
        } else {
            Debug.Log("Enemy is null!");
        }
    }

}
