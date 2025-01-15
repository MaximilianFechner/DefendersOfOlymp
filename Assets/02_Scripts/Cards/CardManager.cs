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

        Cards previewTower = GetCurrentCard();
        PlacementPreview(previewTower);
        AudioManager.Instance.PlayCardSFX();

        //if (!UIManager.Instance.prepareFirstWavePanel) return;
        //UIManager.Instance.prepareFirstWavePanel.SetActive(false);
        //drawCardButton.interactable = false;
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
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Z-Achse auf 0 setzen für 2D

        Cards selectedCard = GetCurrentCard();
        Instantiate(selectedCard.TowerPrefab, mousePosition, Quaternion.identity);

        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }

        ClearCard();
        AudioManager.Instance.PlayButtonSFX();
        GameManager.Instance.StartNextWave();
    }

    private void PlacementPreview(Cards currentCard)
    {
        if (currentCard == null) return;

        if (currentPreview == null)
        {
            currentPreview = Instantiate(currentCard.TowerPrefab);
            currentPreview.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);

            // deactivate the ability of the preview to attack the enemies as preview tower
            MonoBehaviour[] scripts = currentPreview.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
        }
    }
}

