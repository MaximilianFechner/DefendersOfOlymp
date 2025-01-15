using UnityEngine;

public class BloodPoolGrowth : MonoBehaviour
{
    private Vector2 initialScale;
    private Vector2 maxScale;
    private float growTime;

    private float elapsedTime;

    public void Initialize(Vector2 initialScale, Vector2 maxScale, float growTime)
    {
        this.initialScale = initialScale;
        this.maxScale = maxScale;
        this.growTime = growTime;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float progress = Mathf.Clamp01(elapsedTime / growTime);
        transform.localScale = Vector2.Lerp(initialScale, maxScale, progress);

        if (progress >= 1f)
        {
            Destroy(this);
        }
    }
}
