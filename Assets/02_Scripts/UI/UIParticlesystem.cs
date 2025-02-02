using UnityEngine;

public class UIParticlesystem : MonoBehaviour
{
    public RectTransform card;
    public ParticleSystem particleSystem;

    private Camera camera;

    private Vector3 startPosCard;
    private Vector3 startPosParticle;

    void Start()
    {
        camera = Camera.main;

        //startPosCard = card.position;
        startPosParticle = particleSystem.transform.position;
    }

    void Update()
    {
        Vector3 kartenPositionUI = card.position;
        Vector3 kartenPositionViewport = camera.ScreenToViewportPoint(kartenPositionUI);
        Vector3 kartenPositionWelt = camera.ViewportToWorldPoint(kartenPositionViewport);

        particleSystem.transform.position = new Vector3(
            kartenPositionWelt.x - 5.0f,
            kartenPositionWelt.y + 7.5f,
            0f // z = 0
        );
    }

    public void ResetPosParticleSystem()
    {
        //card.position = startPosCard;
        this.gameObject.SetActive(true);
        particleSystem.transform.position = startPosParticle;
    }
}


