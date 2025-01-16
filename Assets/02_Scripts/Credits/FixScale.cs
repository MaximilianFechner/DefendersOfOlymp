using System.Collections.Generic;
using UnityEngine;

public class FixScale : MonoBehaviour
{
    private Vector3 originalScale;
    private Quaternion originalRotation;

    private void Awake()
    {
        // Speichere die ursprüngliche Skalierung und Rotation
        originalScale = transform.localScale;
        originalRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        // Skalierung des Elternobjekts abrufen
        Vector3 parentScale = transform.parent != null ? transform.parent.localScale : Vector3.one;

        // Fixiere die Skalierung (nur X-Achse berücksichtigen)
        transform.localScale = new Vector3(
            originalScale.x * Mathf.Sign(parentScale.x), // Spiegelung auf X-Achse berücksichtigen
            originalScale.y,                             // Y-Skalierung bleibt unverändert
            originalScale.z                              // Z-Skalierung bleibt unverändert
        );

        // Fixiere die Rotation (zurück zur Originalrotation)
        transform.rotation = originalRotation;
    }
}
