using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    public GameObject lightningPrefab;
    public LayerMask enemyLayer;
    public float damage = 100f;
    public float lightningDuration = 0.5f;
    public float attackRadius = 0.5f;

    void Update()
    {
        if (Time.timeScale != 1) return;
        if (Input.GetMouseButtonDown(0))
        {
            TriggerLightning();
        }
    }

    void TriggerLightning()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        GameObject lightning = Instantiate(lightningPrefab, new Vector3(worldPosition.x, worldPosition.y + 10, 0), Quaternion.identity);
        Destroy(lightning, lightningDuration); // Animation löschen, nachdem sie abgelaufen ist

        Collider2D targetEnemy = Physics2D.OverlapCircle(new Vector2(worldPosition.x, worldPosition.y), attackRadius, enemyLayer);

        if (targetEnemy != null)
        {
            targetEnemy.GetComponent<EnemyManager>().TakeDamage(damage);
        }
    }
}
