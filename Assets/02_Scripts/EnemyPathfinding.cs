using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour
{
    private GameObject _player;
    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    void Update()
    {
        _agent.SetDestination(_player.transform.position);
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
}
