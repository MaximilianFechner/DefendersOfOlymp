using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class ZeusProjectile : BaseProjectile
{
    private int indexAttackedEnemies = 0;
    private GameObject lastEnemyAttacked;
    [SerializeField] private GameObject hitPS; // Particle-System for hits

    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>(); // Avoid double damage on the same enemy
    private new GameObject targetEnemy; // Current enemy target
    private Vector2 targetPosition; // Position of the current target to move towards
    private Vector2 initialDirection; // Direction of the projectile when it was first fired

    private bool isMovingToNextEnemy = false;

    void Start()
    {
        targetPosition = transform.position;
        // Save initial direction relative to the first target
        initialDirection = (targetPosition - (Vector2)transform.position).normalized;
        // Rotate the projectile towards the first target
        transform.right = initialDirection;
    }

    void FixedUpdate()
    {
        Move();

        if (isMovingToNextEnemy)
        {
            MoveToNextTarget();
            MaintainInitialRotation();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy") && collider.isTrigger && !hitEnemies.Contains(collider.gameObject))
        {
            hitEnemies.Add(collider.gameObject); // Add the enemy to the hit list to avoid re-hitting it
            DamageCalculation(collider.gameObject);
            GetNextTargetEnemy();
        }
    }

    private void MoveToNextTarget()
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float step = movementSpeed * Time.deltaTime; // Movement step based on speed
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);

        // Check if the projectile has reached the target position
        if ((Vector2)transform.position == targetPosition)
        {
            isMovingToNextEnemy = false; // Stop moving to the next target
        }
    }

    private void MaintainInitialRotation()
    {
        // Ensure the projectile maintains the same initial direction for each jump
        transform.right = initialDirection; // Keep the initial direction
    }

    private void GetNextTargetEnemy()
    {
        List<GameObject> enemiesGameObjects = new List<GameObject>();
        Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(transform.position, aoeRadius);

        foreach (Collider2D enemyCollider in enemiesColliders)
        {
            if (enemyCollider.isTrigger && enemyCollider.CompareTag("Enemy") && !hitEnemies.Contains(enemyCollider.gameObject))
            {
                enemiesGameObjects.Add(enemyCollider.gameObject);
            }
        }

        if (enemiesGameObjects.Count > 0)
        {
            GameObject nextTarget = enemiesGameObjects[0]; // Closest enemy is the next target
            targetEnemy = nextTarget;
            targetPosition = targetEnemy.transform.position; // Set target position to the current position of the next enemy

            // Update direction to the target
            initialDirection = (targetPosition - (Vector2)transform.position).normalized;
            transform.right = initialDirection;

            lastEnemyAttacked = targetEnemy;
            isMovingToNextEnemy = true;

            maxDamageJump -= 1;
            indexAttackedEnemies++;

            // If no more enemies to jump to, destroy the projectile
            if (maxDamageJump <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject); // If no more enemies, destroy the projectile
        }
    }

    public override void DamageCalculation(GameObject enemy)
    {
        if (enemy != null)
        {
            EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();
            if (enemyManager != null)
            {
                int damage = Mathf.RoundToInt(Random.Range(damageLowerLimit, damageUpperLimit) *
                    Mathf.Pow(0.8f, indexAttackedEnemies)); // % damage reduce after hit
                enemyManager.TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log("Enemy is null!");
        }
    }

    private void OnDestroy()
    {
        Instantiate(hitPS, this.transform.position, Quaternion.identity);
        AudioManager.Instance.PlayHitImpactSFX(0);
    }
}