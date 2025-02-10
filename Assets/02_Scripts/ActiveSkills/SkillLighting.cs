using UnityEngine;
using System.Collections;

public class SkillLighting : MonoBehaviour
{

    [SerializeField] private float duration = 0.73f;
    [SerializeField] private float maxAlpha = 1.0f;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer != null)
        {
            StartCoroutine("FadeSprite");
        }
    }
    public IEnumerator FadeSprite()
    {
        float halfDuration = duration / 2f;
        Color color = spriteRenderer.color;

        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, maxAlpha, t / halfDuration);
            spriteRenderer.color = color;
            yield return null;
        }

        color.a = maxAlpha;
        spriteRenderer.color = color;

        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(maxAlpha, 0, t / halfDuration);
            spriteRenderer.color = color;
            yield return null;
        }

        color.a = 0;
        spriteRenderer.color = color;
    }
}
