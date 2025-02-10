using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    //private void Awake()
    //{
    //    // Verhindert, dass die Kamera und ihre Komponenten bei einem Szenenwechsel zerstört werden
    //    DontDestroyOnLoad(gameObject);

    //    // Prüfe, ob es bereits einen AudioListener in der Szene gibt und entferne den alten
    //    if (FindObjectsOfType<AudioListener>().Length > 1)
    //    {
    //        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
    //        foreach (var listener in listeners)
    //        {
    //            if (listener.gameObject != gameObject) // Der aktuelle AudioListener bleibt erhalten
    //            {
    //                Destroy(listener.gameObject); // Entferne den alten AudioListener
    //            }
    //        }
    //    }
    //}

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        transform.localPosition = originalPosition;
    }
}
