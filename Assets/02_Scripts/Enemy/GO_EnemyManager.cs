using UnityEngine;
using UnityEngine.AI;

public class GO_EnemyManager : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private GameObject _player;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("EnemyTarget");
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    private void Update()
    {
        navMeshAgent.SetDestination(_player.transform.position);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyTarget")
        {
            Destroy(this.gameObject); //die 3 Sekunden despawn-Zeit entfernt
        }
    }
}
