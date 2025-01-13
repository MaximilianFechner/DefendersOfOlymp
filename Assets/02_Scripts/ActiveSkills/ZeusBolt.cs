using UnityEngine;
using UnityEngine.EventSystems;

public class ZeusBolt : MonoBehaviour
{
    public GameObject boltPrefab;
    public GameObject boltPreview;
    private GameObject currentPreview;
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

    private void Update()
    {
        if (Time.timeScale == 0) return;
        if (UIManager.Instance.zeusSkillCooldown != null)
        {
            float remainingTime = Mathf.Max(0, lastUseTime + cooldownTime - Time.time);
            UIManager.Instance.zeusSkillCooldown.text = remainingTime > 0 ? $"{remainingTime:F1}s" : "Bolt";
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            ActivateZeusSkill();
        }

        if (isReady)
        {
            PlacementPreview();

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                TriggerLightning();
                Destroy(currentPreview);
                currentPreview = null;
            }
        }
    }

    public void ActivateZeusSkill()
    {
        if (Time.time >= lastUseTime + cooldownTime)
        {
            isReady = true;

            if (currentPreview == null)
            {
                currentPreview = Instantiate(boltPreview);
            }
        }
    }

    private void TriggerLightning()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        Collider2D targetEnemy = Physics2D.OverlapCircle(new Vector2(worldPosition.x, worldPosition.y), attackRadius, enemyLayer);

        if (targetEnemy != null)
        {
            GameObject bolt = Instantiate(boltPrefab, new Vector3(worldPosition.x, worldPosition.y + 10, 0), Quaternion.identity);
            Destroy(bolt, lightningDuration);

            targetEnemy.GetComponent<EnemyManager>().TakeDamage(damage);

            lastUseTime = Time.time;
            isReady = false;

            if (currentPreview != null)
            {
                Destroy(currentPreview);
                currentPreview = null;
            }
        }
    }

    private void PlacementPreview()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;
        worldPosition.y += 10; //only for positioning for the placeholder asset

        if (currentPreview == null)
        {
            currentPreview = Instantiate(boltPreview);
        }

        currentPreview.transform.position = worldPosition;
    }
}
