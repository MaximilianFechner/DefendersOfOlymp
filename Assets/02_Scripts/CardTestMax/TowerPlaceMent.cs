using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    public CardManager cardManager;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (cardManager.GetCurrentCard() != null)
            {
                PlaceTower();
            }
        }
    }

    void PlaceTower()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Z-Achse auf 0 setzen für 2D

        Cards selectedCard = cardManager.GetCurrentCard();
        Instantiate(selectedCard.TowerPrefab, mousePosition, Quaternion.identity);

        cardManager.ClearCard();

    }
}
