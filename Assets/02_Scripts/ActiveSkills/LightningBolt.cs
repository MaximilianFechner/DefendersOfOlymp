using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    public GameObject lightningPrefab;
    public LayerMask enemyLayer;
    public float damage = 100f;
    public float lightningDuration = 0.5f;
    public float attackRadius = 0.5f;
    public float cooldownTime = 20f;
    
    private float lastUseTime = -Mathf.Infinity;
    private bool isLightningBoltReady = false;

    void Update()
    {
        if (Time.timeScale != 1) return;
        if (UIManager.Instance.zeusSkillCooldown != null)
        {
            float remainingTime = Mathf.Max(0, lastUseTime + cooldownTime - Time.time);
            UIManager.Instance.zeusSkillCooldown.text = remainingTime > 0 ? $"{remainingTime:F1}s" : "Ready!";
        }

        if (isLightningBoltReady && Input.GetMouseButtonDown(0))
        {
            TriggerLightning();
        }
    }

    public void ActivateLightningStrike()
    {
        if (Time.time >= lastUseTime + cooldownTime)
        {
            isLightningBoltReady = true;
        }
    }

    void TriggerLightning()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        GameObject lightning = Instantiate(lightningPrefab, new Vector3(worldPosition.x, worldPosition.y + 10, 0), Quaternion.identity);
        Destroy(lightning, lightningDuration);

        Collider2D targetEnemy = Physics2D.OverlapCircle(new Vector2(worldPosition.x, worldPosition.y), attackRadius, enemyLayer);

        if (targetEnemy != null)
        {
            targetEnemy.GetComponent<EnemyManager>().TakeDamage(damage);
        }

        lastUseTime = Time.time;
        isLightningBoltReady = false;
    }
}
