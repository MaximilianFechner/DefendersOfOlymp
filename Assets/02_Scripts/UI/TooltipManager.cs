using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public RectTransform tooltipPanel;
    public Text tooltipText;
    public Text tooltipData;
    public Canvas canvas;

    private Vector2 defaultOffset = new Vector2(170, 65); //offset for 1920x1080
    private Vector2 currentOffset;
    private Vector2 lastScreenResolution; //for check for screensize changes

    private void Awake()
    {
        Instance = this;
        HideTooltip();
    }

    private void Start()
    {
        lastScreenResolution = new Vector2(Screen.width, Screen.height);
        UpdateOffset();
    }

    private void Update()
    {
        if (Screen.width != lastScreenResolution.x || Screen.height != lastScreenResolution.y)
        {
            lastScreenResolution = new Vector2(Screen.width, Screen.height);
            UpdateOffset();
        }
    }

    public void ShowTooltip(string text)
    {
        tooltipText.text = text;
        tooltipPanel.gameObject.SetActive(true);
        UpdateTooltipPosition(Input.mousePosition);
    }

    public void ShowTooltipData(string text)
    {
        tooltipData.text = text;
        tooltipPanel.gameObject.SetActive(true);
        UpdateTooltipPosition(Input.mousePosition);
    }

    public void UpdateTooltipPosition(Vector2 mousePosition)
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 tooltipSize = tooltipPanel.sizeDelta;

        Vector2 newPosition = mousePosition + currentOffset;

        if (newPosition.x + tooltipSize.x * 0.75f > screenSize.x)
        {
            newPosition.x = mousePosition.x - currentOffset.x;
        }

        if (newPosition.y - tooltipSize.y < 0)
        {
            newPosition.y = mousePosition.y + currentOffset.y;
        }

        // f�r die Umrechnugn von Bildschirmkoordinaten in Canvaskoordinaten
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            newPosition,
            canvas.worldCamera,
            out localPosition
        );

        tooltipPanel.anchoredPosition = localPosition;
    }

    private void UpdateOffset()
    {
        float scaleX = Screen.width / 1920f;
        float scaleY = Screen.height / 1080f;

        currentOffset = new Vector2(defaultOffset.x * scaleX, defaultOffset.y * scaleY);
    }

    public void HideTooltip()
    {
        tooltipPanel.gameObject.SetActive(false);
    }
}
