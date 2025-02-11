using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public RectTransform tooltipPanel;
    public Text tooltipText;
    public Text tooltipData;
    public Canvas canvas;

    private Vector2 defaultOffset = new Vector2(180, 65); //offset for 1920x1080
    private Vector2 currentOffset;
    private Vector2 lastScreenResolution; //for check for screensize changes

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        lastScreenResolution = new Vector2(Screen.width, Screen.height);
        UpdateOffset();

        tooltipText.text = "";
        tooltipData.text = "";
    }

    private void OnEnable()
    {
        if (tooltipPanel == null)
        {
            tooltipPanel = GameObject.Find("PNLTooltip")?.GetComponent<RectTransform>();
        }

        if (canvas == null)
        {
            canvas = GameObject.Find("UI")?.GetComponent<Canvas>();
        }

        if (tooltipPanel != null)
        {
            tooltipPanel.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Tooltip Panel not found on enable.");
        }

        HideTooltip();
    }

    private void Update()
    {
        if (Screen.width != lastScreenResolution.x || Screen.height != lastScreenResolution.y)
        {
            lastScreenResolution = new Vector2(Screen.width, Screen.height);
            UpdateOffset();
        }
    }

    public void ShowTooltip(string text, string hoveredElement)
    {
        if (!GameManager.Instance.showTooltips) return;
        if (tooltipPanel == null)
        {
            tooltipPanel = GameObject.Find("PNLTooltip")?.GetComponent<RectTransform>();
        }

        tooltipText.text = text;
        tooltipPanel.gameObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipPanel);

        if (hoveredElement == "enemy")
        {
            SetTooltipHeight(-60);
            UpdateOffsetPerTooltip(-5);
        }
        else if (hoveredElement == "tower")
        {
            SetTooltipHeight(-15f);
            UpdateOffsetPerTooltip(15);
        }
        else if (hoveredElement == "zeusSkill")
        {
            SetTooltipHeight(-15);
            UpdateOffsetPerTooltip(15);
        }
        else if (hoveredElement == "skill")
        {
            SetTooltipHeight(10);
            UpdateOffsetPerTooltip(25);
        }

        UpdateTooltipPosition(Input.mousePosition);
    }

    public void ShowTooltipData(string text)
    {
        if (!GameManager.Instance.showTooltips) return;
        if (tooltipPanel == null)
        {
            tooltipPanel = GameObject.Find("PNLTooltip")?.GetComponent<RectTransform>();
        }

        tooltipData.text = text;
        tooltipPanel.gameObject.SetActive(true);
        UpdateTooltipPosition(Input.mousePosition);
    }

    public void UpdateTooltipPosition(Vector2 mousePosition)
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 tooltipSize = tooltipPanel.sizeDelta;

        Vector2 newPosition = mousePosition + currentOffset;

        if (newPosition.x + tooltipSize.x > screenSize.x * 0.9f)
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

    private void UpdateOffsetPerTooltip(float addY)
    {
        float scaleX = Screen.width / 1920f;
        float scaleY = Screen.height / 1080f;

        currentOffset = new Vector2(defaultOffset.x * scaleX, defaultOffset.y * scaleY + addY);
    }

    public void HideTooltip()
    {
        if (tooltipPanel != null)
        {
            tooltipPanel.gameObject.SetActive(false);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (tooltipPanel == null)
        {
            tooltipPanel = GameObject.Find("PNLTooltip")?.GetComponent<RectTransform>();

            if (tooltipPanel == null)
            {
                Debug.LogError("TooltipPanel nicht gefunden!");
            }
        }
    }

    private void SetTooltipHeight(float height)
    {
        if (tooltipPanel != null)
        {
            Vector2 size = tooltipPanel.sizeDelta;
            size.y = height;
            tooltipPanel.sizeDelta = size;
        }
    }

    private void OnDestroy()
    {
        tooltipText.text = "";
        tooltipData.text = "";
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void DestroyManager()
    {
        var Manager = this.GameObject();
        GameObject.Destroy	(Manager);
    }
}
