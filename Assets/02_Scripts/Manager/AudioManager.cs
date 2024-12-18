using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource audioSource;

    public AudioClip levelBackgroundMusic;
    public AudioClip waveEndMusic;

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
        audioSource.resource = levelBackgroundMusic;
        audioSource.volume = 0.1f;
        audioSource.loop = true;
        audioSource.ignoreListenerPause = true;
        audioSource.Play();
    }

    public void PlayWaveEndMusic()
    {
        GameObject waveSoundObject = new GameObject("WaveEndSound");
        AudioSource tempAudioSource = waveSoundObject.AddComponent<AudioSource>();

        tempAudioSource.clip = waveEndMusic;
        tempAudioSource.ignoreListenerPause = true;
        tempAudioSource.volume = 0.3f;
        tempAudioSource.Play();

        Destroy(waveSoundObject, waveEndMusic.length);
    }



}
