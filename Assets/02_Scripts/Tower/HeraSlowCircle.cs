using System.Threading;
using UnityEngine;

public class HeraSlowCircle : MonoBehaviour
{

    [SerializeField] public float slowValue;
    [SerializeField] public float timeSlowed;
    [SerializeField] private float maxLifeSpan;
    private float timer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > maxLifeSpan) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag.Equals("Enemy")) {
            SlowCalculation(collider.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag.Equals("Enemy")) {
            SlowCalculation(collider.gameObject);
        }
    }

    public void SlowCalculation(GameObject enemy) {
        EnemyPathfinding enemyPathfinding = enemy.GetComponent<EnemyPathfinding>();
        enemyPathfinding.SlowMovement(slowValue, timeSlowed);
    }
}
