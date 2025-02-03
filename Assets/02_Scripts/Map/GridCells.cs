using UnityEngine;

public class GridCells : MonoBehaviour
{
    public int cellIndex;
    public GameObject placedTower;
    public string towerName;
    public int towerLevel;
    public GameObject buildFoundation; //um evtl. ein Fundament zu aktivieren beim Platzieren etc

    public bool isCellBuilt = false; //check ob die Cell bereits bebaut ist

    public void PlaceTower(GameObject tower)
    {
        placedTower = tower;
        towerName = tower.GetComponentInChildren<BaseTower>().nameTower; //save name for checks later
        towerLevel++;
    }

    public void ResetCell()
    {
        placedTower = null;
        towerName = "";
        towerLevel = 0;
        isCellBuilt = false;
    }
}
