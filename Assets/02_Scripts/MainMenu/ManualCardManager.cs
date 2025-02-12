using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ManualCardManager : MonoBehaviour
{
    [SerializeField] private List<Cards> _availableCards;
    private Cards _currentCard;

    [Header("UI Elements")]
    [SerializeField] private Image _cardDisplay;
    [SerializeField] private Button _drawCardButton;

    [SerializeField] private GameObject _towerPreview;
    [SerializeField] private GameObject _currentPreview;

    private void Update()
    {
        if (_currentPreview != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            if (Camera.main != null)
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
                worldPosition.z = 0;
                _currentPreview.transform.position = worldPosition;
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (GetCurrentCard() != null)
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                PlaceTower();
            }
        }

        /* Auskommentiert, da TowerMenu jetzt über ein Tooltip funktioniert
        if (Input.GetMouseButtonDown(1))
        {
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                if (hit.collider != null)
                {
                    GameObject tower = hit.collider.gameObject;
                    if (tower.tag.Equals("Tower"))
                    {
                        BaseTower baseTower = tower.GetComponent<BaseTower>();
                    //    baseTower.SetTowerMenu();
                    }
                }
            }
        } 
        */
    }

    public void DrawCard()
    {
        if (_availableCards.Count == 0) return;

        int randomIndex = Random.Range(0, _availableCards.Count);
        _currentCard = _availableCards[randomIndex];
        _cardDisplay.sprite = _currentCard.CardSprite;
        _cardDisplay.gameObject.SetActive(true);
        _drawCardButton.gameObject.SetActive(false);
        //UIManager.Instance.waveFinPanel.SetActive(false);

        Cards previewTower = GetCurrentCard();
        PlacementPreview(previewTower);

        //if (!UIManager.Instance.prepareFirstWavePanel) return;
        //UIManager.Instance.prepareFirstWavePanel.SetActive(false);
        //_drawCardButton.interactable = false;
    }

    public Cards GetCurrentCard()
    {
        return _currentCard;
    }

    public void ClearCard()
    {
        _currentCard = null;
        _cardDisplay.gameObject.SetActive(false);
    }


    private void PlaceTower()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Z-Achse auf 0 setzen fï¿½r 2D

        Cards selectedCard = GetCurrentCard();
        Instantiate(selectedCard.TowerPrefab, mousePosition, Quaternion.identity);
        Debug.Log("Manual_CardManager");
        if (_currentPreview != null)
        {
            Destroy(_currentPreview);
            _currentPreview = null;
        }

        ClearCard();
        GameManager.Instance.StartNextWave();
    }

    private void PlacementPreview(Cards currentCard)
    {
        if (currentCard == null) return;

        if (_currentPreview == null)
        {
            _currentPreview = Instantiate(currentCard.TowerPrefab);
            _currentPreview.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);

            // deactivate the ability of the preview to attack the enemies as preview tower
            MonoBehaviour[] scripts = _currentPreview.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
        }
    }
}

