using System.Collections.Generic;
using UnityEngine;

public class FixScale : MonoBehaviour
{
    private Vector3 originalScale;
    private Quaternion originalRotation;

    private void Awake()
    {
        originalScale = transform.localScale;
        originalRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        Vector3 parentScale = transform.parent != null ? transform.parent.localScale : Vector3.one;

        transform.localScale = new Vector3(
            originalScale.x * Mathf.Sign(parentScale.x),
            originalScale.y,
            originalScale.z
        );

        transform.rotation = originalRotation;
    }
}
