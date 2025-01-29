using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DamageNumbers : MonoBehaviour
{
    [Header("Game Design Values: Damage Numbers")]
    [Tooltip("How lang the damage numbers stay in the screen")]
    [Min(1)]
    [SerializeField]
    private float damageNumberDuration = 2f;

    private Text textComponent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textComponent = GetComponent<Text>();
        StartCoroutine(nameof(AnimateDamageText));
    }

    private IEnumerator AnimateDamageText()
    {
        float duration = damageNumberDuration;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * (textComponent.color == new Color(255f / 255f, 210f / 255f, 0f / 255f) ? 3.5f : 2f);
        Color startColor = textComponent.color;

        float elapsedTime = 0f;

        Vector3 normalScale = new Vector3(0.075f, 0.075f, 1f);
        Vector3 critStartScale = new Vector3(0.175f, 0.175f, 1f);

        transform.localScale = textComponent.color == new Color(255f / 255f, 210f / 255f, 0f / 255f) ? critStartScale : normalScale;

        float critScaleDuration = duration * 0.85f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, elapsedTime / duration));

            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            if (textComponent.color == new Color(255f / 255f, 210f / 255f, 0f / 255f))
            {
                float critScaleProgress = Mathf.Clamp01(elapsedTime / critScaleDuration);
                transform.localScale = Vector3.Lerp(critStartScale, normalScale, critScaleProgress);

                textComponent.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            }
            else
            {
                transform.localScale = normalScale;

                textComponent.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            }

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
