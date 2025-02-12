using UnityEngine;

public class PartSysDirection : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void LateUpdate()
    {
        int numParticlesAlive = ps.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            // Berechne die Richtung des Partikels
            Vector3 direction = particles[i].velocity.normalized;

            // Bestimme den Winkel basierend auf der Flugrichtung
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Offset, um die Sprite-Ausrichtung zu korrigieren (wenn nötig)
            particles[i].rotation = angle - 90f;
        }

        ps.SetParticles(particles, numParticlesAlive);
    }
}
