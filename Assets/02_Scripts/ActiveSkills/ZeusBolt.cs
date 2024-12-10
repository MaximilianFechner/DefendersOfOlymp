using UnityEngine;

public class ZeusBolt : MonoBehaviour
{
    public GameObject lightningPrefab;
    public LayerMask enemyLayer;

    [Space(10)]
    [Header("Game Design Values")]
    [Tooltip("The damage dealt by the skill")]
    [Min(0)]
    [SerializeField]
    private float damage = 100f;

    [Tooltip("Animationtime, it doesnt change the damage")]
    [Min(0)]
    [SerializeField]
    private float lightningDuration = 0.5f;

    [Tooltip("Radius for detecting collider of enemies")]
    [Min(0)]
    [SerializeField]
    private float attackRadius = 0.5f;

    [Tooltip("The time you have to wait before you can use the skill again")]
    [Min(0)]
    [SerializeField]
    private float cooldownTime = 20f;

    //private int skillLevel;
    //private float levelModifikatorDamage;
    //private float levelModifikatorCooldown;

    private float lastUseTime = -Mathf.Infinity;
    private bool isReady = false;

    void Update()
    {
        if (Time.timeScale != 1) return;
        if (UIManager.Instance.zeusSkillCooldown != null)
        {
            float remainingTime = Mathf.Max(0, lastUseTime + cooldownTime - Time.time);
            UIManager.Instance.zeusSkillCooldown.text = remainingTime > 0 ? $"{remainingTime:F1}s" : "Ready";
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            ActivateZeusSkill();
        }

        if (isReady && Input.GetMouseButtonDown(0))
        {
            TriggerLightning();
        }
    }

    public void ActivateZeusSkill()
    {
        if (Time.time >= lastUseTime + cooldownTime)
        {
            isReady = true;
        }
    }

    void TriggerLightning()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        GameObject bolt = Instantiate(lightningPrefab, new Vector3(worldPosition.x, worldPosition.y + 10, 0), Quaternion.identity);
        Destroy(bolt, lightningDuration);

        Collider2D targetEnemy = Physics2D.OverlapCircle(new Vector2(worldPosition.x, worldPosition.y), attackRadius, enemyLayer);

        if (targetEnemy != null)
        {
            targetEnemy.GetComponent<EnemyManager>().TakeDamage(damage);
        }

        lastUseTime = Time.time;
        isReady = false;
    }
}
