using UnityEngine;
using UnityEngine.EventSystems;

public class Hoverable : MonoBehaviour
{
    private string tooltipInfo;
    private bool isHovered = false;

    private void Update()
    {
        if (this == null) return;
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePosition);

        if (hit != null && hit.gameObject == gameObject)
        {
            isHovered = true;
            UpdateTooltipText();
            TooltipManager.Instance.ShowTooltip(tooltipInfo);
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
            tooltipInfo = $"{enemy.gameObject.name}\nHealth: {enemy._currentHP}/{enemy._maxHP}";
        }
        //else if (TryGetComponent(out TowerManager tower))
        //{
        //    tooltipInfo = $"{tower.towerName}\nDamage: {tower.minDamage} - {tower.maxDamage}\nRange: {tower.attackRange}";
        //}
    }

    private void OnDestroy()
    {
        if (isHovered)
        {
            TooltipManager.Instance.HideTooltip();
        }
    }
}
