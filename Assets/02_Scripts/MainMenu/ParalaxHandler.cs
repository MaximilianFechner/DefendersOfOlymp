using UnityEngine;

public class ParalaxHandler : MonoBehaviour
{
    public float parallaxStrength = 0.1f;
    public float lerpSpeed = 5f;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        float moveX = (mousePos.x / Screen.width - 0.5f) * parallaxStrength * 2f;
        //float moveY = (mousePos.y / Screen.height - 0.5f) * parallaxStrength * 2f;

        targetPosition = new Vector3(startPosition.x + moveX, startPosition.y, startPosition.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
    }
}
