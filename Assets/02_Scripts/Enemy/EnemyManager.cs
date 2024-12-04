using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EnemyHealthBar enemyHealthBar;

    [Header("Game Design Values")]
    [Tooltip("The maximum hp for the enemy")]
    [Min(1)]
    [SerializeField] 
    private float _maxHP = 50f; // default value

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

    private AudioSource audioSource;

    [Space(10)]
    public AudioClip[] enemySounds;

    private float _currentHP;
    private bool _isAlive = true;
    private float nextSoundAvailable = 0f;

    void Start()
    {
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        if (enemyHealthBar == null)
        {
            return;
        }
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
        _currentHP -= damage;
        HitAndDieSound();
        UpdateHealthBar();
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
            GameManager.Instance.AddEnemyKilled();
            GameManager.Instance.SubRemainingEnemy();
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

    private void HitAndDieSound()
    {
        int randomNumber = Random.Range(1, 100);
        if (randomNumber > _chanceToPlaySound) return;

        if (_isAlive && Time.time >= nextSoundAvailable)
        {
            PlaySoundOnTempGameObject(enemySounds[Random.Range(0, enemySounds.Length)]);
            nextSoundAvailable = Time.time + soundCooldown;
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyTarget")
        {
            GameManager.Instance.LoseLife(_playerDamage);
            GameManager.Instance.SubRemainingEnemy();
            Destroy(this.gameObject, 3f);
        }
    }
}