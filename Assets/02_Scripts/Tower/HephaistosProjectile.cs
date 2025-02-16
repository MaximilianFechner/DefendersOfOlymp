using UnityEngine;

public class HephaistosProjectile : BaseProjectile
{

    void FixedUpdate() {
        Move();
    }

    public override void DamageCalculation(GameObject enemy) {
        if (enemy != null) 
        {
            AudioManager.Instance.PlayHitImpactSFX(3);

            EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();
            enemyManager.TakeDamage(damage);
        } 
        else 
        {
            Debug.Log("Enemy is null!");
        }
    }

    //private void OnDestroy()
    //{
    //    AudioManager.Instance.PlayHitImpactSFX(3);
    //}

}
