using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public void SetHealth(float healthPercentage)
    {
        this.gameObject.transform.localScale = new Vector3(healthPercentage * 8, 1, 1);
    }
    public void SetVisible(bool isVisible)
    {
        this.gameObject.SetActive(isVisible);
    }
}