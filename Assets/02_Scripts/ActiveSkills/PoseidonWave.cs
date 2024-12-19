using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PoseidonWave : MonoBehaviour
{
    public GameObject wavePrefab;
    public GameObject wavePreview;
    private GameObject currentPreview;
    public LayerMask enemyLayer;

    [Space(10)]
    [Header("Game Design Values")]
    [Tooltip("The damage per tick in every interval")]
    [Min(0)]
    [SerializeField]
    private float _damagePerInterval = 10f;

    [Tooltip("The time between the damage ticks")]
    [Min(0)]
    [SerializeField]
    private float _damageIntervalSeconds = 2f;

    [Tooltip("The time how long the skill is active")]
    [Min(0)]
    [SerializeField]
    private float _waveDuration = 10f;

    [Tooltip("The radius for the skill")]
    [Min(0)]
    [SerializeField]
    private float _waveRadius = 5f;

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
        if (Time.timeScale != 1) return;

        if (UIManager.Instance.poseidonSkillCooldown != null)
        {
            float remainingTime = Mathf.Max(0, lastUseTime + _cooldownTime - Time.time);
            UIManager.Instance.poseidonSkillCooldown.text = remainingTime > 0 ? $"{remainingTime:F1}s" : "Wave";
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivatePoseidonSkill();
        }

        if (isReady)
        {
            PlacementPreview();

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                PlaceWave();
                Destroy(currentPreview);
                currentPreview = null;
            }
        }

    }

    public void ActivatePoseidonSkill()
    {
        if (Time.timeScale != 1) return;
        if (Time.time >= lastUseTime + _cooldownTime)
        {
            isReady = true;

            if (currentPreview == null)
            {
                currentPreview = Instantiate(wavePreview);
                currentPreview.transform.localScale = new Vector3(1 * (_waveRadius / 4), 1 * (_waveRadius / 4), 1 * (_waveRadius / 4));
            }
        }
    }

    private void PlaceWave()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        GameObject wave = Instantiate(wavePrefab, worldPosition, Quaternion.identity);
        wave.transform.localScale = new Vector3(1 * (_waveRadius / 4), 1 * (_waveRadius / 4), 1 * (_waveRadius / 4));
        StartCoroutine(PoseidonWaveDamageOverTime(wave.transform.position));
        Destroy(wave, _waveDuration);

        lastUseTime = Time.time;
        isReady = false;

        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }
    }

    private IEnumerator PoseidonWaveDamageOverTime(Vector3 wavePosition)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _waveDuration)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(wavePosition, _waveRadius, enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.TryGetComponent(out EnemyManager enemyManager))
                {
                    enemyManager.TakeDamage(_damagePerInterval);
                }
            }

            elapsedTime += _damageIntervalSeconds;
            yield return new WaitForSeconds(_damageIntervalSeconds);
        }
    }

    private void PlacementPreview()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        if (currentPreview == null)
        {
            currentPreview = Instantiate(wavePreview);
            currentPreview.transform.localScale = new Vector3(1 * (_waveRadius / 4), 1 * (_waveRadius / 4), 1 * (_waveRadius / 4));
        }

        currentPreview.transform.position = worldPosition;
    }

}
