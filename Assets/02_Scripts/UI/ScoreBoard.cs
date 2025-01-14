using UnityEngine;
using UnityEngine.EventSystems;

public class ScoreBoard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("isHovered", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("isHovered", false);
    }
}
