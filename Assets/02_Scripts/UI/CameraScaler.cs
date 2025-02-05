using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public Camera mainCamera; // Die Kamera, die du skalieren m�chtest
    public float baseWidth = 1920f; // Referenz-Bildschirmbreite
    public float baseHeight = 1080f; // Referenz-Bildschirmh�he
    public float mapWidth = 200f; // Die tats�chliche Breite deiner Map in Unity-Einheiten
    public float mapHeight = 100f;

    void Start()
    {
        AdjustCamera();
    }

    private void Update()
    {
        AdjustCamera();
    }

    void AdjustCamera()
    {
        float screenRatio = (float)Screen.width / Screen.height;
        float baseRatio = baseWidth / baseHeight;

        // Berechne den Skalierungsfaktor f�r die Orthographic Size basierend auf der Kartenh�he
        float mapRatio = mapWidth / mapHeight;

        if (screenRatio >= baseRatio)
        {
            // Falls der Bildschirm breiter ist, den Zoom basierend auf der Kartenh�he anpassen
            mainCamera.orthographicSize = mapHeight / 2;
        }
        else
        {
            // Falls der Bildschirm h�her ist, den Zoom basierend auf der Kartenbreite anpassen
            mainCamera.orthographicSize = (mapWidth / 2) / screenRatio;
        }
    }
}
