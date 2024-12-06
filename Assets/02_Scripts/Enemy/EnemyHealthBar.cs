using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public GameObject healtbarOutlines;
    public void SetHealth(float healthPercentage)
    {
        //this.gameObject.transform.localScale = new Vector3(healthPercentage * 5, transform.localScale.y, 1);
        this.gameObject.transform.localScale = new Vector3(healthPercentage * 0.25f, transform.localScale.y, 1);
    }
    public void SetVisible(bool isVisible)
    {
        this.gameObject.SetActive(isVisible);
        healtbarOutlines.SetActive(isVisible);
    }
}