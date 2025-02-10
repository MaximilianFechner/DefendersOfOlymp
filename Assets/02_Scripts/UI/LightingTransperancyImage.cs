using UnityEngine;
using UnityEngine.UI;

public class LightingTransperancyImage : MonoBehaviour
{
    private Image image;
    private float targetAlpha;   
    private float currentAlpha;
    private float changeSpeed;

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
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("SpriteRenderer fehlt auf dem GameObject!");
            enabled = false;
            return;
        }

        // Initialisiere den Start-Alpha-Wert
        currentAlpha = image.color.a;

        // Setze das erste Ziel und die Geschwindigkeit
        SetNewTargetAlpha();
    }

    void Update()
    {
        // Glatte Interpolation des aktuellen Alpha-Werts zum Ziel-Alpha-Wert
        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, changeSpeed * Time.deltaTime);

        // Aktualisiere die Farbe des Sprites
        Color newColor = image.color;
        newColor.a = currentAlpha;
        image.color = newColor;

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
