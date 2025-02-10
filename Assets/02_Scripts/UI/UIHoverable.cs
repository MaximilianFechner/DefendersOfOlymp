using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class UIHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string tooltipInfo;
    private string tooltipData;

    public ZeusBolt bolt;
    public PoseidonWave wave;
    public HeraStun stun;
    public HephaistosQuake quake;

    private bool isHovering = false;
    private string hoveredElement;

    private void Start()
    {
        tooltipInfo = "";
        tooltipData = "";
        hoveredElement = "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateTooltipText();
        isHovering = true;

        if (TooltipManager.Instance != null)
        {
            TooltipManager.Instance.ShowTooltip(tooltipInfo, hoveredElement);
            TooltipManager.Instance.ShowTooltipData(tooltipData);
            TooltipManager.Instance.UpdateTooltipPosition(eventData.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = true;

        if (TooltipManager.Instance != null)
        {
            TooltipManager.Instance.HideTooltip();
        }

        tooltipInfo = "";
        tooltipData = "";
        hoveredElement = "";
    }

    private void Update()
    {
        if (isHovering && TooltipManager.Instance != null)
        {
            TooltipManager.Instance.UpdateTooltipPosition(Input.mousePosition);
        }
    }

    private void UpdateTooltipText()
    {

        if (gameObject.name == "BTNZeusSkill")
        {
            hoveredElement = "zeusSkill";

            if (bolt != null)
            {
                tooltipInfo = $"<b><color=#E1E0E1>Lightning Strike</color></b>\n" +
                    $"Damage:\n" +
                    $"Damage Type:\n" +
                    $"Crit Chance:\n" +
                    $"Cooldown:";

                tooltipData = $"Level: <b><color=#E1E0E1>{GameManager.Instance.zeusTower}</color></b>\n" +
                    $"{bolt.damageLowerLimit} - {bolt.damageUpperLimit}\n" +
                    $"Single Target\n" +
                    $"{GameManager.Instance.critChance}%\n" +
                    $"{Mathf.Round(bolt.cooldownTime * 10f) / 10f}s";
            }
        }
        else if (gameObject.name == "BTNPoseidonSkill")
        {
            hoveredElement = "skill";

            if (wave != null)
            {
                tooltipInfo = $"<b><color=#0EA1D2>Holy Wave</color></b>\n" +
                    $"Damage / {wave._damageIntervalSeconds}s:\n" +
                    $"Damage Type:\n" +
                    $"Crit Chance:\n" +
                    $"Duration:\n" +
                    $"Cooldown:";

                tooltipData = $"Level: <b><color=#0EA1D2>{GameManager.Instance.poseidonTower}</color></b>\n" +
                    $"{wave.damageLowerLimitPerInterval} - {wave.damageUpperLimitPerInterval}\n" +
                    $"Area Of Effect\n" +
                    $"{GameManager.Instance.critChance}%\n" +
                    $"{wave._waveDuration}s\n" +
                    $"{Mathf.Round(wave._cooldownTime * 10f) / 10f}s";
            }
        }
        else if (gameObject.name == "BTNHeraSkill")
        {
            hoveredElement = "skill";

            if (stun != null)
            {
                tooltipInfo = $"<b><color=#E19CF1>Toxic Binding</color></b>\n" +
                    $"Damage:\n" +
                    $"Damage Type:\n" +
                    $"Crit Chance:\n" +
                    $"Slow / Duration:\n" +
                    $"Cooldown:";

                tooltipData = $"Level: <b><color=#E19CF1>{GameManager.Instance.heraTower}</color></b>\n" +
                    $"{stun.damageLowerLimit} - {stun.damageUpperLimit}\n" +
                    $"Area Of Effect\n" +
                    $"{GameManager.Instance.critChance}%\n" +
                    $"{(1 - stun._slowPercentage) * 100f}% / {stun._slowDuration}s\n" +
                    $"{Mathf.Round(stun._cooldownTime * 10f) / 10f}s";
            }
        }
        else if (gameObject.name == "BTNHephaistosSkill")
        {
            hoveredElement = "skill";

            if (quake != null)
            {
                tooltipInfo = $"<b><color=#FA9821>Earths Anger</color></b>\n" +
                    $"Damage / {quake._damageIntervalSeconds}s\n" +
                    $"Damage Type:\n" +
                    $"Crit Chance:\n" +
                    $"Slow / Duration:\n" +
                    $"Cooldown:";

                tooltipData = $"Level: <b><color=#FA9821>{GameManager.Instance.hephaistosTower}</color></b>\n" +
                    $"{quake.damageLowerLimitPerInterval} - {quake.damageUpperLimitPerInterval}\n" +
                    $"Area Of Effect\n" +
                    $"{GameManager.Instance.critChance}%\n" +
                    $"{(1 - quake._slowPercentage) * 100f}% / {quake._quakeDuration}s\n" +
                    $"{Mathf.Round(quake._cooldownTime * 10f) / 10f}s";
            }
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

