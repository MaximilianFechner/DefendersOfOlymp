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

    private void Update() {
        if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null) {
            grid.GetXY(GetMouseWorldPosition(), out int x, out int z);

            Vector2Int placedObjectOrigin = new Vector2Int(x, z);
            
            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);

            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList) {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                    //Cannot build here
                    canBuild = false; 
                    break;
                }
            }
            
            
            if (canBuild) {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) +
                    new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();
                
                PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);
                placedObject.transform.rotation = Quaternion.Euler(0, 0, - placedObjectTypeSO.GetRotationAngle(dir));

                foreach (Vector2Int gridPosition in gridPositionList) {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }

                OnObjectPlaced?.Invoke(this, EventArgs.Empty);

            } else {
                Debug.Log("Cant build! Transform not null");
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            GridObject gridObject = grid.GetGridObject(GetMouseWorldPosition());
            PlacedObject placedObject = gridObject.GetPlacedObject();

            if (placedObject != null) {
                //Demolish
                placedObject.DestroySelf();

                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                foreach (Vector2Int gridPosition in gridPositionList) {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            placedObjectTypeSO = placedObjectTypeSOList[0];
            RefreshSelectedObjectType();
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            placedObjectTypeSO = placedObjectTypeSOList[1];
            RefreshSelectedObjectType();
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            placedObjectTypeSO = placedObjectTypeSOList[2];
            RefreshSelectedObjectType();
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            DeselectObjectType();
        }
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

    private void RefreshSelectedObjectType() {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    private void DeselectObjectType() {
        placedObjectTypeSO = null;
        RefreshSelectedObjectType();
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
