using System.Collections.Generic;
using UnityEngine;

public class BuffEffectTower : MonoBehaviour
{
    [SerializeField] private float _buffDamageValue;
    [SerializeField] private float _buffAttackSpeedValue;

    [SerializeField] private List<GameObject> _buffedTowers;

    [SerializeField] private TowerSO _towerSO;

    private void Awake() {
        _buffedTowers = new List<GameObject>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuffTowers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        BuffTowers();
    }

    private void BuffTowers() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _towerSO.attackRadius);
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject.CompareTag("Tower")) {
                if (_buffedTowers.Contains(collider.gameObject)) {
                    return;
                } else {
                    EnemyDetection unbuffedTower = collider.gameObject.GetComponent<EnemyDetection>();
                    unbuffedTower.AddBonusToAttackDamage(CalculatePercentage(_buffDamageValue, true));
                    unbuffedTower.AddBonusToAttackSpeed(CalculatePercentage(_buffAttackSpeedValue, false));
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
