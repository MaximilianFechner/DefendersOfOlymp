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

    [System.Serializable]
    public struct GridData
    {
        public int width;
        public int height;
        public float cellSize;
        public Vector3 offset;
        [HideInInspector] public Grid<GridObject> grid;
    }

    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList;
    private PlacedObjectTypeSO placedObjectTypeSO;
    private PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Down;

    [SerializeField] private GridData mainGrid; // Daten für das Hauptgrid
    [SerializeField] private GridData upperGrid;
    [SerializeField] private GridData lowerGrid;

    private void Awake() {
        Instance = this;

        // Initialisiere beide Grids
        InitializeGrid(ref mainGrid);
        InitializeGrid(ref upperGrid);
        InitializeGrid(ref lowerGrid);

        placedObjectTypeSO = null;
    }

    private void InitializeGrid(ref GridData gridData)
    {
        gridData.grid = new Grid<GridObject>(
            gridData.width,
            gridData.height,
            gridData.cellSize,
            transform.position + gridData.offset,
            (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z)
        );
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

    public bool PlaceTower() 
    {
        GridData activeGrid = GetActiveGrid();
        if (activeGrid.grid == null) return false; // falls kein Grid gefunden wurde abbrechen

        activeGrid.grid.GetXY(GetMouseWorldPosition(), out int x, out int z);
        Vector2Int placedObjectOrigin = new Vector2Int(x, z);
        List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);

        BaseTower towerToUpgrade = null;

        bool canBuild = true;
        foreach (Vector2Int gridPosition in gridPositionList) {
            if (!activeGrid.grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                //Cannot build here
                canBuild = false;
                towerToUpgrade = activeGrid.grid.GetGridObject(gridPosition.x, gridPosition.y).GetPlacedObject().gameObject.GetComponent<BaseTower>();
                if (towerToUpgrade == null) {
                    Debug.LogError("towerToUpgrade is null on position: " + gridPosition.ToString());
                }
                break;
            }
        }


        if (canBuild) 
        {

            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = activeGrid.grid.GetWorldPosition(x, z) +
                new Vector3(rotationOffset.x, rotationOffset.y) * activeGrid.grid.GetCellSize();

            if (!placedObjectTypeSO.prefab.gameObject.GetComponentInChildren<BaseTower>().enabled) {
                placedObjectTypeSO.prefab.gameObject.GetComponentInChildren<BaseTower>().enabled = true;
            }

            PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);
            placedObject.transform.rotation = Quaternion.Euler(0, 0, -placedObjectTypeSO.GetRotationAngle(dir));

            foreach (Vector2Int gridPosition in gridPositionList) {
                activeGrid.grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
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

    private GridData GetActiveGrid()
    {
        if (Input.mousePosition.x < 200) // Beispielbedingung: Linke Seite des Bildschirms
        {
            return upperGrid;
        }
        else if (Input.mousePosition.x > 200)
        {
            return mainGrid;
        }
        else
        {
            return lowerGrid;
        }
        //return new GridData();
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


    public Vector3 GetMouseWorldSnappedPosition() 
    {
        GridData activeGrid = GetActiveGrid();

        Vector3 mousePosition = GetMouseWorldPosition();
        activeGrid.grid.GetXY(mousePosition, out int x, out int y);

        if (placedObjectTypeSO != null) {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = activeGrid.grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y) * activeGrid.grid.GetCellSize();
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

    private void OnDrawGizmos()
    {
        if (mainGrid.grid != null)
        {
            DrawGridGizmos(mainGrid);
        }
        if (upperGrid.grid != null)
        {
            DrawGridGizmos(upperGrid);
        }
        if (lowerGrid.grid != null)
        {
            DrawGridGizmos(lowerGrid);
        }
    }

    private void DrawGridGizmos(GridData gridData)
    {
        Gizmos.color = Color.white; // Farbe des Gitters
        Vector3 offset = gridData.grid.GetWorldPosition(0, 0) - transform.position; // Offset des Grids relativ zum Transform des Scripts

        for (int x = 0; x <= gridData.width; x++)
        {
            for (int y = 0; y <= gridData.height; y++)
            {
                Vector3 worldPos = gridData.grid.GetWorldPosition(x, y);
                Vector3 gizmosPos = new Vector3(worldPos.x, worldPos.y, 0f) + offset; // Z-Koordinate auf 0 setzen und Offset berücksichtigen

                Gizmos.DrawLine(gizmosPos, gizmosPos + Vector3.right * gridData.grid.GetCellSize());
                Gizmos.DrawLine(gizmosPos, gizmosPos + Vector3.up * gridData.grid.GetCellSize()); // Verwende Vector3.up für 2D
            }
        }
    }

}
