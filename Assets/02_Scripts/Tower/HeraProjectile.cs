using UnityEngine;

public class HeraProjectile : BaseProjectile
{

    [SerializeField] private GameObject slowCirclePrefab;
    [SerializeField] private int projectileUpgradeByTowerLevel;

    public GameObject hitPS; //Particle-System for hits

    void FixedUpdate() {
        Move();
    }

    public override void DamageCalculation(GameObject enemy) {
        if (enemy != null) {
            EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();
            enemyManager.TakeDamage(Mathf.RoundToInt(Random.Range(damageLowerLimit, damageUpperLimit)));
            EnemyPathfinding enemyPathfinding = enemy.GetComponent<EnemyPathfinding>();
            float calculatedSlowValue = (100 - slowValue) / 100;
            enemyPathfinding.SlowMovement(calculatedSlowValue, timeSlowed);
            if (towerLevel >= projectileUpgradeByTowerLevel) {
                GameObject slowCircle = Instantiate(slowCirclePrefab, enemy.transform);
                slowCircle.transform.SetParent(null);
                HeraSlowCircle heraSlowCircle = slowCircle.GetComponent<HeraSlowCircle>();
                heraSlowCircle.slowValue = calculatedSlowValue;
                heraSlowCircle.timeSlowed = timeSlowed;
            }
        } else {
            Debug.Log("Enemy is null!");
        }
    }

    private void OnDestroy()
    {
        Instantiate(hitPS, this.transform.position, Quaternion.identity);
        AudioManager.Instance.PlayHitImpactSFX(2);
    }

}
