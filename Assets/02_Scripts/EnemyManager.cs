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

    private float _currentHP; // serialize field to test in inspector
    private bool _isAlive = true;
    //[SerializeField] private float enemySpeed = 5f; // not used because NavMeshAgent

    void Start()
    {
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        if (enemyHealthBar == null)
        {
            return;
        }
        _currentHP = _maxHP;
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