using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class ScoreBoard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Animator animator;

    private AudioSource audioSource;
    public AudioClip stoneSFX;

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
        animator.SetBool("isHovered", true);

        if (stoneSFX != null && audioSource != null)
        {
            audioSource.pitch = 1.1f;
            audioSource.PlayOneShot(stoneSFX);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("isHovered", false);

        if (stoneSFX != null && audioSource != null)
        {
            audioSource.pitch = 0.8f;
            audioSource.PlayOneShot(stoneSFX);
        }
    }
}
