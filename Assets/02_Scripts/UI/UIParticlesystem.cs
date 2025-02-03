using System.Collections.Generic;
using UnityEngine;

public class UIParticlesystem : MonoBehaviour
{
    public RectTransform card;
    public ParticleSystem psDrawCardBtn;

    private Camera camRef;

    private Vector3 startPosCard;
    private Vector3 startPosParticle;

    void Start()
    {
        camRef = Camera.main;

        //startPosCard = card.position;
        startPosParticle = psDrawCardBtn.transform.position;
    }

    void Update()
    {
        if (camRef == null)
        {
            camRef = Camera.main;
        }

        Vector3 kartenPositionUI = card.position;
        Vector3 kartenPositionViewport = camRef.ScreenToViewportPoint(kartenPositionUI);
        Vector3 kartenPositionWelt = camRef.ViewportToWorldPoint(kartenPositionViewport);

        psDrawCardBtn.transform.position = new Vector3(
            kartenPositionWelt.x - 5.0f,
            kartenPositionWelt.y + 7.5f,
            0f // z = 0
        );
    }

    public void ResetPosParticleSystem()
    {
        //card.position = startPosCard;
        this.gameObject.SetActive(true);
        psDrawCardBtn.transform.position = startPosParticle;
        camRef = Camera.main;
    }
}


