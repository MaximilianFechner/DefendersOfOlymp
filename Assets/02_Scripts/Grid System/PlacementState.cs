using UnityEngine;

public class PlacementState : IBuildingState
{
    private int _selectedObjectIndex = -1;
    private int _ID;
    private Grid _grid;
    private PreviewSystem _previewSystem;
    private ObjectsDatabaseSO _database;
    private GridData _floorData;
    private GridData _towerData;
    private ObjectPlacer _objectPlacer;

    public PlacementState(int id, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData floorData, GridData towerData, ObjectPlacer objectPlacer)
    {
        _ID = id;
        _grid = grid;
        _previewSystem = previewSystem;
        _database = database;
        _floorData = floorData;
        _towerData = towerData;
        _objectPlacer = objectPlacer;
        
        _selectedObjectIndex = _database.ObjectsData.FindIndex(data => data.ID == _ID);
        if (_selectedObjectIndex > -1)
        {
            _previewSystem.StartShowingPlacementPreview(_database.ObjectsData[_selectedObjectIndex].Prefab, _database.ObjectsData[_selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No Object With ID {id}");
        }
            
    }

    public void EndState()
    {
        _previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
        if (placementValidity == false)
            return;
        int index = _objectPlacer.PlaceObject(_database.ObjectsData[_selectedObjectIndex].Prefab, _grid.CellToWorld(gridPosition));

        GridData selectedData = _database.ObjectsData[_selectedObjectIndex].ID == 0 ? _floorData : _towerData;
        selectedData.AddObjectAt(gridPosition,
            _database.ObjectsData[_selectedObjectIndex].Size, 
            _database.ObjectsData[_selectedObjectIndex].ID,
            index);
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), false);
    }
    
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = _database.ObjectsData[_selectedObjectIndex].ID == 0 ? _floorData : _towerData;

        return selectedData.CanPlaceObjectAt(gridPosition, _database.ObjectsData[_selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
    }
}
