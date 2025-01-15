using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] List<GameObject> _placedGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
                
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        _placedGameObjects.Add(newObject);
        return _placedGameObjects.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (_placedGameObjects.Count >= gameObjectIndex || _placedGameObjects[gameObjectIndex] ==null)
            return;
        Destroy(_placedGameObjects[gameObjectIndex]);
        _placedGameObjects[gameObjectIndex] = null;
    }
}
