using UnityEngine;
using static System.Net.WebRequestMethods;

public class PoseidonProjectile : BaseProjectile
{

    public GameObject hitPS; 

    void FixedUpdate() {
        Move();
    }

    public override void DamageCalculation(GameObject enemy) {
        if (enemy != null)
        {
            Instantiate(hitPS, this.transform.position, Quaternion.identity);
            AudioManager.Instance.PlayHitImpactSFX(1);

            Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(transform.position, aoeRadius);
            foreach (Collider2D enemyCollider in enemiesColliders) 
            {
                if (enemyCollider.isTrigger)
                {
                    if (enemyCollider.gameObject.CompareTag("Enemy"))
                    {
                        EnemyManager enemyManager = enemyCollider.gameObject.GetComponent<EnemyManager>();
                        enemyManager.TakeDamage(Mathf.RoundToInt(Random.Range(damageLowerLimit, damageUpperLimit)));
                    }
                }
            }
        } 
        else 
        {
            Debug.Log("Enemy is null!");
        }
    }

    //private void OnDestroy()
    //{
    //    Instantiate(hitPS, this.transform.position, Quaternion.identity);
    //    AudioManager.Instance.PlayHitImpactSFX(1);
    //}

}
