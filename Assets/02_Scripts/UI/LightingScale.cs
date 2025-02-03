using Unity.VisualScripting;
using UnityEngine;

public class LightingScale : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float targetAlpha;
    private float currentAlpha;
    private float changeSpeed;

    private float targetScaleX;
    private float currentScaleX;

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

    [Header("X-Scale Settings")]
    [Tooltip("Minimale Skalierung auf der X-Achse")]
    public float minScale = 0.8f;

    [Tooltip("Maximale Skalierung auf der X-Achse")]
    public float maxScale = 1.2f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            enabled = false;
            return;
        }

        currentAlpha = spriteRenderer.color.a;
        currentScaleX = transform.localScale.x;

        SetNewTargetValues();
    }

    void Update()
    {
        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, changeSpeed * Time.deltaTime);
        Color newColor = spriteRenderer.color;
        newColor.a = currentAlpha;
        spriteRenderer.color = newColor;

        currentScaleX = Mathf.MoveTowards(currentScaleX, targetScaleX, changeSpeed * Time.deltaTime);
        transform.localScale = new Vector3(currentScaleX, transform.localScale.y, transform.localScale.z);

        if (Mathf.Approximately(currentAlpha, targetAlpha) && Mathf.Approximately(currentScaleX, targetScaleX))
        {
            SetNewTargetValues();
        }
    }

    private void SetNewTargetValues()
    {
        targetAlpha = Random.Range(minAlpha, maxAlpha);
        targetScaleX = Random.Range(minScale, maxScale);
        changeSpeed = Random.Range(minSpeed, maxSpeed);
    }
}
