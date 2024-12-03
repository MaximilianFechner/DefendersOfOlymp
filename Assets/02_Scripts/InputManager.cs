using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera _sceneCamera;

    private Vector3 _lastPosition;

    [SerializeField] private LayerMask _placementLayermask;

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _sceneCamera.nearClipPlane;
        Ray ray = _sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, _placementLayermask))
        {
            _lastPosition = hit.point;
        }
        return _lastPosition;
    }
}
