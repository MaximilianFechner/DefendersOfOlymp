using UnityEngine;
using System.Collections;

public class BloodPools : MonoBehaviour
{
    private BloodManager bloodManager;
    private SpriteRenderer spriteRenderer;

    [Header("Game Design Values")]
    [Tooltip("How fast/slow the corpse fade out when the max corpses value is reached")]
    [Min(1)]
    [SerializeField]
    private float fadeOutDuration = 5f;

    private void Awake()
    {
        bloodManager = FindFirstObjectByType<BloodManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        bloodManager.bloodQueue.Enqueue(this.gameObject);
    }

    public void StartFadeAndDestroy()
    {
        StartCoroutine(FadeOutEnd());
    }

    private IEnumerator FadeOutEnd()
    {
        float elapsedTime = 0f;

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer fehlt. Das Blut-Objekt kann nicht ausgeblendet werden.");
            yield break;
        }

        Color originalColor = spriteRenderer.color;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);

            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }

        Destroy(gameObject);
    }
}
