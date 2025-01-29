using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    private EnemyHealthBar enemyHealthBar;
    private NavMeshAgent navMeshAgent;

    [Header("Game Design Values")]
    [Tooltip("name of the enemy")]
    public string enemyName;

    [Tooltip("The maximum hp for the enemy")]
    [Min(1)]
    [SerializeField]
    public float _maxHP = 50f; // default value

    [Tooltip("Add extra absolute HP for this enemy for every wave")]
    [Min(0)]
    [SerializeField]
    private float absoluteHPIncreaseWave = 0f;

    [Tooltip("Add extra prozentual HP for this enemy for every wave")]
    [Min(0)]
    [SerializeField]
    private float prozentualHPIncreaseWave = 0f;


    [Tooltip("Add extra absolute Speed for this enemy for every wave")]
    [Min(0)]
    [SerializeField]
    private float absoluteSpeedIncreaseWave = 0f;

    [Tooltip("Add extra prozentual Speed for this enemy for every wave")]
    [Min(0)]
    [SerializeField]
    private float prozentualSpeedIncreaseWave = 0f;

    [Tooltip("The damage the enemy did on the player when he reached the target/goal")]
    [Min(1)]
    [SerializeField] 
    private int _playerDamage = 1; // default value

    [Header("Game Design Values: Sounds")]
    [Tooltip("Chance to play the enemy sound when the enemy get a hit or dies")]
    [Min(1)]
    [SerializeField]
    private int _chanceToPlaySound = 20;

    [Tooltip("The Waiting-Time in seconds before the enemy can play the sound again")]
    [Min(1)]
    [SerializeField]
    private float soundCooldown = 3f;

    [Tooltip("Minimum volume for the enemy sounds")]
    [Range(0,1)]
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

    [Header("Game Design Values: Blood")]
    [Tooltip("Chance to spawn blood on enemies hit")]
    [Range(0, 1)]
    [SerializeField]
    private float bloodSpawnChance = 0.1f;

    [Tooltip("Minimum scale for spawned blood")]
    [Min(0)]
    [SerializeField]
    private float minBloodScale = 0.8f;

    [Tooltip("Maximum scale for spawned blood")]
    [Min(0)]
    [SerializeField]
    private float maxBloodScale = 1f;

    private float bloodSpawnRadius = 1.5f;

    private AudioSource audioSource;

    [Space(10)]
    public AudioClip[] enemySounds;
    public AudioClip[] deathSounds;

    [Space(10)]
    public GameObject[] bloodPrefabs;


    [HideInInspector] public float _currentHP;

    private bool _isAlive = true;
    private float nextSoundAvailable = 0f;

    public GameObject deathPrefab;
    public GameObject bloodParticlePrefab;

    public GameObject damageTextPrefab;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Speed Increase per Wave
        prozentualSpeedIncreaseWave = ((navMeshAgent.speed / 100) * prozentualSpeedIncreaseWave) * GameManager.Instance.waveNumber;
        absoluteSpeedIncreaseWave *= GameManager.Instance.waveNumber;
        navMeshAgent.speed += (absoluteSpeedIncreaseWave + prozentualSpeedIncreaseWave);
    }

    void Start()
    {
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        if (enemyHealthBar == null)
        {
            return;
        }

        // HP Increase per Wave
        prozentualHPIncreaseWave = ((_maxHP / 100) * prozentualHPIncreaseWave) * GameManager.Instance.waveNumber;
        absoluteHPIncreaseWave *= GameManager.Instance.waveNumber;
        _maxHP += Mathf.RoundToInt(absoluteHPIncreaseWave + prozentualHPIncreaseWave); //auf eine Ganzzahl runden
        _currentHP = _maxHP;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (enemyHealthBar == null)
        {
            return;
        }

        if (_currentHP <= 0 || _currentHP == _maxHP)
        {
            enemyHealthBar.SetVisible(false);
        }
        else
        {
            enemyHealthBar.SetVisible(true);
        }
    }
    public void TakeDamage(float damage)
    {
        if (!_isAlive) return; // avoid damage on dead enemies

        bool isCrit = Random.Range(0,100) <= GameManager.Instance.critChance;

        if (isCrit)
        {
            damage *= 2;
        }

        _currentHP -= damage;

        if (GameManager.Instance.showDamageNumbers) ShowDamageText(damage, isCrit);

        if (Random.value <= bloodSpawnChance)
        {
            SpawnBlood();
        }

        HitSound();
        UpdateHealthBar();
        Instantiate(bloodParticlePrefab, this.transform.position, Quaternion.identity);

        if (_currentHP <= 0 && _isAlive)
        {
            Die();
        }
    }

    public void Die()
    {
        if (_isAlive)
        {
            _isAlive = false;

            if (this.gameObject.name == "Centaur(Clone)") GameManager.Instance.centaurKills++;
            if (this.gameObject.name == "Cerberus(Clone)") GameManager.Instance.cerberusKills++;
            if (this.gameObject.name == "Cyclop(Clone)") GameManager.Instance.cyclopKills++;

            GameManager.Instance.AddEnemyKilled();
            GameManager.Instance.SubRemainingEnemy();
            DieSound();

            if (deathPrefab != null)
            {
                Instantiate(deathPrefab, this.gameObject.transform.position, Quaternion.identity);
                SpawnBloodPool();
            }

            Destroy(this.gameObject);

        }
    }

    public void UpdateHealthBar()
    {
        float healthPercentage = _currentHP / _maxHP;
        enemyHealthBar.SetHealth(healthPercentage);
    }

    public float GetCurrentHP() {
        return _currentHP; 
    }

    private void HitSound()
    {
        int randomNumber = Random.Range(1, 100);
        if (randomNumber > _chanceToPlaySound) return;

        if (_isAlive && Time.time >= nextSoundAvailable)
        {
            PlaySoundOnTempGameObject(enemySounds[Random.Range(0, enemySounds.Length)]);
            nextSoundAvailable = Time.time + soundCooldown;
        }
    }
    private void DieSound()
    {
        PlaySoundOnTempGameObject(deathSounds[Random.Range(0, enemySounds.Length)]);
    }

    private void PlaySoundOnTempGameObject(AudioClip clip)
    {
        GameObject soundObject = new GameObject("TemporarySound");
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();

        tempAudioSource.clip = clip;
        tempAudioSource.ignoreListenerPause = true;
        tempAudioSource.volume = Random.Range(minVolumeSounds, maxVolumeSounds);
        tempAudioSource.pitch = Random.Range(minPitchSounds, maxPitchSounds);

        tempAudioSource.Play();

        Destroy(soundObject, clip.length);
    }

    private void SpawnBlood()
    {
        Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * bloodSpawnRadius;
        GameObject blood = Instantiate(bloodPrefabs[Random.Range(0, bloodPrefabs.Length)], 
            randomPosition, Quaternion.Euler(0, 0, Random.Range(-25f, 25f)));
        blood.transform.localScale = new Vector2(Random.Range(minBloodScale, maxBloodScale), Random.Range(minBloodScale, maxBloodScale));
    }
    public void SpawnBloodPool()
    {
        if (this.gameObject.name == "Centaur(Clone)")
        {
            GameObject bloodPool = Instantiate(bloodPrefabs[1],
                new Vector3(this.transform.position.x, this.transform.position.y - 2f, 0), Quaternion.Euler(0, 0, Random.Range(-25f, 25f)));

            bloodPool.transform.localScale = new Vector2(0.3f, 0.3f);
            bloodPool.AddComponent<BloodPoolGrowth>().Initialize(new Vector2(0.3f, 0.3f),
                new Vector2(Random.Range(1.65f, 1.9f), Random.Range(1.65f, 1.9f)), Random.Range(5f, 10f));
        }

        else if (this.gameObject.name == "Cerberus(Clone)")
        {
            GameObject bloodPool = Instantiate(bloodPrefabs[1],
                new Vector3(this.transform.position.x, this.transform.position.y - 2f, 0), Quaternion.Euler(0, 0, Random.Range(-25f, 25f)));

            bloodPool.transform.localScale = new Vector2(0.3f, 0.3f);
            bloodPool.AddComponent<BloodPoolGrowth>().Initialize(new Vector2(0.3f, 0.3f), 
                new Vector2(Random.Range(1.45f, 1.7f), Random.Range(1.45f, 1.7f)), Random.Range(5f, 10f));
        }

        else
        {
            GameObject bloodPool = Instantiate(bloodPrefabs[1],
                new Vector3(this.transform.position.x, this.transform.position.y - 2.5f, 0), Quaternion.Euler(0, 0, Random.Range(-25f, 25f)));

            bloodPool.transform.localScale = new Vector2(0.3f, 0.3f);
            bloodPool.AddComponent<BloodPoolGrowth>().Initialize(new Vector2(0.3f, 0.3f), 
                new Vector2(Random.Range(1.9f, 2.3f), Random.Range(1.9f, 2.3f)), Random.Range(5f, 10f));
        }

        //Instantiate(bloodPrefabs[Random.Range(0, bloodPrefabs.Length)]
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyTarget")
        {
            GameManager.Instance.LoseLife(_playerDamage);
            GameManager.Instance.SubRemainingEnemy();
            Destroy(this.gameObject, 3f);
        }
    }

    private void ShowDamageText(float damage, bool isCrit)
    {
        float randomXOffset = Random.Range(-1.5f, 1.5f);
        float randomYOffset = Random.Range(0.2f, 0.8f);

        Vector3 spawnPosition = transform.position + new Vector3(randomXOffset, 1.5f + randomYOffset, 0);

        GameObject damageTextInstance = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity);

        Text textComponent = damageTextInstance.GetComponent<Text>();
        textComponent.text = damage.ToString();

        if (isCrit)
        {
            textComponent.color = new Color(255f / 255f, 210f / 255f, 0f / 255f);
            textComponent.fontSize += 4;
        }
    }
}