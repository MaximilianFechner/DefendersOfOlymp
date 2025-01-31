using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyPathfinding : MonoBehaviour
{
    private GameObject _player;
    private NavMeshAgent _agent;

    [SerializeField] private float _defaultSpeed;
    private float _currentSlowValue = 1f; // 1f = kein Slow
    [SerializeField] private float _timeSlowed;
    [SerializeField] private bool _isSlowed;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("EnemyTarget");
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _defaultSpeed = _agent.speed;
    }

    void Update()
    {
        _agent.SetDestination(_player.transform.position);

        if (_isSlowed)
        {
            if (_timeSlowed > 0)
            {
                _timeSlowed -= Time.deltaTime;
            }
            else
            {
                ResetSpeed();
            }
        }
    }

    void LateUpdate()
    {
        if (Mathf.Abs(transform.position.z) > 0.01f)
        {
            Vector3 pos = transform.position;
            pos.z = 0;
            transform.position = pos;
        }
    }

    public void SlowMovement(float slowValue, float timeSlowed)
    {
        if (!_isSlowed || slowValue < _currentSlowValue) // if not slowed or if the new slowValue is stronger than the first one
        {
            _currentSlowValue = slowValue;
            _timeSlowed = timeSlowed;
            _agent.speed = _defaultSpeed * slowValue;
            _isSlowed = true;
          
            StopAllCoroutines();
            StartCoroutine(ResetSpeedAfterTime(timeSlowed)); //reset to default speed after slowtime
        }
    }

    private IEnumerator ResetSpeedAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ResetSpeed();
    }

    private void ResetSpeed()
    {
        _agent.speed = _defaultSpeed;
        _isSlowed = false;
        _currentSlowValue = 1f; // Kein Slow mehr
    }
}
