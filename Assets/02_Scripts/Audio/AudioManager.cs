using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource audioSource;

    public AudioClip levelBackgroundMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayLevelBackgroundMusic()
    {
        audioSource.volume = 0.1f;
        audioSource.loop = true;
        audioSource.ignoreListenerPause = true;
        audioSource.resource = levelBackgroundMusic;
        audioSource.Play();
    }



}
