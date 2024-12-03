using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    private List<Cards> AvailableCards;
    private Cards currentCard;

    [Header("UI Elements")]
    public Image CardDisplay;
    public Button drawCardButton;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GetCurrentCard() != null)
            {
                PlaceTower();
            }
        }
    }

    public void DrawCard()
    {
        if (AvailableCards.Count == 0) return;

        int randomIndex = Random.Range(0, AvailableCards.Count);
        currentCard = AvailableCards[randomIndex];
        CardDisplay.sprite = currentCard.CardSprite;
        CardDisplay.gameObject.SetActive(true);
        drawCardButton.gameObject.SetActive(false);

    }

    public Cards GetCurrentCard()
    {
        return currentCard;
    }

    public void ClearCard()
    {
        currentCard = null;
        CardDisplay.gameObject.SetActive(false);
    }


    void PlaceTower()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Z-Achse auf 0 setzen für 2D

        Cards selectedCard = GetCurrentCard();
        Instantiate(selectedCard.TowerPrefab, mousePosition, Quaternion.identity);

        ClearCard();
    }
}

