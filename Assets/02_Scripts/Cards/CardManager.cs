using System.Collections;
using System.Collections.Generic;
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

    private void HighlightCell(GridCells cell)
    {
        foreach (GridCells c in gridCells)
        {
            c.GetComponent<SpriteRenderer>().color = Color.white; // Reset aller Zellen
        }

        cell.GetComponent<SpriteRenderer>().color = Color.green; // Neue Zelle hervorheben
    }

    public void DrawCard()
    {
        if (AvailableCards.Count == 0) return;

        int randomIndex = Random.Range(0, AvailableCards.Count);
        currentCard = AvailableCards[randomIndex];

        //CardDisplay.gameObject.SetActive(true);
        //drawCardButton.gameObject.SetActive(false);

        RectTransform buttonRect = drawCardBTN.GetComponent<RectTransform>(); //MOVEBTN
        Vector2 targetPosition = buttonOriginalPosition + new Vector2(0, -200); //MOVEBTN
        StartCoroutine(MoveButton(buttonRect, targetPosition)); //MOVEBTN
        drawCardBTN.interactable = false; //MOVEBTN
        isDrawableLighting.enabled = false;

        cardToFlip.FlipCard(currentCard.CardSprite); //Cardflip Animation

        AudioManager.Instance.PlayCardSFX();

        // Preview-Turm erstellen
        if (currentPreview != null) Destroy(currentPreview);
        currentPreview = Instantiate(currentCard.TowerPrefab);

        //Collider2D collider = currentPreview.GetComponentInChildren<Collider2D>(); // Sucht auch in Kind-Objekten
        //if (collider != null)
        //{
        //    collider.enabled = false;
        //}

        SpriteRenderer sprite = currentPreview.GetComponentInChildren<SpriteRenderer>(); // Sucht auch in Kind-Objekten
        if (sprite != null)
        {
            sprite.color = new Color(1, 1, 1, 0.5f);
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
            // Falls der Turm bereits existiert, prüfen, ob er geupgraded werden kann
            BaseTower existingTower = currentTargetCell.GetComponentInChildren<BaseTower>();
            BaseTower drawedTower = currentCard.TowerPrefab.GetComponent<BaseTower>();

            if (existingTower != null && existingTower.towerName == drawedTower.towerName)
            {
                existingTower.UpgradeTower();
                Destroy(currentPreview);
                currentPreview = null;
                ClearCard();
                GameManager.Instance.StartNextWave();
                return;
            }
        }
        else
        {
            // Neuen Turm platzieren
            GameObject newTower = Instantiate(currentCard.TowerPrefab, currentTargetCell.transform.position, Quaternion.identity);
            currentTargetCell.PlaceTower(newTower);

            Destroy(currentPreview);
            currentPreview = null;
            ClearCard();
            GameManager.Instance.StartNextWave();
        }


        //if (currentCard.TowerName.Contains("Zeus")) AudioManager.Instance.PlayTowerPlacementSFX(0);
        //else if (currentCard.TowerName.Contains("Poseidon")) AudioManager.Instance.PlayTowerPlacementSFX(1);
        //else if (currentCard.TowerName.Contains("Hera")) AudioManager.Instance.PlayTowerPlacementSFX(2);
        //else if (currentCard.TowerName.Contains("Hephaistos")) AudioManager.Instance.PlayTowerPlacementSFX(3);

        //bool buildOrUpgradedTower = GridBuildingSystem.Instance.PlaceTower();
        //if (buildOrUpgradedTower) {
        //    currentPreview = null;
        //    ClearCard();
        //    GameManager.Instance.StartNextWave();
        //} else {
        //    Debug.Log("Couldn't build or upgrade. The player has to choose another position.");
        //    //TODO Feedback to player
        //}
    }
}

