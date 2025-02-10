using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public GameObject healtbarOutlines;
    public GameObject healtbarBackground;
    private float scaleX;
    private float scaleY;

    private void Start()
    {
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
    }

    public void SetHealth(float healthPercentage)
    {
        //this.gameObject.transform.localScale = new Vector3(healthPercentage * 5, transform.localScale.y, 1);
        this.gameObject.transform.localScale = new Vector3(healthPercentage * scaleX, scaleY, 1);
    }
    public void SetVisible(bool isVisible)
    {
        this.gameObject.SetActive(isVisible);
        healtbarOutlines.SetActive(isVisible);
        healtbarBackground.SetActive(isVisible);
    }
}