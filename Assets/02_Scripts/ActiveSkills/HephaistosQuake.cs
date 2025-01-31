using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class HephaistosQuake : MonoBehaviour
{
    public LayerMask enemyLayer;

    [Space(10)]
    [Header("Game Design Values")]
    //[Tooltip("The damage per tick in every interval")]
    //[Min(0)]
    //[SerializeField]
    //private float _damagePerInterval = 1f;

    [Tooltip("The minimum damage the ability does per interval")]
    [Min(0)]
    [SerializeField]
    public float damageLowerLimitPerInterval = 1f;

    [Tooltip("The maximum damage the ability does per interval")]
    [Min(0)]
    [SerializeField]
    public float damageUpperLimitPerInterval = 5f;

    [Tooltip("The time between the damage ticks")]
    [Min(0)]
    [SerializeField]
    public float _damageIntervalSeconds = 1f;

    [Tooltip("The Percentage for the default movement speed")]
    [Min(0)]
    [SerializeField]
    public float _slowPercentage = 0.5f;

    [Tooltip("The time how long the skill is active")]
    [Min(0)]
    [SerializeField]
    public float _quakeDuration = 5f;

    [Tooltip("The radius for the skill")]
    [Min(0)]
    [SerializeField]
    private float _quakeRadius = 50f;

    [Tooltip("The time you have to wait before you can use the skill again")]
    [Min(0)]
    [SerializeField]
    public float _cooldownTime = 30f;

    [Tooltip("The intensity of the camera shake")]
    [Min(0)]
    [SerializeField]
    private float _cameraShakeMagnitude = 0.1f;

    [Space(10)]

    [Tooltip("Minimum volume for the enemy sounds")]
    [Range(0, 1)]
    [SerializeField]
    private float minVolumeSounds = 0.1f;

    [Tooltip("Maximum volume for the enemy sounds")]
    [Range(0, 1)]
    [SerializeField]
    private float maxVolumeSounds = 0.35f;

    [Tooltip("Minimum pitch for the enemy sounds")]
    [Range(-3, 3)]
    [SerializeField]
    private float minPitchSounds = 0.8f;

    [Tooltip("Maximum pitch for the enemy sounds")]
    [Range(-3, 3)]
    [SerializeField]
    private float maxPitchSounds = 1f;

    public AudioClip skillSound;

    //private int skillLevel;
    //private float levelModifikatorDamage;
    //private float levelModifikatorRadius;
    //private float levelModifikatorDuration;
    //private float levelModifikatorCooldown;

    private float lastUseTime = -Mathf.Infinity;
    private bool isReady = false;

    private float remainingCooldownTime = 0f;

    private CameraShake _cameraShake;

    [HideInInspector] public int hephaistosSkillLevel = 1;

    private void Start()
    {
        AssignCameraShake();
        _cameraShake = Camera.main.GetComponent<CameraShake>();
        //_cameraShake = FindFirstObjectByType<CameraShake>();
    }

    private void AssignCameraShake()
    {
        if (_cameraShake == null)
        {
            _cameraShake = FindFirstObjectByType<CameraShake>();

            if (_cameraShake == null)
            {
                Debug.LogError("CameraShake not found in the scene!");
            }
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;

        if (GameManager.Instance.isInWave)
        {
            if (remainingCooldownTime > 0)
            {
                remainingCooldownTime -= Time.deltaTime;
                if (remainingCooldownTime <= 0)
                {
                    remainingCooldownTime = 0;
                    UIManager.Instance.hephaistosSkillCooldown.text = "Quake";
                }
                else
                {
                    UIManager.Instance.hephaistosSkillCooldown.text = $"{remainingCooldownTime:F1}s";
                }
            }
        }

        //if (UIManager.Instance.hephaistosSkillCooldown != null)
        //{
        //    float remainingTime = Mathf.Max(0, lastUseTime + _cooldownTime - Time.time);
        //    UIManager.Instance.hephaistosSkillCooldown.text = remainingTime > 0 ? $"{remainingTime:F1}s" : "Quake";
        //}

        if (isReady && GameManager.Instance.isInWave)
        {
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ActivateQuake();
            }
        }

        if (remainingCooldownTime <= 0 && GameManager.Instance.isInWave) //if (Time.time >= lastUseTime + _cooldownTime)
        {
            isReady = true;
        }
    }


    public void ActivateQuake()
    {
        if (Time.timeScale == 0) return;
        if (!isReady) return;

        if (_cameraShake == null)
        {
            _cameraShake = FindFirstObjectByType<CameraShake>();
        }

        if (_cameraShake != null)
        {
            StartCoroutine(_cameraShake.Shake(_quakeDuration, _cameraShakeMagnitude));
        }
        else
        {
            Debug.LogError("CameraShake is NULL!");
        }

        StartCoroutine(HephaitosQuakeDamageOverTime());
        PlaySoundOnTempGameObject(skillSound);

        remainingCooldownTime = _cooldownTime;
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
                if (enemy.isTrigger)
                {
                    if (enemy.TryGetComponent(out EnemyManager enemyManager))
                    {
                        enemyManager.TakeDamage(Mathf.RoundToInt(Random.Range(damageLowerLimitPerInterval, damageUpperLimitPerInterval)));
                    }

                    if (enemy.TryGetComponent(out EnemyPathfinding enemyPathfinding))
                    {
                        enemyPathfinding.SlowMovement(_slowPercentage, _damageIntervalSeconds);
                    }
                }

            }

            elapsedTime += _damageIntervalSeconds;
            yield return new WaitForSeconds(_damageIntervalSeconds);
        }
    }

    private void PlaySoundOnTempGameObject(AudioClip clip)
    {
        GameObject soundObject = new GameObject("QuakeSound");
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();

        tempAudioSource.clip = clip;
        tempAudioSource.ignoreListenerPause = true;
        tempAudioSource.volume = Random.Range(minVolumeSounds, maxVolumeSounds);
        tempAudioSource.pitch = Random.Range(minPitchSounds, maxPitchSounds);

        tempAudioSource.Play();

        Destroy(soundObject, clip.length);
    }
}
