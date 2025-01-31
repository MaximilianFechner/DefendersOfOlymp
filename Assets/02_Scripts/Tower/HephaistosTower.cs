using System.Collections.Generic;
using UnityEngine;

public class HephaistosTower : BaseTower
{
    [SerializeField] private float _buffDamageValue;
    [SerializeField] private float _buffDamageValueAbsolute;
    [SerializeField] private float _buffAttackSpeedValue;

    private List<GameObject> _buffedTowers = new List<GameObject>();

    void Start()
    {
         //ApplyBuffToNearbyTowers();
    }

    //private void ApplyBuffToNearbyTowers()
    //{
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);

    //    foreach (Collider2D collider in colliders)
    //    {
    //        if (collider.CompareTag("Tower") && !_buffedTowers.Contains(collider.gameObject))
    //        {
    //            BaseTower tower = collider.GetComponent<BaseTower>();
    //            if (tower != null)
    //            {
    //                ApplyBuff(tower);
    //                _buffedTowers.Add(collider.gameObject);
    //            }
    //        }
    //    }
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Tower") && !_buffedTowers.Contains(collision.gameObject))
    //    {
    //        BaseTower newTower = collision.gameObject.GetComponent<BaseTower>();
    //        if (newTower != null)
    //        {
    //            ApplyBuff(newTower);
    //            _buffedTowers.Add(collision.gameObject);
    //        }
    //    }
    //}


    //private void ApplyBuff(BaseTower tower)
    //{
    //    Debug.Log($"Buffing tower: {tower.gameObject.name}");

    //    float bonusDamage = CalculatePercentage(_buffDamageValue, true);
    //    float bonusAttackSpeed = CalculatePercentage(_buffAttackSpeedValue, false);

    //    Debug.Log($"Damage Buff: {bonusDamage}");
    //    Debug.Log($"Attack Speed Buff: {bonusAttackSpeed}");

    //    tower.AddBonusToAttackDamage(bonusDamage);
    //    tower.AddBonusToAttackDamageAbsolute(Mathf.RoundToInt(_buffDamageValueAbsolute));
    //    tower.AddBonusToAttackSpeed(bonusAttackSpeed);
    //}

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

                if (collider.gameObject.CompareTag("Enemy") && collider.isTrigger)
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

                    //Debug.Log($"Buffing tower: {collider.gameObject.name}");

                    //float bonusDamage = CalculatePercentage(_buffDamageValue, true);
                    //Debug.Log($"Damage Buff: {bonusDamage}");

                    float bonusAttackSpeed = CalculatePercentage(_buffAttackSpeedValue, false);
                    //Debug.Log($"Attack Speed Buff: {bonusAttackSpeed}");

                    //unbuffedTower.AddBonusToAttackDamage(bonusDamage);
                    unbuffedTower.AddBonusToAttackDamageAbsolute(Mathf.RoundToInt(_buffDamageValueAbsolute));
                    unbuffedTower.AddBonusToAttackSpeed(bonusAttackSpeed);

                    _buffedTowers.Add(collider.gameObject);
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
