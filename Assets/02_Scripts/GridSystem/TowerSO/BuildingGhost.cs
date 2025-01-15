using System.Runtime.CompilerServices;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{

    private Transform visual;
    private PlacedObjectTypeSO placedObjectTypeSO;

    private void Start() {
        RefreshVisual();

        GridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
    }

    private void Instance_OnSelectedChanged(object sender, System.EventArgs e) {
        RefreshVisual();
    }

    private void LateUpdate() {
        Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldSnappedPosition();
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, 15f); //TODO Time.deltaTime * 15f

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, GridBuildingSystem.Instance.GetPlacedObjectRotation(), 15f); //TODO Time.deltaTime * 15f

    }

    private void RefreshVisual() {
        if (visual != null) {
            Destroy(visual.gameObject);
            visual = null;
        }

        PlacedObjectTypeSO placedObjectTypeSO = GridBuildingSystem.Instance.GetPlacedObjectTypeSO();

        if (placedObjectTypeSO != null) {
            visual = Instantiate(placedObjectTypeSO.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
        }

    }

}
