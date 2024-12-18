using UnityEngine;

public class PoseidonTower : BaseTower
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
        baseProjectile.towerType = towerType;
        baseProjectile.targetEnemy = targetEnemy;
        baseProjectile.enemyLayerMask = enemyLayerMask;
        baseProjectile.damage = damage;
        baseProjectile.movementSpeed = movementSpeed;
    }
}
