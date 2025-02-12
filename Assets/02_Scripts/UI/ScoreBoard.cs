using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using System.Collections;

public class ScoreBoard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Animator animator;
    private AudioSource audioSource;
    public AudioClip stoneSFX;

    private Coroutine hoverCoroutine;
    private bool isHovering = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.volume = 0.025f;
        audioSource.clip = stoneSFX;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;

        if (hoverCoroutine == null)
        {
            hoverCoroutine = StartCoroutine(HoverDelay());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;

        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
            hoverCoroutine = null;
        }

        animator.SetBool("isHovered", false);
    }

    private IEnumerator HoverDelay()
    {
        yield return new WaitForSeconds(1.5f);

        if (isHovering)
        {
            animator.SetBool("isHovered", true);

            if (stoneSFX != null && audioSource != null)
            {
                audioSource.pitch = 0.8f;
                audioSource.PlayOneShot(stoneSFX);
            }
        }

        hoverCoroutine = null;
    }
}
