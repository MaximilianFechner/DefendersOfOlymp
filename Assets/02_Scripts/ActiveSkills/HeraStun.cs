using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class HeraStun : MonoBehaviour
{
    public GameObject stunPrefab;
    public GameObject stunPreview;
    private GameObject currentPreview;
    public LayerMask enemyLayer;

    [Space(10)]
    [Header("Game Design Values")]
    [Tooltip("The damage per tick in every interval")]
    [Min(0)]
    [SerializeField]
    private float _damageOnUse = 15f;

    [Tooltip("The Percentage for the default movement speed")]
    [Min(0)]
    [SerializeField]
    private float _slowPercentage = 0.1f;

    [Tooltip("The time how long the skill is active")]
    [Min(0)]
    [SerializeField]
    private float _slowDuration = 3f;

    [Tooltip("The radius for the skill")]
    [Min(0)]
    [SerializeField]
    private float _skillRadius = 5f;

    [Tooltip("The time you have to wait before you can use the skill again")]
    [Min(0)]
    [SerializeField]
    private float _cooldownTime = 30f;

    //private int skillLevel;
    //private float levelModifikatorDamage;
    //private float levelModifikatorRadius;
    //private float levelModifikatorDuration;
    //private float levelModifikatorCooldown;

    private float lastUseTime = -Mathf.Infinity;
    private bool isReady = false;

    private void Update()
    {
        if (Time.timeScale == 0) return;

        if (UIManager.Instance.heraSkillCooldown != null)
        {
            float remainingTime = Mathf.Max(0, lastUseTime + _cooldownTime - Time.time);
            UIManager.Instance.heraSkillCooldown.text = remainingTime > 0 ? $"{remainingTime:F1}s" : "Stun";
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateHeraSkill();
        }

        if (isReady)
        {
            PlacementPreview();

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                PlaceStun();
                Destroy(currentPreview);
                currentPreview = null;
            }
        }

    }

    public void ActivateHeraSkill()
    {
        if (Time.timeScale == 0) return;
        if (Time.time >= lastUseTime + _cooldownTime)
        {
            isReady = true;

            if (currentPreview == null)
            {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
                worldPosition.z = 0;

                currentPreview = Instantiate(stunPreview);
                currentPreview.transform.localScale = new Vector3(1 * (_skillRadius / 5), 1 * (_skillRadius / 5), 1 * (_skillRadius / 5));
            }
        }
    }

    private void PlaceStun()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        GameObject stun = Instantiate(stunPrefab, worldPosition, Quaternion.identity);
        stun.transform.localScale = new Vector3(1 * (_skillRadius / 5), 1 * (_skillRadius / 5), 1 * (_skillRadius / 5));
        Destroy(stun, 0.74f);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(worldPosition, _skillRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out EnemyManager enemyManager))
            {
                enemyManager.TakeDamage(_damageOnUse);
            }

            if (enemy.TryGetComponent(out EnemyPathfinding enemyPathfinding))
            {
                enemyPathfinding.SlowMovement(_slowPercentage, _slowDuration);
            }
        }

        lastUseTime = Time.time;
        isReady = false;

        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }
    }

    private void PlacementPreview()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        if (currentPreview == null)
        {
            currentPreview = Instantiate(stunPreview);
            currentPreview.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 10, 0);
            currentPreview.transform.localScale = new Vector3(1 * (_skillRadius / 5), 1 * (_skillRadius / 5), 1 * (_skillRadius / 5));
        }

        currentPreview.transform.position = worldPosition;
    }
}
