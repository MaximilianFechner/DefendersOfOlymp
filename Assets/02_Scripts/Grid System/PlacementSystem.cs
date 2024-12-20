using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject _mouseIndicator, _cellIndicator;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Grid _grid;

    [SerializeField] private ObjectsDatabaseSO _database;
    private int _selectedObjectIndex = -1;

    [SerializeField] private GameObject _gridVisualization;

    private GridData _floorData, _towerData;

    private Renderer _previewRenderer;

    private List<GameObject> _placedGameObjects = new();

    private void Start()
    {
        StopPlacement();
        _floorData = new();
        _towerData = new();
        _previewRenderer = _cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        _selectedObjectIndex = _database.ObjectsData.FindIndex(data => data.ID == ID);
        if (_selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        _gridVisualization.SetActive(true);
        _cellIndicator.SetActive(true);
        _inputManager.OnClicked += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (_inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
        if (placementValidity == false)
            return;
        
        GameObject newObject = Instantiate(_database.ObjectsData[_selectedObjectIndex].Prefab);
        newObject.transform.position = _grid.CellToWorld(gridPosition);
        _placedGameObjects.Add(newObject);
        GridData selectedData = _database.ObjectsData[_selectedObjectIndex].ID == 0 ? _floorData : _towerData;
        selectedData.AddObjectAt(gridPosition,
            _database.ObjectsData[_selectedObjectIndex].Size, 
            _database.ObjectsData[_selectedObjectIndex].ID,
                _placedGameObjects.Count - 1);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = _database.ObjectsData[_selectedObjectIndex].ID == 0 ? _floorData : _towerData;

        return selectedData.CanPlaceObjectAt(gridPosition, _database.ObjectsData[_selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        _selectedObjectIndex = -1;
        _gridVisualization.SetActive(false);
        _cellIndicator.SetActive(false);
        _inputManager.OnClicked -= PlaceStructure;
        _inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (_selectedObjectIndex < 0)
            return;
        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);
        
        bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
        _previewRenderer.material.color = placementValidity ? Color.white : Color.red;
        
        _mouseIndicator.transform.position = mousePosition;
        _cellIndicator.transform.position = _grid.CellToWorld(gridPosition);
    }
}
