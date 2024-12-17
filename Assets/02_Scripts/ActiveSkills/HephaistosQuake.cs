using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HephaistosQuake : MonoBehaviour
{
    public LayerMask enemyLayer;

    [Space(10)]
    [Header("Game Design Values")]
    [Tooltip("The damage per tick in every interval")]
    [Min(0)]
    [SerializeField]
    private float _damagePerInterval = 1f;

    [Tooltip("The time between the damage ticks")]
    [Min(0)]
    [SerializeField]
    private float _damageIntervalSeconds = 1f;

    [Tooltip("The Percentage for the default movement speed")]
    [Min(0)]
    [SerializeField]
    private float _slowPercentage = 0.5f;

    [Tooltip("The time how long the skill is active")]
    [Min(0)]
    [SerializeField]
    private float _quakeDuration = 5f;

    [Tooltip("The radius for the skill")]
    [Min(0)]
    [SerializeField]
    private float _quakeRadius = 50f;

    [Tooltip("The time you have to wait before you can use the skill again")]
    [Min(0)]
    [SerializeField]
    private float _cooldownTime = 30f;

    [Tooltip("The intensity of the camera shake")]
    [Min(0)]
    [SerializeField]
    private float _cameraShakeMagnitude = 0.1f;

    //private int skillLevel;
    //private float levelModifikatorDamage;
    //private float levelModifikatorRadius;
    //private float levelModifikatorDuration;
    //private float levelModifikatorCooldown;

    private float lastUseTime = -Mathf.Infinity;
    private bool isReady = false;

    private CameraShake _cameraShake;

    private void Awake()
    {
        _cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    private void Update()
    {
        if (Time.timeScale != 1) return;

        if (UIManager.Instance.hephaistosSkillCooldown != null)
        {
            float remainingTime = Mathf.Max(0, lastUseTime + _cooldownTime - Time.time);
            UIManager.Instance.hephaistosSkillCooldown.text = remainingTime > 0 ? $"{remainingTime:F1}s" : "Ready";
        }

        if (isReady)
        {
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ActivateQuake();
            }
        }

        if (Time.time >= lastUseTime + _cooldownTime)
        {
            isReady = true;
        }
    }

    public void ActivateQuake()
    {
        if (!isReady) return;

        if (_cameraShake != null)
        {
            StartCoroutine(_cameraShake.Shake(_quakeDuration, _cameraShakeMagnitude));
        }

        StartCoroutine(HephaitosQuakeDamageOverTime());

        lastUseTime = Time.time;
        isReady = false;
    }

    private IEnumerator HephaitosQuakeDamageOverTime()
    {
        float elapsedTime = 0f;
        while (elapsedTime <= _quakeDuration)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(Vector3.zero, _quakeRadius, enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.TryGetComponent(out EnemyManager enemyManager))
                {
                    enemyManager.TakeDamage(_damagePerInterval);
                }

                if (enemy.TryGetComponent(out EnemyPathfinding enemyPathfinding))
                {
                    enemyPathfinding.SlowMovement(_slowPercentage, _damageIntervalSeconds);
                }
            }

            elapsedTime += _damageIntervalSeconds;
            yield return new WaitForSeconds(_damageIntervalSeconds);
        }
    }
}
