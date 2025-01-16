using UnityEngine;
using UnityEngine.UI;

public class TimeButton : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite pressedSprite;
    public Image buttonImage;
    public Button button;

    private TimeButtonManager manager;

    private void Start()
    {
        manager = FindFirstObjectByType<TimeButtonManager>();
        button.onClick.AddListener(OnButtonPressed);
    }

    private void OnButtonPressed()
    {
        manager.SetActiveButton(this);
    }

    public void SetPressedState(bool isPressed)
    {
        buttonImage.sprite = isPressed ? pressedSprite : normalSprite;
        buttonImage.color = isPressed ? new Color(0.8f, 0.8f, 0.8f) : Color.white;
        button.transform.position = isPressed ? new Vector2(this.transform.position.x, this.transform.position.y - 5f)
            : new Vector2(this.transform.position.x, this.transform.position.y + 5f);
    }
}
