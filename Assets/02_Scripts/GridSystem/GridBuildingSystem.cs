using UnityEngine;
using System.Collections.Generic;
using System;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Instance {
        get; private set;
    }

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    private Grid<GridObject> grid;
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList;
    private PlacedObjectTypeSO placedObjectTypeSO;
    private PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Down;

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float cellSize;



    private void Awake() {
        Instance = this;

        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, transform.position, (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z));

        placedObjectTypeSO = null;
    }


    public class GridObject {

        private Grid<GridObject> grid;
        private int x;
        private int y;
        private PlacedObject placedObject;

        public GridObject(Grid<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
        }

        public void SetPlacedObject(PlacedObject placedObject) {
            this.placedObject = placedObject; 
            grid.TriggerGridObjectChanged(x, y);
        }

        public PlacedObject GetPlacedObject() { return placedObject; }

        public void ClearPlacedObject() {
            this.placedObject = null; 
            grid.TriggerGridObjectChanged(x, y);
        }

        public bool CanBuild() {
            return placedObject == null; 
        }

        public override string ToString() {
            return x + "," + y + "\n" + placedObject;
        }
    }

    public bool PlaceTower() {
        grid.GetXY(GetMouseWorldPosition(), out int x, out int z);
        Vector2Int placedObjectOrigin = new Vector2Int(x, z);
        List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);

        BaseTower towerToUpgrade = null;

        bool canBuild = true;
        foreach (Vector2Int gridPosition in gridPositionList) {
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                //Cannot build here
                canBuild = false;
                towerToUpgrade = grid.GetGridObject(gridPosition.x, gridPosition.y).GetPlacedObject().gameObject.GetComponent<BaseTower>();
                if (towerToUpgrade == null) {
                    Debug.LogError("towerToUpgrade is null on position: " + gridPosition.ToString());
                }
                break;
            }
        }


        if (canBuild) 
        {

            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) +
                new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();

            if (!placedObjectTypeSO.prefab.gameObject.GetComponentInChildren<BaseTower>().enabled) {
                placedObjectTypeSO.prefab.gameObject.GetComponentInChildren<BaseTower>().enabled = true;
            }

            PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);
            placedObject.transform.rotation = Quaternion.Euler(0, 0, -placedObjectTypeSO.GetRotationAngle(dir));

            foreach (Vector2Int gridPosition in gridPositionList) {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }
            DeselectObjectType();
            OnObjectPlaced?.Invoke(this, EventArgs.Empty);

        } else {
            Debug.Log("Cant build! Transform not null. Try to upgrade");
            if (towerToUpgrade == null) {
                Debug.LogError("TowerToUpgrade is null!");
            } else if (!towerToUpgrade.towerName.Equals(placedObjectTypeSO.towerName)) {
                Debug.Log("Cant upgrade! Selected Tower is not equals placed Tower! Try again");
                return canBuild;
            } else {
                towerToUpgrade.UpgradeTower();
                DeselectObjectType();
                OnObjectPlaced?.Invoke(this, EventArgs.Empty);
                canBuild = true;
                Debug.Log("Tower upgraded");
            }
        }
        return canBuild;
    }

    public static Vector3 GetMouseWorldPosition() {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 pos, Camera worldCamera) {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(pos);
        return worldPosition;
    }


    public Vector3 GetMouseWorldSnappedPosition() {
        Vector3 mousePosition = GetMouseWorldPosition();
        grid.GetXY(mousePosition, out int x, out int y);

        if (placedObjectTypeSO != null) {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        } else {
            return mousePosition;
        }
    }

    public void RefreshSelectedObjectType(PlacedObjectTypeSO selectedTower) {
        for (int i = 0;i < placedObjectTypeSOList.Count;i++) {
            if (selectedTower.towerName.Equals(placedObjectTypeSOList[i].towerName)) {
                placedObjectTypeSO = placedObjectTypeSOList[i];
                OnSelectedChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        if (placedObjectTypeSO == null) {
            Debug.LogError($"Selected Tower {selectedTower.ToString()} not found in PlacedObjectTypeSOList {placedObjectTypeSOList.ToString()}.");
        }
    }

    public void DeselectObjectType() {
        placedObjectTypeSO = null;
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public Quaternion GetPlacedObjectRotation() {
        if (placedObjectTypeSO != null) {
            return Quaternion.Euler(0, 0, -placedObjectTypeSO.GetRotationAngle(dir));
        } else {
            return Quaternion.identity;
        }
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return placedObjectTypeSO;
    }

}
