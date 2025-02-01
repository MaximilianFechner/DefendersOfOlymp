using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardFlip : MonoBehaviour
{
    public RectTransform card;
    public Sprite defaultImage;
    public Image cardImage;
    private Sprite newCardSprite;
    public float duration = 1f;
    public float moveOutDuration = 0.5f;
    public Canvas canvas;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private Vector2 originalPosition;
    private Vector2 targetPosition;
    private bool isCardFlipped = false;
    public ParticleSystem psLighting;

    void Start()
    {
        originalScale = card.localScale;
        targetScale = new Vector3(1.3f, 1.3f, 1);
        originalPosition = card.anchoredPosition;
        psLighting.gameObject.SetActive(true);

        UpdateTargetPosition();
    }

    void UpdateTargetPosition()
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float adjustedWidth = (card.rect.width * 1.3f) / 2;
        float adjustedHeight = (card.rect.height * 1.3f) / 2;

        targetPosition = new Vector2(
            (canvasRect.rect.width / 2) - adjustedWidth - 5, // Abstand nach Animation
            (-canvasRect.rect.height / 2) + adjustedHeight + 5 // Abstand nach Animation
        );
    }

    public void FlipCard(Sprite sprite)
    {
        UpdateTargetPosition();
        StartCoroutine(FlipAnimation(sprite));
    }

    IEnumerator FlipAnimation(Sprite sprite)
    {
        psLighting.Stop();

        cardImage.sprite = defaultImage;
        newCardSprite = sprite;

        float time = 0;
        bool spriteChanged = false;

        Vector2 targetPosition = new Vector2(-10, 10);

        while (time < duration)
        {
            float progress = time / duration;

            float scaleFactor = Mathf.Lerp(1, 1.3f, progress);

            float scaleX = Mathf.Lerp(1, 0, progress < 0.5f ? progress * 2 : (1 - progress) * 2);

            Vector2 newPosition = Vector2.Lerp(originalPosition, targetPosition, Mathf.SmoothStep(0, 1, progress));
            card.anchoredPosition = newPosition;

            card.localScale = new Vector3(scaleX * scaleFactor, scaleFactor, scaleFactor);

            if (!spriteChanged && progress >= 0.5f)
            {
                cardImage.sprite = newCardSprite;
                spriteChanged = true;
            }

            time += Time.deltaTime;
            yield return null;
        }

        card.anchoredPosition = targetPosition;
        card.localScale = targetScale; // Skalierung bleibt auf 1.3
        isCardFlipped = true;

        yield return new WaitForSeconds(5);
        psLighting.gameObject.SetActive(false);
    }

    public void MoveCardOut()
    {
        if (isCardFlipped)
        {
            StartCoroutine(MoveOutAnimation());
        }
    }

    IEnumerator MoveOutAnimation()
    {
        float time = 0;
        Vector2 startPosition = card.anchoredPosition;
        Vector2 endPosition = startPosition + new Vector2(500, 0);

        while (time < moveOutDuration)
        {
            float progress = time / moveOutDuration;

            card.anchoredPosition = Vector2.Lerp(startPosition, endPosition, Mathf.SmoothStep(0, 1, progress));

            time += Time.deltaTime;
            yield return null;
        }

        card.anchoredPosition = endPosition;

    }

    public void SetNewCard()
    {
        card.rotation = Quaternion.identity;
    }
}