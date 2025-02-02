using UnityEngine;

public class GridCells : MonoBehaviour
{
    public int cellIndex;
    public GameObject placedTower;
    public string towerName;
    public int towerLevel;
    public GameObject buildFoundation; //um evtl. ein Fundament zu aktivieren beim Platzieren etc

    public bool isCellBuilt => placedTower != null; //check ob die Cell bereits bebaut ist

    public void PlaceTower(GameObject tower)
    {
        placedTower = tower;
        towerName = tower.name; // Name für spätere Vergleiche speichern
    }
}
