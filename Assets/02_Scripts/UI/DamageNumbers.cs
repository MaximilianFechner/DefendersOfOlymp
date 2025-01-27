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
        Vector3 endPos = startPos + Vector3.up * 2f;
        Color startColor = textComponent.color;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, elapsedTime / damageNumberDuration));

            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / damageNumberDuration);
            textComponent.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
