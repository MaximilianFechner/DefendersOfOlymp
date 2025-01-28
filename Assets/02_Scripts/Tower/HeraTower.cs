using UnityEngine;

public class HeraTower : BaseTower
{

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;


        if (timer >= attackSpeed) {
            if (GetTargetEnemy() != null) {
                SpawnProjectile();
                timer = 0;
            }
        }
    }
    public void SpawnProjectile() {
        if (animator != null) {
            animator.SetTrigger("attackTrigger");
        }
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint);
        projectile.transform.SetParent(null);
        BaseProjectile baseProjectile = projectile.GetComponent<BaseProjectile>();
        baseProjectile.towerLevel = towerLevel;
        baseProjectile.towerType = towerType;
        baseProjectile.targetEnemy = targetEnemy;
        baseProjectile.enemyLayerMask = enemyLayerMask;
        baseProjectile.damage = damage;
        baseProjectile.damageUpperLimit = damageUpperLimit;
        baseProjectile.damageLowerLimit = damageLowerLimit;
        baseProjectile.movementSpeed = movementSpeed;
        baseProjectile.slowValue = slowValue;
        baseProjectile.timeSlowed = timeSlowed;
    }
}
