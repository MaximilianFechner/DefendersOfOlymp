using UnityEngine;
using System.Collections;

public class Hermes : MonoBehaviour
{
    public Vector3 hiddenPosition; // pos down
    public Vector3 visiblePosition; // target pos
    public float moveSpeed = 5f; // move speed
    public GameObject speechBubble;

    private void Start()
    {
        transform.localPosition = hiddenPosition; // starts visible

        if (speechBubble != null)
        {
            speechBubble.SetActive(false);
        }
    }

    public void ShowHermes()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(visiblePosition, true));
    }

    public void HideHermes()
    {
        StopAllCoroutines();
        speechBubble.SetActive(false);
        StartCoroutine(MoveToPosition(hiddenPosition, false));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, bool showBubble)
    {
        float activationThreshold = 50f; // Abstand, bei dem die Sprechblase erscheint

        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

            // Sprechblase aktivieren, wenn Hermes fast an der sichtbaren Position ist
            if (showBubble && Vector3.Distance(transform.localPosition, visiblePosition) < activationThreshold)
            {
                speechBubble.SetActive(true);
            }

            yield return null;
        }

        transform.localPosition = targetPosition;

        // Sprechblase ausblenden, wenn Hermes zur Hidden-Position fährt
        if (!showBubble)
        {
            speechBubble.SetActive(false);
        }
    }
}
