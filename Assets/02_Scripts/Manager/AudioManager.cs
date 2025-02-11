using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource audioSource;

    public AudioClip levelBackgroundMusic;
    public AudioClip mainMenuMusic;
    public AudioClip levelAmbienteSFX;
    public AudioClip waveEndMusic;
    public AudioClip[] lostLifeSFX;
    public AudioClip cardSFX;
    public AudioClip buttonSFX;
    public AudioClip stoneSFX;
    public AudioClip[] towerPlacementSFX;
    public AudioClip[] hitImpactSFX;
    public AudioClip highscoreSFX;

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
    
    public void PlayMainMenuMusic()
    {
        audioSource.resource = mainMenuMusic;
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

        StartCoroutine(FadeInVolume(tempAudioSource, 0.015f, 10f));
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

    public void PlayLostLifeSFX()
    {
        GameObject lifeSoundObject = new GameObject("LostLifeSFX");
        AudioSource tempAudioSource = lifeSoundObject.AddComponent<AudioSource>();

        tempAudioSource.clip = lostLifeSFX[Random.Range(0, lostLifeSFX.Length)];
        tempAudioSource.ignoreListenerPause = true;
        tempAudioSource.volume = 0.4f;
        tempAudioSource.Play();

        Destroy(lifeSoundObject, tempAudioSource.clip.length);
    }

    public void PlayCardSFX()
    {
        GameObject cardSoundObject = new GameObject("CardSFX");
        AudioSource tempAudioSource = cardSoundObject.AddComponent<AudioSource>();

        tempAudioSource.clip = cardSFX;
        tempAudioSource.ignoreListenerPause = true;
        tempAudioSource.volume = Random.Range(0.1f, 0.2f);
        tempAudioSource.pitch = Random.Range(0.9f, 1.1f);
        tempAudioSource.Play();

        Destroy(cardSoundObject, cardSFX.length);
    }

    public void PlayButtonSFX()
    {
        GameObject buttonSoundObject = new GameObject("ButtonSFX");
        AudioSource tempAudioSource = buttonSoundObject.AddComponent<AudioSource>();

        tempAudioSource.clip = buttonSFX;
        tempAudioSource.ignoreListenerPause = true;
        tempAudioSource.volume = Random.Range(0.2f, 0.3f);
        tempAudioSource.pitch = Random.Range(0.9f, 1.1f);
        tempAudioSource.Play();

        Destroy(buttonSoundObject, buttonSFX.length);
    }

    public void PlayStoneSFX()
    {
        GameObject stoneSoundObject = new GameObject("StoneSFX");
        AudioSource tempAudioSource = stoneSoundObject.AddComponent<AudioSource>();

        tempAudioSource.clip = stoneSFX;
        tempAudioSource.ignoreListenerPause = true;
        tempAudioSource.volume = Random.Range(0.1f, 0.2f);
        tempAudioSource.pitch = Random.Range(0.9f, 1.1f);
        tempAudioSource.Play();

        Destroy(stoneSoundObject, stoneSFX.length);
    }

    public void PlayTowerPlacementSFX(int god)
    {
        GameObject towerPlacementSoundObject = new GameObject("TowerPlacementSFX");
        AudioSource tempAudioSource = towerPlacementSoundObject.AddComponent<AudioSource>();

        tempAudioSource.clip = towerPlacementSFX[god];
        tempAudioSource.ignoreListenerPause = true;
        tempAudioSource.volume = 0.2f;
        tempAudioSource.pitch = Random.Range(0.9f, 1.1f);
        tempAudioSource.Play();

        Destroy(towerPlacementSoundObject, towerPlacementSFX[god].length);
    }
    public void PlayHitImpactSFX(int god)
    {
        GameObject hitImpactSoundObject = new GameObject("HitImpactSFX");
        AudioSource tempAudioSource = hitImpactSoundObject.AddComponent<AudioSource>();

        tempAudioSource.clip = hitImpactSFX[god];
        tempAudioSource.ignoreListenerPause = true;

        if (tempAudioSource.clip == hitImpactSFX[0])
        {
            tempAudioSource.volume = Random.Range(0.01f, 0.02f);
            tempAudioSource.pitch = Random.Range(0.9f, 1.1f);
        }
        else
        {
            tempAudioSource.volume = Random.Range(0.035f, 0.06f);
            tempAudioSource.pitch = Random.Range(0.9f, 1.1f);
        }

        tempAudioSource.Play();

        Destroy(hitImpactSoundObject, hitImpactSFX[god].length);

    }

    public void PlayHighscoreSFX()
    {
        GameObject highscoreSFXObject = new GameObject("HighscoreSFX");
        AudioSource tempAudioSource = highscoreSFXObject.AddComponent<AudioSource>();

        tempAudioSource.clip = highscoreSFX;
        tempAudioSource.ignoreListenerPause = true;
        tempAudioSource.volume = 0.7f;
        tempAudioSource.loop = false;

        tempAudioSource.Play();

        Destroy(highscoreSFXObject, highscoreSFX.length);
    }
}
