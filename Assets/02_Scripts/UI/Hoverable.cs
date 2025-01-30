using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Hoverable : MonoBehaviour
{
    private string tooltipInfo;
    private string tooltipData;
    private bool isHovered = false;
    private string hoveredElement;

    private void Start()
    {
        tooltipInfo = "";
        tooltipData = "";
        hoveredElement = "";
    }

    private void Update()
    {
        if (this == null) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePosition);

        if (hit != null && hit.gameObject == gameObject) //&& Input.GetKey(KeyCode.LeftAlt))
        {
            isHovered = true;
            UpdateTooltipText();

            if (TooltipManager.Instance != null)
            {
                TooltipManager.Instance.ShowTooltip(tooltipInfo, hoveredElement);
                TooltipManager.Instance.ShowTooltipData(tooltipData);
                TooltipManager.Instance.UpdateTooltipPosition(Input.mousePosition);
            }
        }
        else if (isHovered)
        {
            isHovered = false;

            if (TooltipManager.Instance != null)
            {
                TooltipManager.Instance.HideTooltip();
            }

            tooltipInfo = "";
            tooltipData = "";
            hoveredElement = "";

            if (TryGetComponent(out ZeusTower zeus))
            {
                zeus.rangeVisual.SetActive(false);
            }
            else if (TryGetComponent(out PoseidonTower poseidon))
            {
                poseidon.rangeVisual.SetActive(false);
            }
            else if (TryGetComponent(out HeraTower hera))
            {
                hera.rangeVisual.SetActive(false);
            }
            else if (TryGetComponent(out HephaistosTower heph))
            {
                heph.rangeVisual.SetActive(false);
            }
        }
    }

    private void UpdateTooltipText()
    {
        if (TryGetComponent(out EnemyManager zeus))
        {
            hoveredElement = "enemy";

            if (zeus.TryGetComponent(out NavMeshAgent navAgent))
            {
                tooltipInfo = $"{zeus.enemyName}\n" +
                    $"Health:\n" +
                    $"Speed:\n";

                tooltipData = $"\n{zeus._currentHP}/{zeus._maxHP}\n" +
                    $"{navAgent.speed}\n";
            }
        }
        else if (TryGetComponent(out ZeusTower zeusSkill))
        {
            hoveredElement = "tower";

            tooltipInfo = $"{zeusSkill.towerName}\n" +
                $"Damage:\n" +
                $"Damage/Bounce:\n" +
                $"Crit Chance:\n" +
                $"Attacks/Second:";

            tooltipData = $"Level: {zeusSkill.towerLevel}\n" +
                $"{zeusSkill.damageLowerLimit} - {zeusSkill.damageUpperLimit}\n" +
                $"-20%/Bounce\n" +
                $"{GameManager.Instance.critChance}%\n" +
                $"{1 / zeusSkill.attackSpeed:F2}";

            if (isHovered)
            {
                zeusSkill.rangeVisual.SetActive(true);
            }

        }
        else if (TryGetComponent(out PoseidonTower poseidon))
        {
            hoveredElement = "tower";

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

            poseidon.rangeVisual.SetActive(true);
        }
        else if (TryGetComponent(out HeraTower hera))
        {
            hoveredElement = "tower";

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

            hera.rangeVisual.SetActive(true);
        }
        else if (TryGetComponent(out HephaistosTower heph))
        {
            hoveredElement = "tower";

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

            heph.rangeVisual.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        tooltipInfo = "";
        tooltipData = "";
        hoveredElement = "";

        if (TooltipManager.Instance != null)
        {
            TooltipManager.Instance.HideTooltip();
        }
    }
}