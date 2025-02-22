using UnityEngine;
using UnityEngine.EventSystems;

public class BloodAnimationHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject bloodTop;
    private Animator bloodAnimator;

    void Start()
    {
        if (bloodTop != null)
            bloodAnimator = bloodTop.GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (bloodAnimator != null)
            bloodAnimator.SetBool("isHovered", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (bloodAnimator != null)
            bloodAnimator.SetBool("isHovered", false);
    }
}
