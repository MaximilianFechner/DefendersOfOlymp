using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour
{
    private GameObject _player;
    private NavMeshAgent _agent;

    [SerializeField] private float _defaultSpeed;
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
        
        if (_isSlowed) {
            if (_timeSlowed > 0) {
                _timeSlowed -= Time.deltaTime;
            } else {
                _isSlowed = false;
                _agent.speed = _defaultSpeed;
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

    public void SlowMovement(float slowValue, float timeSlowed) {
        _timeSlowed = timeSlowed;
        if (_isSlowed) {
            //SlowValue is already added to the agent speed
            return;
        } else {
            _agent.speed = _agent.speed * slowValue;
            _isSlowed = true;
        }
    }

}
