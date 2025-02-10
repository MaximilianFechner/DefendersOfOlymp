using UnityEngine;

public class LightingTransparency : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer fehlt auf dem GameObject!");
            enabled = false;
            return;
        }

        currentAlpha = spriteRenderer.color.a;

        SetNewTargetAlpha();
    }

    void Update()
    {
        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, changeSpeed * Time.deltaTime);

        Color newColor = spriteRenderer.color;
        newColor.a = currentAlpha;
        spriteRenderer.color = newColor;

        if (Mathf.Approximately(currentAlpha, targetAlpha))
        {
            SetNewTargetAlpha();
        }
    }

    private void SetNewTargetAlpha()
    {
        targetAlpha = Random.Range(minAlpha, maxAlpha);
        changeSpeed = Random.Range(minSpeed, maxSpeed);
    }
}
