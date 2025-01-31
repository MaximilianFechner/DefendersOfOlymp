using UnityEngine;

public class UIParticlesystem : MonoBehaviour
{
    public RectTransform karte;
    public ParticleSystem partikelSystem;

    private Camera kamera;

    void Start()
    {
        kamera = Camera.main;
    }

    void Update()
    {
        Vector3 kartenPositionUI = karte.position;
        Vector3 kartenPositionViewport = kamera.ScreenToViewportPoint(kartenPositionUI);
        Vector3 kartenPositionWelt = kamera.ViewportToWorldPoint(kartenPositionViewport);

        partikelSystem.transform.position = new Vector3(
            kartenPositionWelt.x,
            kartenPositionWelt.y + 7.5f,
            0f // Z-Koordinate immer auf 0 setzen
        );
    }
}


