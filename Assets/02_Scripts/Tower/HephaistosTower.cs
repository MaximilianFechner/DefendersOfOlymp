using System.Collections.Generic;
using UnityEngine;

public class HephaistosTower : BaseTower
{
    [SerializeField] private float _buffDamageValue;
    [SerializeField] private float _buffDamageValueAbsolute;
    [SerializeField] private float _buffAttackSpeedValue;
    [SerializeField] private float _buffCheckInterval = 1f; // Zeitintervall für die Überprüfung in Sekunden

    private List<GameObject> _buffedTowers = new List<GameObject>();
    private float _buffTimer = 0f;

    void Start()
    {
        ApplyBuffToNearbyTowers();
    }

    void Update()
    {
        // Überprüfen, ob eine Welle aktiv ist
        if (!GameManager.Instance.isInWave)
        {
            return; // Keine Buff-Anwendung, wenn nicht platziert oder keine Welle
        }

        // Buff in regelmäßigen Intervallen anwenden
        _buffTimer += Time.deltaTime;
        if (_buffTimer >= _buffCheckInterval)
        {
            ApplyBuffToNearbyTowers();
            _buffTimer = 0f; // Timer zurücksetzen
        }

        timer += Time.deltaTime;
        if (timer >= attackSpeed)
        {
            if (GetTargetEnemy() != null)
            {
                SupportAOEDamageCalculation();
                timer = 0;
            }
        }
    }

    private void ApplyBuffToNearbyTowers()
    {
        // Türme in Reichweite suchen
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Tower") && !_buffedTowers.Contains(collider.gameObject))
            {
                BaseTower tower = collider.GetComponent<BaseTower>();
                if (tower != null)
                {
                    ApplyBuff(tower);
                    _buffedTowers.Add(collider.gameObject);
                }
            }
        }
    }

    private void ApplyBuff(BaseTower tower)
    {
        Debug.Log($"Buffing tower: {tower.gameObject.name}");

        float bonusDamage = CalculatePercentage(_buffDamageValue, true);
        float bonusAttackSpeed = CalculatePercentage(_buffAttackSpeedValue, false);

        Debug.Log($"Damage Buff: {bonusDamage}");
        Debug.Log($"Attack Speed Buff: {bonusAttackSpeed}");

        tower.AddBonusToAttackDamage(bonusDamage);
        tower.AddBonusToAttackDamageAbsolute(Mathf.RoundToInt(_buffDamageValueAbsolute));
        tower.AddBonusToAttackSpeed(bonusAttackSpeed);
    }

    private float CalculatePercentage(float value, bool isBonus)
    {
        if (isBonus)
        {
            return (100 + value) / 100;
        }
        else
        {
            return (100 - value) / 100;
        }
    }

    private void SupportAOEDamageCalculation()
    {
        if (animator != null)
        {
            animator.SetTrigger("attackTrigger");
            AudioManager.Instance.PlayHitImpactSFX(3);
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(midPoint.transform.position, attackRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy") && collider.isTrigger)
            {
                EnemyManager enemy = collider.gameObject.GetComponent<EnemyManager>();
                enemy.TakeDamage(Mathf.RoundToInt(Random.Range(damageLowerLimit, damageUpperLimit)));
            }
        }
    }
}
