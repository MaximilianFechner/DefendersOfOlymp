using UnityEngine;
using System.Collections;

public class CorpseEnemy : MonoBehaviour
{
    private CorpseManager corpseManager;
    private SpriteRenderer spriteRenderer;

    [Header("Game Design Values")]
    [Tooltip("How fast/slow the corpse fade out when the max corpses value is reached")]
    [Min(1)]
    [SerializeField]
    private float fadeOutDuration = 10f;

    [Tooltip("Transparency for the laying corpse after the death animation")]
    [Min(0)]
    [SerializeField]
    private float fadeOutstart = 0.9f;

    private void Awake()
    {
        corpseManager = FindFirstObjectByType<CorpseManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        corpseManager.corpseQueue.Enqueue(this.gameObject);
        StartCoroutine(StartFadeOut());
    }

    public void StartFadeAndDestroy()
    {
        StartCoroutine(FadeOutEnd());
    }

    private IEnumerator FadeOutEnd()
    {
        float elapsedTime = 0f;
        Color originalColor = spriteRenderer != null ? spriteRenderer.color : Color.white;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(fadeOutstart, 0f, elapsedTime / fadeOutDuration);

            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }

            yield return null;
        }

        Destroy(gameObject);
    }

    private IEnumerator StartFadeOut()
    {
        float elapsedTime = 0f;

        Color originalColor = spriteRenderer != null ? spriteRenderer.color : Color.white;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, fadeOutstart, elapsedTime / fadeOutDuration);

            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }

            yield return null;
        }
    }
}
