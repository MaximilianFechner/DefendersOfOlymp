using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{

    private static int defaultSortingOrder = 12;

    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO) {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition, Quaternion.Euler(0, 0, placedObjectTypeSO.GetRotationAngle(dir))); //TODO Vielleicht muss es auf y und z auf 0

        PlacedObject placedObject = placedObjectTransform.GetComponentInChildren<PlacedObject>();
        placedObject.placedObjectTypeSO = placedObjectTypeSO;
        placedObject.origin = origin;
        placedObject.dir = dir;

        PlacedObject towerWithSpriteRenderer = FindAnyObjectByType<PlacedObject>();
        SpriteRenderer spriteRenderer = towerWithSpriteRenderer.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = defaultSortingOrder - origin.y;

        return placedObject;
    }

    [SerializeField] private PlacedObjectTypeSO placedObjectTypeSO;
    private Vector2Int origin;
    private PlacedObjectTypeSO.Dir dir;

    public List<Vector2Int> GetGridPositionList() {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return placedObjectTypeSO;
    }

}
