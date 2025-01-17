using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ZeusBolt : MonoBehaviour
{
    public GameObject boltPrefab;
    public GameObject boltPreview;
    private GameObject currentPreview;
    public LayerMask enemyLayer;

    [Space(10)]
    [Header("Game Design Values")]
    [Tooltip("The damage dealt by the skill")]
    [Min(0)]
    [SerializeField]
    private float damage = 100f;

    [Tooltip("Animationtime, it doesnt change the damage")]
    [Min(0)]
    [SerializeField]
    private float lightningDuration = 0.5f;

    [Tooltip("Radius for detecting collider of enemies")]
    [Min(0)]
    [SerializeField]
    private float attackRadius = 0.5f;

    [Tooltip("The time you have to wait before you can use the skill again")]
    [Min(0)]
    [SerializeField]
    private float cooldownTime = 20f;

    [Space(10)]

    [Tooltip("Minimum volume for the enemy sounds")]
    [Range(0, 1)]
    [SerializeField]
    private float minVolumeSounds = 0.1f;

    [Tooltip("Maximum volume for the enemy sounds")]
    [Range(0, 1)]
    [SerializeField]
    private float maxVolumeSounds = 0.35f;

    [Tooltip("Minimum pitch for the enemy sounds")]
    [Range(-3, 3)]
    [SerializeField]
    private float minPitchSounds = 0.8f;

    [Tooltip("Maximum pitch for the enemy sounds")]
    [Range(-3, 3)]
    [SerializeField]
    private float maxPitchSounds = 1f;

    public AudioClip skillSound;
    public AudioClip preSkillSound;
    private GameObject preBoltSoundObject;

    //private int skillLevel;
    //private float levelModifikatorDamage;
    //private float levelModifikatorCooldown;

    private float lastUseTime = -Mathf.Infinity;
    private bool isReady = false;

    private float remainingCooldownTime = 0f;

    private Vector2 buttonOriginalPosition; //BTN CD MOVE TEST
    public Button skillButton; //BTN CD MOVE TEST
    public Animator uiAnimation; //BTN CD MOVE TEST
    public Image image; //BTN CD MOVE TEST

    //BTN CD MOVE TEST
    private void Start()
    {
        buttonOriginalPosition = skillButton.GetComponent<RectTransform>().anchoredPosition;
        uiAnimation = uiAnimation.GetComponent<Animator>();
        image = image.GetComponent<Image>();
    }
    //

    private void Update()
    {
        if (Time.timeScale == 0) return;

        if (GameManager.Instance.isInWave)
        {
            if (remainingCooldownTime > 0)
            {
                remainingCooldownTime -= Time.deltaTime;
                if (remainingCooldownTime <= 0)
                {
                    remainingCooldownTime = 0;

                    StartCoroutine(MoveButton(skillButton.GetComponent<RectTransform>(), 
                        buttonOriginalPosition, new Color(0.73f, 0.73f, 0.73f), Color.white)); //BTN CD MOVE TEST
                    skillButton.interactable = true; //BTN CD MOVE TEST

                    UIManager.Instance.zeusSkillCooldown.text = "READY";
                }
                else
                {
                    UIManager.Instance.zeusSkillCooldown.text = $"{remainingCooldownTime:F1}s";
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateZeusSkill();
        }

        if (isReady)
        {
            PlacementPreview();

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                TriggerLightning();
                Destroy(currentPreview);
                currentPreview = null;
            }
        }
    }

    public void ActivateZeusSkill()
    {
        if (remainingCooldownTime <= 0 && GameManager.Instance.isInWave) //if (Time.time >= lastUseTime + cooldownTime)
        {
            isReady = true;

            if (currentPreview == null)
            {
                currentPreview = Instantiate(boltPreview);
                PlayPreBoltSFX(preSkillSound);
            }
        }
    }

    private void TriggerLightning()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        Collider2D targetEnemy = Physics2D.OverlapCircle(new Vector2(worldPosition.x, worldPosition.y), attackRadius, enemyLayer);

        if (targetEnemy != null)
        {
            GameObject bolt = Instantiate(boltPrefab, new Vector3(worldPosition.x, worldPosition.y + 10, 0), Quaternion.identity);
            PlayBoltSFX(skillSound);
            Destroy(bolt, lightningDuration);

            targetEnemy.GetComponent<EnemyManager>().TakeDamage(damage);

            remainingCooldownTime = cooldownTime;
            lastUseTime = Time.time;
            isReady = false;

            if (currentPreview != null)
            {
                Destroy(currentPreview);
                currentPreview = null;
            }

            if (preBoltSoundObject != null)
            {
                Destroy(preBoltSoundObject);
                preBoltSoundObject = null;
            }

            RectTransform buttonRect = skillButton.GetComponent<RectTransform>();
            Vector2 targetPosition = buttonOriginalPosition + new Vector2(0, -50);
            StartCoroutine(MoveButton(buttonRect, targetPosition, Color.white, new Color(0.73f, 0.73f, 0.73f)));
            skillButton.interactable = false;
        }
    }

    private void PlacementPreview()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;
        worldPosition.y += 10; //only for positioning for the placeholder asset

        if (currentPreview == null)
        {
            currentPreview = Instantiate(boltPreview);
        }

        currentPreview.transform.position = worldPosition;
    }

    private void PlayBoltSFX(AudioClip clip)
    {
        GameObject soundObject = new GameObject("BoltSound");
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();

        tempAudioSource.clip = clip;
        tempAudioSource.ignoreListenerPause = true;
        tempAudioSource.volume = Random.Range(minVolumeSounds, maxVolumeSounds);
        tempAudioSource.pitch = Random.Range(minPitchSounds, maxPitchSounds);

        tempAudioSource.Play();

        Destroy(soundObject, clip.length);
    }
    private void PlayPreBoltSFX(AudioClip clip)
    {
        preBoltSoundObject = new GameObject("PreBoltSound");
        AudioSource tempAudioSource = preBoltSoundObject.AddComponent<AudioSource>();

        tempAudioSource.clip = clip;
        tempAudioSource.ignoreListenerPause = true;
        tempAudioSource.loop = true;
        tempAudioSource.volume = Random.Range(minVolumeSounds, maxVolumeSounds);
        tempAudioSource.pitch = Random.Range(minPitchSounds, maxPitchSounds);

        tempAudioSource.Play();
    }

    private IEnumerator MoveButton(RectTransform buttonRect, Vector2 targetPosition, Color startColor, Color targetColor)
    {
        float duration = 1f;
        Vector2 startPosition = buttonRect.anchoredPosition;
        float elapsedTime = 0f;

        float startSpeed = 1f;
        float targetSpeed = 0.5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            t = t * t * (3f - 2f * t); // smoothes Movement des Buttons

            buttonRect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);

            if (image != null)
            {
                image.color = Color.Lerp(startColor, targetColor, t);
            }

            if (uiAnimation != null)
            {
                uiAnimation.speed = Mathf.Lerp(startSpeed, targetSpeed, t);
            }

            yield return null;
        }

        buttonRect.anchoredPosition = targetPosition;

        if (image != null)
        {
            image.color = targetColor;
        }

        if (uiAnimation != null)
        {
            uiAnimation.speed = targetSpeed;
        }
    }
}
