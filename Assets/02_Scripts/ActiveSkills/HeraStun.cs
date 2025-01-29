using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class HeraStun : MonoBehaviour
{
    public GameObject stunPrefab;
    public GameObject stunPreview;
    private GameObject currentPreview;
    public LayerMask enemyLayer;

    [Space(10)]
    [Header("Game Design Values")]
    //[Tooltip("The damage per tick in every interval")]
    //[Min(0)]
    //[SerializeField]
    //private float _damageOnUse = 15f;

    [Tooltip("The minimum damage the ability does")]
    [Min(0)]
    [SerializeField]
    private float damageLowerLimit = 12f;

    [Tooltip("The maximum damage the ability does")]
    [Min(0)]
    [SerializeField]
    private float damageUpperLimit = 18f;

    [Tooltip("The Percentage for the default movement speed")]
    [Min(0)]
    [SerializeField]
    private float _slowPercentage = 0.1f;

    [Tooltip("The time how long the skill is active")]
    [Min(0)]
    [SerializeField]
    private float _slowDuration = 3f;

    [Tooltip("The radius for the skill")]
    [Min(0)]
    [SerializeField]
    private float _skillRadius = 5f;

    [Tooltip("The time you have to wait before you can use the skill again")]
    [Min(0)]
    [SerializeField]
    private float _cooldownTime = 30f;

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
    private GameObject preStunSoundObject;

    //private int skillLevel;
    //private float levelModifikatorDamage;
    //private float levelModifikatorRadius;
    //private float levelModifikatorDuration;
    //private float levelModifikatorCooldown;

    private float lastUseTime = -Mathf.Infinity;
    private bool isReady = false;

    private float remainingCooldownTime = 0f;

    private Vector2 buttonOriginalPosition; //BTN CD MOVE
    public Button skillButton; //BTN CD MOVE
    public Animator uiAnimation; //BTN CD MOVE
    public Image image; //BTN CD MOVE

    public ZeusBolt zeusBolt;
    public PoseidonWave poseidonWave;
    public HephaistosQuake hephaistosQuake;

    //BTN CD MOVE
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
                    UIManager.Instance.heraSkillCooldown.text = "READY";

                    StartCoroutine(MoveButton(skillButton.GetComponent<RectTransform>(),
                        buttonOriginalPosition, new Color(0.73f, 0.73f, 0.73f), Color.white)); //BTN CD MOVE
                    skillButton.interactable = true; //BTN CD MOVE
                }
                else
                {
                    UIManager.Instance.heraSkillCooldown.text = $"{remainingCooldownTime:F1}s";
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (isReady)
            {
                CancelHeraSkill();
            }
            else
            {
                ActivateHeraSkill();
                zeusBolt.CancelZeusSkill();
                poseidonWave.CancelPoseidonSkill();
            }
        }

        if (isReady)
        {
            PlacementPreview();

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                PlaceStun();
                Destroy(currentPreview);
                currentPreview = null;
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelHeraSkill();
            }
        }

    }

    public void ActivateHeraSkill()
    {
        if (Time.timeScale == 0) return;
        if (remainingCooldownTime <= 0 && GameManager.Instance.isInWave) //if (Time.time >= lastUseTime + _cooldownTime)
        {
            if (isReady)
            {
                CancelHeraSkill();
            }
            else
            {
                isReady = true;

                if (currentPreview == null)
                {
                    Vector3 mousePosition = Input.mousePosition;
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
                    worldPosition.z = 0;

                    currentPreview = Instantiate(stunPreview);
                    currentPreview.transform.localScale = new Vector3(1 * (_skillRadius / 5), 1 * (_skillRadius / 5), 1 * (_skillRadius / 5));
                    PlayPreStunSFX(preSkillSound);
                }

                zeusBolt.CancelZeusSkill();
                poseidonWave.CancelPoseidonSkill();
            }
        }
    }

    private void PlaceStun()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        GameObject stun = Instantiate(stunPrefab, worldPosition, Quaternion.identity);
        stun.transform.localScale = new Vector3(1 * (_skillRadius / 5), 1 * (_skillRadius / 5), 1 * (_skillRadius / 5));
        Destroy(stun, 0.74f);
        PlaySoundOnTempGameObject(skillSound);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(worldPosition, _skillRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out EnemyManager enemyManager))
            {
                enemyManager.TakeDamage(Mathf.RoundToInt(Random.Range(damageLowerLimit, damageUpperLimit)));
            }

            if (enemy.TryGetComponent(out EnemyPathfinding enemyPathfinding))
            {
                enemyPathfinding.SlowMovement(_slowPercentage, _slowDuration);
            }
        }

        remainingCooldownTime = _cooldownTime;
        lastUseTime = Time.time;
        isReady = false;

        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }

        if (preStunSoundObject != null)
        {
            Destroy(preStunSoundObject);
            preStunSoundObject = null;
        }

        RectTransform buttonRect = skillButton.GetComponent<RectTransform>();
        Vector2 targetPosition = buttonOriginalPosition + new Vector2(0, -50);
        StartCoroutine(MoveButton(buttonRect, targetPosition, Color.white, new Color(0.73f, 0.73f, 0.73f)));
        skillButton.interactable = false;
    }

    private void PlacementPreview()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        if (currentPreview == null)
        {
            currentPreview = Instantiate(stunPreview);
            currentPreview.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 10, 0);
            currentPreview.transform.localScale = new Vector3(1 * (_skillRadius / 5), 1 * (_skillRadius / 5), 1 * (_skillRadius / 5));
        }

        currentPreview.transform.position = worldPosition;
    }

    public void CancelHeraSkill()
    {
        isReady = false;
        //GameManager.Instance.isASkillSelected = false;

        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }

        if (preStunSoundObject != null)
        {
            Destroy(preStunSoundObject);
            preStunSoundObject = null;
        }

        skillButton.interactable = true;
        UIManager.Instance.poseidonSkillCooldown.text = remainingCooldownTime <= 0 ? "READY" : $"{remainingCooldownTime:F1}s";
    }

    private void PlaySoundOnTempGameObject(AudioClip clip)
    {
        GameObject soundObject = new GameObject("StunSound");
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();

        tempAudioSource.clip = clip;
        tempAudioSource.ignoreListenerPause = true;
        tempAudioSource.volume = Random.Range(minVolumeSounds, maxVolumeSounds);
        tempAudioSource.pitch = Random.Range(minPitchSounds, maxPitchSounds);

        tempAudioSource.Play();

        Destroy(soundObject, clip.length);
    }

    private void PlayPreStunSFX(AudioClip clip)
    {
        preStunSoundObject = new GameObject("PreStunSound");
        AudioSource tempAudioSource = preStunSoundObject.AddComponent<AudioSource>();

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

    public void ResetCooldown()
    {
        remainingCooldownTime = 0f;
        isReady = false;

        UIManager.Instance.heraSkillCooldown.text = "READY";

        RectTransform buttonRect = skillButton.GetComponent<RectTransform>();
        StartCoroutine(MoveButton(buttonRect, buttonOriginalPosition, new Color(0.73f, 0.73f, 0.73f), Color.white));

        skillButton.interactable = true;

        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }

        if (preStunSoundObject != null)
        {
            Destroy(preStunSoundObject);
            preStunSoundObject = null;
        }
    }
}
