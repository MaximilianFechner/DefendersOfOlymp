using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    private List<Cards> AvailableCards;
    private Cards currentCard;

    [Header("UI Elements")]
    public Image CardDisplay;
    public Button drawCardButton;

    public GameObject towerPreview;
    private GameObject currentPreview;

    private void Update()
    {
        if (currentPreview != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
            worldPosition.z = 0;
            currentPreview.transform.position = worldPosition;
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (GetCurrentCard() != null)
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                PlaceTower();
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null) {
                GameObject tower = hit.collider.gameObject;
                if (tower.tag.Equals("Tower")) {
                    BaseTower baseTower = tower.GetComponent<BaseTower>();
                    baseTower.SetTowerMenu();
                }
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
        //UIManager.Instance.waveFinPanel.SetActive(false);

        //TODO Der PreviewTower aka BuildingGhost, darf nicht angreifen. 
        Cards previewTower = GetCurrentCard();
        PlacedObject placedObjectPreviewTower = previewTower.TowerPrefab.GetComponentInChildren<PlacedObject>();
        if (placedObjectPreviewTower != null) {
            GridBuildingSystem.Instance.RefreshSelectedObjectType(placedObjectPreviewTower.GetPlacedObjectTypeSO());
        } else {
            Debug.LogError($"Cannot GetComponent<PlacedObject>() from previewTower {previewTower.ToString()}");
        }

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


    private void PlaceTower()
    {
        if (currentCard.TowerName.Contains("Zeus")) AudioManager.Instance.PlayTowerPlacementSFX(0);
        else if (currentCard.TowerName.Contains("Poseidon")) AudioManager.Instance.PlayTowerPlacementSFX(1);
        else if (currentCard.TowerName.Contains("Hera")) AudioManager.Instance.PlayTowerPlacementSFX(2);
        else if (currentCard.TowerName.Contains("Hephaistos")) AudioManager.Instance.PlayTowerPlacementSFX(3);

        GridBuildingSystem.Instance.PlaceTower();
        currentPreview = null;
        
        ClearCard();

        GameManager.Instance.StartNextWave();
    }

}

