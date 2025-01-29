using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Hoverable : MonoBehaviour
{
    private string tooltipInfo;
    private string tooltipData;
    private bool isHovered = false;

    private void Update()
    {
        if (this == null) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePosition);

        if (hit != null && hit.gameObject == gameObject) //&& Input.GetKey(KeyCode.LeftAlt))
        {
            isHovered = true;
            UpdateTooltipText();
            TooltipManager.Instance.ShowTooltip(tooltipInfo);
            TooltipManager.Instance.ShowTooltipData(tooltipData);
            TooltipManager.Instance.UpdateTooltipPosition(Input.mousePosition);
        }
        else if (isHovered)
        {
            isHovered = false;
            TooltipManager.Instance.HideTooltip();
        }
    }

    private void UpdateTooltipText()
    {
        if (TryGetComponent(out EnemyManager enemy))
        {
            if (enemy.TryGetComponent(out NavMeshAgent navAgent))
            {
                tooltipInfo = $"{enemy.enemyName}\n" +
                    $"Health:\n" +
                    $"Health+/Wave\n" +
                    $"Speed:\n" +
                    $"Speed+/Wave";

                tooltipData = $"\n{enemy._currentHP}/{enemy._maxHP}\n" +
                    $"{enemy.absoluteHPIncreaseWave + enemy.prozentualHPIncreaseWave}\n" +
                    $"{navAgent.speed}\n" +
                    $"{enemy.absoluteSpeedIncreaseWave + enemy.prozentualSpeedIncreaseWave}";
            }
        }
        else if (TryGetComponent(out ZeusTower zeus))
        {
            tooltipInfo = $"{zeus.towerName}\n" +
                $"Damage:\n" +
                $"Damage/Bounce:\n" +
                $"Crit Chance:\n" +
                $"Attacks/Second:";

            tooltipData = $"Level: {zeus.towerLevel}\n" +
                $"{zeus.damageLowerLimit} - {zeus.damageUpperLimit}\n" +
                $"-20%/Bounce\n" +
                $"{GameManager.Instance.critChance}%\n" +
                $"{1 / zeus.attackSpeed:F2}";
        }
        else if (TryGetComponent(out PoseidonTower poseidon))
        {
            tooltipInfo = $"{poseidon.towerName}\n" +
                $"Damage:\n" +
                $"Damage Area:\n" +
                $"Crit Chance:\n" +
                $"Attacks/Second:";

            tooltipData = $"Level: {poseidon.towerLevel}\n" +
                $"{poseidon.damageLowerLimit} - {poseidon.damageUpperLimit}\n" +
                $"{poseidon.aoeRadius}\n" +
                $"{GameManager.Instance.critChance}%\n" +
                $"{1 / poseidon.attackSpeed:F2}";
        }
        else if (TryGetComponent(out HeraTower hera))
        {
            tooltipInfo = $"{hera.towerName}\n" +
                $"Damage:\n" +
                $"Slow Value:\n" +
                $"Crit Chance:\n" +
                $"Attacks/Second:";

            tooltipData = $"Level: {hera.towerLevel}\n" +
                $"{hera.damageLowerLimit} - {hera.damageUpperLimit}\n" +
                $"{hera.slowValue}%\n" +
                $"{GameManager.Instance.critChance}%\n" +
                $"{1 / hera.attackSpeed:F2}";
        }
        else if (TryGetComponent(out HephaistosTower heph))
        {
            tooltipInfo = $"{heph.towerName}\n" +
                $"Damage:\n" +
                $"Damage/Speed Buff Value:\n" +
                $"Crit Chance:\n" +
                $"Attacks/Second:";

            tooltipData = $"Level: {heph.towerLevel}\n" +
                $"{heph.damageLowerLimit} - {heph.damageUpperLimit}\n" +
                $"5%\n" +
                $"{GameManager.Instance.critChance}%\n" +
                $"{1 / heph.attackSpeed:F2}";
        }
    }

    private void OnDestroy()
    {
        if (isHovered)
        {
            TooltipManager.Instance.HideTooltip();
        }
    }
}