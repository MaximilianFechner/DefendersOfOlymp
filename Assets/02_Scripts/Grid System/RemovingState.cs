using UnityEngine;

public class RemovingState : IBuildingState
{
    private int _gameObjectIndex = -1;
    private Grid _grid;
    private PreviewSystem _previewSystem;
    private GridData _floorData;
    private GridData _towerData;
    private ObjectPlacer _objectPlacer;


    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData floorData, GridData towerData, ObjectPlacer objectPlacer)
    {
        _grid = grid;
        _previewSystem = previewSystem;
        _floorData = floorData;
        _towerData = towerData;
        _objectPlacer = objectPlacer;
        
        _previewSystem.StartShowingRemovingPreview();
    }

    public void EndState()
    {
        _previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if (_towerData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false )
        {
            selectedData = _towerData;
        } else if(_floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = _floorData;
        }

        if (selectedData == null)
        {
            //Sound Maybe?
        }
        else
        {
            _gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (_gameObjectIndex == -1)
                return;
            selectedData.RemovedObjectAt(gridPosition);
            _objectPlacer.RemoveObjectAt(_gameObjectIndex);
        }
        Vector3 cellPosition = _grid.CellToWorld(gridPosition);
        _previewSystem.UpdatePosition(cellPosition, CheckIfSelectionsIsValid(gridPosition));
    }

    private bool CheckIfSelectionsIsValid(Vector3Int gridPosition)
    {
        return !(_towerData.CanPlaceObjectAt(gridPosition, Vector2Int.one) && _floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionsIsValid(gridPosition);
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), validity);
    }
}
