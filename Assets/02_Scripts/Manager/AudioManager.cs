using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource audioSource;

    public AudioClip levelBackgroundMusic;
    public AudioClip levelAmbienteSFX;
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

    public void PlayLevelAmbienteSFX()
    {
        GameObject soundObject = new GameObject("LevelAmbienteSFX");
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();

        tempAudioSource.clip = levelAmbienteSFX;
        tempAudioSource.ignoreListenerPause = true;
        tempAudioSource.volume = 0f;
        tempAudioSource.loop = true;

        tempAudioSource.Play();

        StartCoroutine(FadeInVolume(tempAudioSource, 0.01f, 10f));
    }

    private IEnumerator FadeInVolume(AudioSource audioSource, float targetVolume, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
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
