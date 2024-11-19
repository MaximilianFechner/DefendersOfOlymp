using UnityEngine;
using UnityEngine.AI;

public class PathfindingTest : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        agent.SetDestination(target.position);

        //Vector3 direction = agent.velocity.normalized;
        //if (direction != Vector3.zero)
        //{
        //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.Euler(0, 0, angle);
        //}
    }
}
