using UnityEngine;
using UnityEngine.SceneManagement;

public class BloodParticles : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(this.gameObject, 1f);
    }
}
