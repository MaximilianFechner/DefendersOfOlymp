using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Grid _grid;

    [SerializeField] private ObjectsDatabaseSO _database;

    [SerializeField] private GameObject _gridVisualization;

    private GridData _floorData, _towerData;

    [SerializeField] private PreviewSystem _preview;

    private Vector3Int _lastDetectedPosition = Vector3Int.zero;

    [SerializeField] private ObjectPlacer _objectPlacer;

    private IBuildingState _buildingState;

    private void Start()
    {
        StopPlacement();
        _floorData = new();
        _towerData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        _gridVisualization.SetActive(true);
        _buildingState = new PlacementState(ID, _grid, _preview, _database, _floorData, _towerData, _objectPlacer);
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

        _buildingState.OnAction(gridPosition);
    }

    // private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    // {
    //     GridData selectedData = _database.ObjectsData[_selectedObjectIndex].ID == 0 ? _floorData : _towerData;
    //
    //     return selectedData.CanPlaceObjectAt(gridPosition, _database.ObjectsData[_selectedObjectIndex].Size);
    // }

    private void StopPlacement()
    {
        if (_buildingState == null)
            return;
        _gridVisualization.SetActive(false);
        _buildingState.EndState();
        _inputManager.OnClicked -= PlaceStructure;
        _inputManager.OnExit -= StopPlacement;
        _lastDetectedPosition = Vector3Int.zero;
        _buildingState = null;
    }

    private void Update()
    {
        if (_buildingState == null)
            return;
        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

        if (_lastDetectedPosition != gridPosition)
        {
            _buildingState.UpdateState(gridPosition);
            _lastDetectedPosition = gridPosition;
        }

    }
}
