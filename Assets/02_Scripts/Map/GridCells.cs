using UnityEngine;
using UnityEngine.UI;

public class GridCells : MonoBehaviour
{
    public int cellIndex;
    public GameObject placedTower;
    public string towerName;
    public int towerLevel;
    public Text towerLevelText;
    public GameObject buildFoundation; //um evtl. ein Fundament zu aktivieren beim Platzieren etc

    public bool isCellBuilt = false; //check ob die Cell bereits bebaut ist

    public void PlaceTower(GameObject tower)
    {
        placedTower = tower;
        towerName = tower.GetComponentInChildren<BaseTower>().nameTower; //save name for checks later
        towerLevel++;

        if (GameManager.Instance.showTooltips)
        {
            UpdateTowerLevelText();
        }
    }

    public void ResetCell()
    {
        placedTower = null;
        towerName = "";
        towerLevel = 0;
        isCellBuilt = false;

        if (towerLevelText != null)
        {
            towerLevelText.text = "";
        }
    }

    public void UpdateTowerLevelText()
    {
        if (!GameManager.Instance.showTooltips) return;

        if (towerLevelText != null && towerLevel > 0)
        {
            if (!towerLevelText.gameObject.activeInHierarchy) towerLevelText.gameObject.SetActive(true);
            towerLevelText.text = $"{towerLevel.ToString()}";

            switch (towerName)
            {
                case "Zeus":
                    towerLevelText.color = new Color(225f / 255f, 224f / 255f, 225f / 255f);
                    break;
                case "Poseidon":
                    towerLevelText.color = new Color(14f / 255f, 161f / 255f, 210f / 255f);
                    break;
                case "Hera":
                    towerLevelText.color = new Color(225f / 255f, 156f / 255f, 241f / 255f);
                    break;
                case "Hephaistos":
                    towerLevelText.color = new Color(250f / 255f, 152f / 255f, 33f / 255f);
                    break;
            }
            
        }
    }
}
