using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public Button drawCardButton;
    public Image isDrawableLighting;

    public GameObject towerPreview;
    private GameObject currentPreview;

    private Vector2 buttonOriginalPosition; //MOVEBTN
    public Button drawCardBTN; //MOVEBTN
    public Animator uiAnimation; //MOVEBTN

    public CardFlip cardToFlip;

    public ZeusBolt zeusBolt;
    public PoseidonWave poseidonWave;
    public HeraStun heraStun;
    public HephaistosQuake hephaistosQuake;

    private GridCells currentTargetCell; //OwnGrid: Die aktuelle Ziel-Zelle fürs Snappen
    public GridCells[] gridCells; //OwnGrid:  Array mit allen Grid-Zellen
    public float snapSpeed = 10f; //OwnGrid: Geschwindigkeit für Smooth-Snapping

    private void Start()
    {
        buttonOriginalPosition = drawCardBTN.GetComponent<RectTransform>().anchoredPosition;
        uiAnimation = uiAnimation.GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentPreview != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
            worldPosition.z = 0;

            // Nächstgelegene Zelle finden
            GridCells nearestCell = GetNearestCell(worldPosition);
            if (nearestCell != null)
            {
                if (nearestCell != currentTargetCell)
                {
                    currentTargetCell = nearestCell;
                    HighlightCell(nearestCell);
                }
            }

            // Smooth Snapping zur Zelle
            if (currentTargetCell != null)
            {
                currentPreview.transform.position = Vector3.Lerp(
                    currentPreview.transform.position,
                    currentTargetCell.transform.position,
                    Time.deltaTime * snapSpeed);
            }
        }

        if (GameManager.Instance.isCardDrawable)
        {
            StartCoroutine(MoveButton(drawCardBTN.GetComponent<RectTransform>(), buttonOriginalPosition)); //BTN CD MOVE
            drawCardBTN.interactable = true; //BTN CD MOVE
            isDrawableLighting.enabled = true;
            cardToFlip.psLighting.gameObject.SetActive(true);

            GameManager.Instance.isCardDrawable = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (GetCurrentCard() != null)
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                PlaceTower();
            }
        }
    }

    private GridCells GetNearestCell(Vector3 position)
    {
        GridCells nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GridCells cell in gridCells)
        {
            float dist = Vector3.Distance(position, cell.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = cell;
            }
        }
        return nearest;
    }

    private void HighlightCell(GridCells cell) //method to highlight the hovered cell
    {
        foreach (GridCells cellBackground in gridCells)
        {
            if (cellBackground.isCellBuilt)
            {
                BaseTower towerInCell = cellBackground.placedTower?.GetComponentInChildren<BaseTower>();

                if (towerInCell != null && cellBackground.towerName == currentCard.TowerName)
                {
                    cellBackground.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 0.50f); ; // green if upgradeable
                }
                else
                {
                    cellBackground.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.50f); // red if not placeable
                }
            }
            else
            {
                cellBackground.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.15f); // Normale Zelle
            }
        }

        if (!cell.isCellBuilt)
        {
            cell.GetComponent<SpriteRenderer>().color = Color.white; // Nur hervorheben, wenn nicht upgradbar
        }
    }

    private void DisableCells() //method to highlight the hovered cell
    {
        StartCoroutine(FadeOutCells());
    }

    private IEnumerator FadeOutCells()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        // Speichert die aktuelle Farbe jeder Zelle
        Dictionary<GridCells, Color> initialColors = new Dictionary<GridCells, Color>();

        foreach (GridCells cell in gridCells)
        {
            initialColors[cell] = cell.GetComponent<SpriteRenderer>().color;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            foreach (GridCells cell in gridCells)
            {
                Color startColor = initialColors[cell];
                Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

                cell.GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, targetColor, t);
            }

            yield return null;
        }

        foreach (GridCells cell in gridCells)
        {
            cell.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
    }

    public void DrawCard()
    {
        if (AvailableCards.Count == 0) return;

        int randomIndex = Random.Range(0, AvailableCards.Count);
        currentCard = AvailableCards[randomIndex];

        RectTransform buttonRect = drawCardBTN.GetComponent<RectTransform>(); //MOVEBTN
        Vector2 targetPosition = buttonOriginalPosition + new Vector2(0, -200); //MOVEBTN
        StartCoroutine(MoveButton(buttonRect, targetPosition)); //MOVEBTN
        drawCardBTN.interactable = false; //MOVEBTN
        isDrawableLighting.enabled = false;

        cardToFlip.FlipCard(currentCard.CardSprite); //Cardflip Animation

        AudioManager.Instance.PlayCardSFX();

        if (currentPreview != null) Destroy(currentPreview);
        currentPreview = Instantiate(currentCard.TowerPrefab); // Preview-Turm erstellen

        Transform rangeVisual = currentPreview.GetComponentsInChildren<Transform>(true) //true sagt, dass auch deaktivierte GOs durchsucht werden
                                            .FirstOrDefault(t => t.name == "RangeVisual");

        if (rangeVisual != null)
        {
            rangeVisual.gameObject.SetActive(true);
        }

        Collider2D collider = currentPreview.GetComponentInChildren<Collider2D>(); // Deaktiviert den Collider der Preview, damit man die Tooltips von bereits
        if (collider != null)                                                      // platzierten Türmen hovern kann
        {
            collider.enabled = false;
        }

        SpriteRenderer sprite = currentPreview.GetComponentInChildren<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = new Color(1, 1, 1, 0.6f); //slightly transparent preview
        }
    }

    private IEnumerator MoveButton(RectTransform buttonRect, Vector2 targetPosition) //MOVEBTN
    {
        float duration = 1f;
        Vector2 startPosition = buttonRect.anchoredPosition;
        float elapsedTime = 0f;

        float startSpeed = 1f;
        float targetSpeed = 0.5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            t = t * t * (3f - 2f * t); // smoothes Movement des Buttons

            buttonRect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);

            if (uiAnimation != null)
            {
                uiAnimation.speed = Mathf.Lerp(startSpeed, targetSpeed, t);
            }

            yield return null;
        }

        buttonRect.anchoredPosition = targetPosition;

        if (uiAnimation != null)
        {
            uiAnimation.speed = targetSpeed;
        }
    }

    public Cards GetCurrentCard()
    {
        return currentCard;
    }

    public void ClearCard()
    {
        cardToFlip.MoveCardOut();
        cardToFlip.SetNewCard();
        currentCard = null;
    }


    private void PlaceTower()
    {
        if (currentTargetCell == null) return;

        if (currentTargetCell.isCellBuilt)
        {
            //if the cell is not empty then check for an able upgrade
            BaseTower towerInCell = currentTargetCell.placedTower.GetComponentInChildren<BaseTower>();
            string existingTower = currentTargetCell.towerName;

            Debug.Log($"Existing Tower: {towerInCell.nameTower}, Drawed Tower: {currentCard.TowerName}");

            if (towerInCell != null && existingTower == currentCard.TowerName)
            {
                towerInCell.UpgradeTower();
                currentTargetCell.towerLevel++;
                currentTargetCell.UpdateTowerLevelText();

                switch (currentCard.TowerName) 
                {
                    case "Zeus":
                        AudioManager.Instance.PlayTowerPlacementSFX(0);
                        GameManager.Instance.zeusTower++;
                        zeusBolt.UpgradeBolt();
                        break;
                    case "Poseidon":
                        AudioManager.Instance.PlayTowerPlacementSFX(1);
                        GameManager.Instance.poseidonTower++;
                        poseidonWave.UpgradeWave();
                        break;
                    case "Hera":
                        AudioManager.Instance.PlayTowerPlacementSFX(2);
                        GameManager.Instance.heraTower++;
                        heraStun.UpgradeStun();
                        break;
                    case "Hephaistos":
                        AudioManager.Instance.PlayTowerPlacementSFX(3);
                        GameManager.Instance.hephaistosTower++;
                        hephaistosQuake.UpgradeQuake();
                        break;
                }

                Destroy(currentPreview);
                currentPreview = null;
                ClearCard();
                DisableCells();
                GameManager.Instance.StartNextWave();
                return;
            }
        }
        else
        {
            //place a new tower
            GameObject newTower = Instantiate(currentCard.TowerPrefab, currentTargetCell.transform.position, Quaternion.identity);
            currentTargetCell.PlaceTower(newTower);

            SpriteRenderer towerRenderer = newTower.GetComponentInChildren<SpriteRenderer>();
            if (towerRenderer != null)
            {
                int sortingOffset = 0;
                if (currentTargetCell.cellIndex >= 20 && currentTargetCell.cellIndex < 30)
                {
                    sortingOffset = 1;
                }
                else if (currentTargetCell.cellIndex >= 30 && currentTargetCell.cellIndex < 40)
                {
                    sortingOffset = 2;
                }

                foreach (SpriteRenderer renderer in newTower.GetComponentsInChildren<SpriteRenderer>())
                {
                    renderer.sortingOrder += sortingOffset;
                }
            }

            currentTargetCell.isCellBuilt = true;

            switch (currentCard.TowerName)
            {
                case "Zeus":
                    AudioManager.Instance.PlayTowerPlacementSFX(0);
                    GameManager.Instance.zeusTower++;
                    zeusBolt.UpgradeBolt();
                    break;
                case "Poseidon":
                    AudioManager.Instance.PlayTowerPlacementSFX(1);
                    GameManager.Instance.poseidonTower++;
                    poseidonWave.UpgradeWave();
                    break;
                case "Hera":
                    AudioManager.Instance.PlayTowerPlacementSFX(2);
                    GameManager.Instance.heraTower++;
                    heraStun.UpgradeStun();
                    break;
                case "Hephaistos":
                    AudioManager.Instance.PlayTowerPlacementSFX(3);
                    GameManager.Instance.hephaistosTower++;
                    hephaistosQuake.UpgradeQuake();
                    break;
            }

            Destroy(currentPreview);
            currentPreview = null;
            ClearCard();
            DisableCells();
            GameManager.Instance.StartNextWave();
        }
    }

    public void ResetGrid()
    {
        foreach (GridCells cell in gridCells)
        {
            cell.ResetCell();
        }
    }
}

