using System.Collections.Generic;
using UnityEngine;

public class HephaistosTower : BaseTower
{
    [SerializeField] private float _buffDamageValue;
    [SerializeField] private float _buffAttackSpeedValue;

    [SerializeField] private List<GameObject> _buffedTowers;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _buffedTowers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackSpeed) {
            if (GetTargetEnemy() != null) {
                SupportAOEDamageCalculation();
                timer = 0;
            }
        }
    }
    private void SupportAOEDamageCalculation() {
        if (animator != null) {
            animator.SetTrigger("attackTrigger");
        }
        Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (Collider2D enemyCollider in enemiesColliders) {
            if (enemyCollider.gameObject.CompareTag("Enemy")) {
                EnemyManager enemy = enemyCollider.gameObject.GetComponent<EnemyManager>();
                enemy.TakeDamage(damage);
            } else if (GetComponent<Collider>().gameObject.CompareTag("Tower")) {
                if (_buffedTowers.Contains(GetComponent<Collider>().gameObject)) {
                    return;
                } else {
                    BaseTower unbuffedTower = GetComponent<Collider>().gameObject.GetComponent<BaseTower>();
                    unbuffedTower.AddBonusToAttackDamage(CalculatePercentage(_buffDamageValue, true));
                    unbuffedTower.AddBonusToAttackSpeed(CalculatePercentage(_buffAttackSpeedValue, false));
                    _buffedTowers.Add(GetComponent<Collider>().gameObject);
                }

            }
        }
    }


    private float CalculatePercentage(float value, bool isBonus) {
        if (isBonus) {
            return (100 + value) / 100;
        } else {
            return (100 - value) / 100;
        }
    }
    
}
