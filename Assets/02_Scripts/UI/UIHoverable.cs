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
                tooltipInfo = $"Lightning Strike\n" +
                    $"Damage:\n" +
                    $"Damage Type:\n" +
                    $"Crit Chance:\n" +
                    $"Cooldown:";

                tooltipData = $"Level: {bolt.zeusSkillLevel}\n" +
                    $"{bolt.damageLowerLimit} - {bolt.damageUpperLimit}\n" +
                    $"Single Target\n" +
                    $"{GameManager.Instance.critChance}%\n" +
                    $"{bolt.cooldownTime}s";
            }
        }
        else if (gameObject.name == "BTNPoseidonSkill")
        {
            hoveredElement = "skill";

            if (wave != null)
            {
                tooltipInfo = $"Holy Wave\n" +
                    $"Damage / {wave._damageIntervalSeconds}s:\n" +
                    $"Damage Type:\n" +
                    $"Crit Chance:\n" +
                    $"Duration:\n" +
                    $"Cooldown:";

                tooltipData = $"Level: {wave.poseidonSkillLevel}\n" +
                    $"{wave.damageLowerLimitPerInterval} - {wave.damageUpperLimitPerInterval}\n" +
                    $"Area Of Effect\n" +
                    $"{GameManager.Instance.critChance}%\n" +
                    $"{wave._waveDuration}s\n" +
                    $"{wave._cooldownTime}s";
            }
        }
        else if (gameObject.name == "BTNHeraSkill")
        {
            hoveredElement = "skill";

            if (stun != null)
            {
                tooltipInfo = $"Toxic Binding\n" +
                    $"Damage:\n" +
                    $"Damage Type:\n" +
                    $"Crit Chance:\n" +
                    $"Slow / Duration:\n" +
                    $"Cooldown:";

                tooltipData = $"Level: {stun.heraSkillLevel}\n" +
                    $"{stun.damageLowerLimit} - {stun.damageUpperLimit}\n" +
                    $"Area Of Effect\n" +
                    $"{GameManager.Instance.critChance}%\n" +
                    $"{(1 - stun._slowPercentage) * 100f}% / {stun._slowDuration}s\n" +
                    $"{stun._cooldownTime}s";
            }
        }
        else if (gameObject.name == "BTNHephaistosSkill")
        {
            hoveredElement = "skill";

            if (quake != null)
            {
                tooltipInfo = $"Earths Anger\n" +
                    $"Damage / {quake._damageIntervalSeconds}s\n" +
                    $"Damage Type:\n" +
                    $"Crit Chance:\n" +
                    $"Slow / Duration:\n" +
                    $"Cooldown:";

                tooltipData = $"Level: {quake.hephaistosSkillLevel}\n" +
                    $"{quake.damageUpperLimitPerInterval} - {quake.damageUpperLimitPerInterval}\n" +
                    $"Area Of Effect\n" +
                    $"{GameManager.Instance.critChance}%\n" +
                    $"{(1 - quake._slowPercentage) * 100f}% / {quake._quakeDuration}s\n" +
                    $"{quake._cooldownTime}s";
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

