using UnityEngine;
using System.Collections;

public class WaveLighting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float targetAlpha;
    private float currentAlpha;
    private float changeSpeed;
    private bool isFadingOut = false;

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

    [Tooltip("Gesamtdauer des Effekts")]
    public float effectDuration = 5f;

    [Tooltip("Dauer des Ausfadens")]
    public float fadeOutDuration = 0.5f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer fehlt auf dem GameObject!");
            enabled = false;
            return;
        }

        currentAlpha = spriteRenderer.color.a;
        StartCoroutine(LightEffectRoutine());
    }

    private IEnumerator LightEffectRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < effectDuration - fadeOutDuration)
        {
            if (!isFadingOut)
            {
                SetNewTargetAlpha();
            }

            float stepTime = 0f;
            while (stepTime < changeSpeed && elapsedTime < effectDuration - fadeOutDuration)
            {
                currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, (changeSpeed * Time.deltaTime));
                Color newColor = spriteRenderer.color;
                newColor.a = currentAlpha;
                spriteRenderer.color = newColor;

                stepTime += Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        isFadingOut = true;
        float startAlpha = currentAlpha;
        float fadeTime = 0f;

        while (fadeTime < fadeOutDuration)
        {
            currentAlpha = Mathf.Lerp(startAlpha, 0, fadeTime / fadeOutDuration);
            Color newColor = spriteRenderer.color;
            newColor.a = currentAlpha;
            spriteRenderer.color = newColor;

            fadeTime += Time.deltaTime;
            yield return null;
        }

        Color finalColor = spriteRenderer.color;
        finalColor.a = 0;
        spriteRenderer.color = finalColor;
    }

    private void SetNewTargetAlpha()
    {
        targetAlpha = Random.Range(minAlpha, maxAlpha);
        changeSpeed = Random.Range(minSpeed, maxSpeed);
    }
}
