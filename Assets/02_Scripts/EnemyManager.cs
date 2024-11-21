using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EnemyHealthBar enemyHealthBar;
    [SerializeField] private float _maxHP = 50f; // default value
    [SerializeField] private float _currentHP; // serialize field to test in inspector
    [SerializeField] private int _playerDamage = 1; // default value
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

        UpdateHealthBar(); // only for testing with the inspector
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
        _currentHP -= damage;
        UpdateHealthBar();
        if (_currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    public void UpdateHealthBar()
    {
        float healthPercentage = _currentHP / _maxHP;
        enemyHealthBar.SetHealth(healthPercentage);
    }

    public float GetCurrentHP() {
        return _currentHP; 
    }
}