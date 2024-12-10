using UnityEngine;
using System.Collections;

public class PoseidonWave : MonoBehaviour
{
    public GameObject wavePrefab;
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
            UIManager.Instance.poseidonSkillCooldown.text = remainingTime > 0 ? $"{remainingTime:F1}s" : "Ready";
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivatePoseidonSkill();
        }

        if (isReady && Input.GetMouseButtonDown(0))
        {
            PlaceWave();
        }
    }

    public void ActivatePoseidonSkill()
    {
        if (Time.time >= lastUseTime + _cooldownTime)
        {
            isReady = true;
        }
    }

    private void PlaceWave()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        GameObject wave = Instantiate(wavePrefab, worldPosition, Quaternion.identity);
        wave.transform.localScale = new Vector3(_waveRadius, _waveRadius, _waveRadius);
        StartCoroutine(PoseidonWaveDamageOverTime(wave.transform.position));
        Destroy(wave, _waveDuration);

        lastUseTime = Time.time;
        isReady = false;
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
}
