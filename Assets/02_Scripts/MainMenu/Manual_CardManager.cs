using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Manual_CardManager : MonoBehaviour
{
    [SerializeField]
    private List<Cards> _AvailableCards;
    private Cards _currentCard;

    [Header("UI Elements")]
    public Image _CardDisplay;
    public Button _drawCardButton;

    public GameObject _towerPreview;
    private GameObject _currentPreview;

    private void Update()
    {
        if (_currentPreview != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
            worldPosition.z = 0;
            _currentPreview.transform.position = worldPosition;
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (_GetCurrentCard() != null)
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                _PlaceTower();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null)
            {
                GameObject tower = hit.collider.gameObject;
                if (tower.tag.Equals("Tower"))
                {
                    BaseTower baseTower = tower.GetComponent<BaseTower>();
                    baseTower.SetTowerMenu();
                }
            }
        }
    }

    public void DrawCard()
    {
        if (_AvailableCards.Count == 0) return;

        int randomIndex = Random.Range(0, _AvailableCards.Count);
        _currentCard = _AvailableCards[randomIndex];
        _CardDisplay.sprite = _currentCard.CardSprite;
        _CardDisplay.gameObject.SetActive(true);
        _drawCardButton.gameObject.SetActive(false);
        //UIManager.Instance.waveFinPanel.SetActive(false);

        Cards previewTower = _GetCurrentCard();
        _PlacementPreview(previewTower);

        //if (!UIManager.Instance.prepareFirstWavePanel) return;
        //UIManager.Instance.prepareFirstWavePanel.SetActive(false);
        //_drawCardButton.interactable = false;
    }

    public Cards _GetCurrentCard()
    {
        return _currentCard;
    }

    public void _ClearCard()
    {
        _currentCard = null;
        _CardDisplay.gameObject.SetActive(false);
    }


    private void _PlaceTower()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Z-Achse auf 0 setzen für 2D

        Cards selectedCard = _GetCurrentCard();
        Instantiate(selectedCard.TowerPrefab, mousePosition, Quaternion.identity);

        if (_currentPreview != null)
        {
            Destroy(_currentPreview);
            _currentPreview = null;
        }

        _ClearCard();
        GameManager.Instance.StartNextWave();
    }

    private void _PlacementPreview(Cards currentCard)
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

