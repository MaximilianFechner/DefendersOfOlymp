using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ZeusProjectile : BaseProjectile
{
    private int indexAttackedEnemies = 0;
    private GameObject lastEnemyAttacked;
    [SerializeField] private GameObject projectilePrefab;

    public GameObject hitPS; //Particle-System for hits

    private void Start() {
        lastEnemyAttacked = targetEnemy;
    }

    //Bei einem Hit, neues Projektil mit dem nächsten targetEnemy und überschriebenen Wert des jumpCounter



    void FixedUpdate() {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.tag.Equals("Enemy") && collider.gameObject.Equals(targetEnemy) && collider.isTrigger) 
        {
            DamageCalculation(collider.gameObject);
            GetNextTargetEnemy();
        }
    }

    private void GetNextTargetEnemy() {
        List<GameObject> enemiesGameObjects = new List<GameObject>();
        if (indexAttackedEnemies <= Mathf.RoundToInt(maxDamageJump)) {
            Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(transform.position, aoeRadius);

            if (enemiesColliders.Length > 0) 
            {
                foreach (Collider2D enemyCollider in enemiesColliders) 
                {
                    if (enemyCollider.isTrigger)
                    {
                        if (enemyCollider.gameObject.CompareTag("Enemy"))
                        {
                            enemiesGameObjects.Add(enemyCollider.gameObject);
                        }
                    }
                }
                if (enemiesGameObjects.Count == 1) {
                    Destroy(gameObject);
                    return;
                } else if (enemiesGameObjects.Count > 1) {
                    BubbleSort(enemiesGameObjects);

                    GameObject tempEnemy = enemiesGameObjects[0];
                    for (int i = 1;i < enemiesGameObjects.Count;i++) {
                        if (!tempEnemy.Equals(lastEnemyAttacked) && !tempEnemy.Equals(targetEnemy)) {
                            lastEnemyAttacked = targetEnemy;
                            targetEnemy = tempEnemy;
                            maxDamageJump -= 1;
                            return;
                        }
                        tempEnemy = enemiesGameObjects[i];
                    }

                    if (targetEnemy == null) {
                        Debug.Log("Target enenym is null");
                        Destroy(gameObject);
                        return;
                    }

                }

                if (maxDamageJump <= 0) {
                    Destroy(gameObject);
                    return;
                } else {
                    SpawnProjectile();
                    Destroy(gameObject);
                }

                indexAttackedEnemies++;
            } else {
                Debug.Log("No Enemies found!");
                Destroy(gameObject);
            }
        } else {
            Destroy(gameObject);
        }
    }

    public override void DamageCalculation(GameObject enemy) {
        if (enemy != null) {
            EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();
            enemyManager.TakeDamage(Mathf.RoundToInt(Random.Range(damageLowerLimit, damageUpperLimit)));
        } else {
            Debug.Log("Enemy is null!");
        }
    }

    public void SpawnProjectile() {
        GameObject projectile = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);
        projectile.transform.SetParent(null);
        BaseProjectile baseProjectile = projectile.GetComponent<BaseProjectile>();
        baseProjectile.towerType = towerType;
        baseProjectile.targetEnemy = targetEnemy;
        baseProjectile.enemyLayerMask = enemyLayerMask;
        baseProjectile.damage = damage;
        baseProjectile.damageLowerLimit = damageLowerLimit;
        baseProjectile.damageUpperLimit = damageUpperLimit;
        baseProjectile.movementSpeed = movementSpeed;
        baseProjectile.maxDamageJump = maxDamageJump;
        baseProjectile.aoeRadius = aoeRadius;
    }

    public static void BubbleSort(List<GameObject> enemies) {
        if (enemies.Count > 2) {
            GameObject temp = enemies[0];
            for (int i = 0;i < enemies.Count -1;i++) {
                for (int j = 0;j < enemies.Count - i -1;j++) {
                    float tempDistance = Vector2.Distance(temp.transform.position, enemies[j].transform.position);
                    float newDistance = Vector2.Distance(enemies[j].transform.position, enemies[j + 1].transform.position);
                    if (tempDistance > newDistance) {
                        temp = enemies[j];
                        enemies[j] = enemies[j + 1];
                        enemies[j + 1] = temp;
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        Instantiate(hitPS, this.transform.position, Quaternion.identity);
        AudioManager.Instance.PlayHitImpactSFX(0);
    }

}
