using UnityEngine;

public class LightingTransparency : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // Referenz auf den SpriteRenderer
    private float targetAlpha;            // Ziel-Alpha-Wert
    private float currentAlpha;           // Aktueller Alpha-Wert
    private float changeSpeed;            // Geschwindigkeit der Alpha-Änderung

    [Header("Transparency Settings")]
    [Tooltip("Minimale Transparenz (0 = komplett unsichtbar)")]
    [Range(0f, 1f)]
    public float minAlpha = 0.3f;

    [Tooltip("Maximale Transparenz (1 = komplett sichtbar)")]
    [Range(0f, 1f)]
    public float maxAlpha = 0.8f;

    [Tooltip("Minimaler Übergangsgeschwindigkeit")]
    public float minSpeed = 0.5f;

    [Tooltip("Maximale Übergangsgeschwindigkeit")]
    public float maxSpeed = 2f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer fehlt auf dem GameObject!");
            enabled = false;
            return;
        }

        // Initialisiere den Start-Alpha-Wert
        currentAlpha = spriteRenderer.color.a;

        // Setze das erste Ziel und die Geschwindigkeit
        SetNewTargetAlpha();
    }

    void Update()
    {
        // Glatte Interpolation des aktuellen Alpha-Werts zum Ziel-Alpha-Wert
        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, changeSpeed * Time.deltaTime);

        // Aktualisiere die Farbe des Sprites
        Color newColor = spriteRenderer.color;
        newColor.a = currentAlpha;
        spriteRenderer.color = newColor;

        // Wenn das Ziel erreicht ist, neuen Ziel-Alpha-Wert setzen
        if (Mathf.Approximately(currentAlpha, targetAlpha))
        {
            SetNewTargetAlpha();
        }
    }

    private void SetNewTargetAlpha()
    {
        // Wähle einen neuen zufälligen Alpha-Wert im Bereich [minAlpha, maxAlpha]
        targetAlpha = Random.Range(minAlpha, maxAlpha);

        // Wähle eine neue zufällige Geschwindigkeit im Bereich [minSpeed, maxSpeed]
        changeSpeed = Random.Range(minSpeed, maxSpeed);
    }
}
