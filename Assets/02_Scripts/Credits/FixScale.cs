using System.Collections.Generic;
using UnityEngine;

public class FixScale : MonoBehaviour
{
    private Vector3 originalScale;
    private Quaternion originalRotation;

    private void Awake()
    {
        // Speichere die urspr�ngliche Skalierung und Rotation
        originalScale = transform.localScale;
        originalRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        // Skalierung des Elternobjekts abrufen
        Vector3 parentScale = transform.parent != null ? transform.parent.localScale : Vector3.one;

        // Fixiere die Skalierung (nur X-Achse ber�cksichtigen)
        transform.localScale = new Vector3(
            originalScale.x * Mathf.Sign(parentScale.x), // Spiegelung auf X-Achse ber�cksichtigen
            originalScale.y,                             // Y-Skalierung bleibt unver�ndert
            originalScale.z                              // Z-Skalierung bleibt unver�ndert
        );

        // Fixiere die Rotation (zur�ck zur Originalrotation)
        transform.rotation = originalRotation;
    }
}
