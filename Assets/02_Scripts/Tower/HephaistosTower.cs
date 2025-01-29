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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (Collider2D collider in colliders) 
        {
            if (collider.isTrigger)
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    EnemyManager enemy = collider.gameObject.GetComponent<EnemyManager>();
                    enemy.TakeDamage(Mathf.RoundToInt(Random.Range(damageLowerLimit, damageUpperLimit)));
                }
                else if (collider.gameObject.CompareTag("Tower"))
                {
                    if (_buffedTowers.Contains(collider.gameObject))
                    {
                        return;
                    }
                    else
                    {
                        BaseTower unbuffedTower = collider.gameObject.GetComponent<BaseTower>();
                        unbuffedTower.AddBonusToAttackDamage(CalculatePercentage(_buffDamageValue, true));
                        unbuffedTower.AddBonusToAttackSpeed(CalculatePercentage(_buffAttackSpeedValue, false));
                        _buffedTowers.Add(collider.gameObject);
                    }
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
