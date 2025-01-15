using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GridData
{
    private Dictionary<Vector3Int, PlacementData> _placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = _calculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            if (_placedObjects.ContainsKey(pos))
                throw new Exception($"Dictionary already contains this cell Position {pos}");
            _placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> _calculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
            
        }
        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = _calculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (_placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (_placedObjects.ContainsKey(gridPosition) == false)
            return -1;
        return _placedObjects[gridPosition].PlacedObjectIndex;
    }

    public void RemovedObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in _placedObjects[gridPosition].occupiedPositions)
        {
            _placedObjects.Remove(pos);
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }
    
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
    
}
