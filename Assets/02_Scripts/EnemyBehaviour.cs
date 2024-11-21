using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    [SerializeField] public float maxLife;
    [SerializeField] public float currentLife;
    [SerializeField] public float velocity = 5f;

    private Rigidbody2D _rb2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLife = maxLife;
        _rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update() {

        _rb2D.AddForce(new Vector2(1,0) * velocity * Time.deltaTime);
    }

    public void LoseLife(float damage) {
        currentLife -= damage;
        if (currentLife <= 0) {
            Destroy(this.gameObject);
        }
    }

    
}
