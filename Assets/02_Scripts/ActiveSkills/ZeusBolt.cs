using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class ZeusBolt : MonoBehaviour
{
    public GameObject boltPrefab;
    public GameObject boltPreview;
    private GameObject currentPreview;
    public LayerMask enemyLayer;

    [Space(10)]
    [Header("Game Design Values")]
    [Tooltip("The minimum damage the ability does")]
    [Min(0)]
    [SerializeField]
    public float damageLowerLimit = 90f;

    [Tooltip("The maximum damage the ability does")]
    [Min(0)]
    [SerializeField]
    public float damageUpperLimit = 110f;

    [Tooltip("Animationtime, it doesnt change the damage")]
    [Min(0)]
    [SerializeField]
    private float lightningDuration = 0.5f;

    [Tooltip("The time you have to wait before you can use the skill again")]
    [Min(0)]
    [SerializeField]
    public float cooldownTime = 20f;

    [Header("Game Design Values: UPGRADE / TOWER")]
    [SerializeField][Tooltip("Lower Damage Limit increase per Level/Zeustower - absolut value")] private float damageLowerLimitUpgrade;
    [SerializeField][Tooltip("Upper Damage Limit increase per Level/Zeustower - absolut value")] private float damageUpperLimitUpgrade;
    [SerializeField][Tooltip("Cooldown reduction per Level/Zeustower - absolut value")] private float cooldownReductionUpgrade;
    [Space(20)]

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

    [Space(20)]
    [Tooltip("The intensity of the camera shake")]
    [Min(0)]
    [SerializeField]
    private float _cameraShakeMagnitude = 0.1f;

    [Tooltip("The duration of the camera shake")]
    [Min(0)]
    [SerializeField]
    private float _cameraShakeDuration = 1f;

    [HideInInspector] public int zeusSkillLevel = 1;

    public AudioClip skillSound;
    public AudioClip preSkillSound;
    private GameObject preBoltSoundObject;

    public GameObject hitPS;

    private CameraShake _cameraShake;

    private float lastUseTime = -Mathf.Infinity;
    private bool isReady = false;

    private float remainingCooldownTime = 0f;

    private Vector2 buttonOriginalPosition; //BTN CD MOVE
    public Button skillButton; //BTN CD MOVE
    public Animator uiAnimation; //BTN CD MOVE
    public Image image; //BTN CD MOVE

    // for cancel other skills on activation
    public PoseidonWave poseidonWave;
    public HeraStun heraStun;
    public HephaistosQuake hephaistosQuake;

    private void Awake()
    {
        _cameraShake = Camera.main.GetComponent<CameraShake>();
    }
    private void Start()
    {
        buttonOriginalPosition = skillButton.GetComponent<RectTransform>().anchoredPosition;
        uiAnimation = uiAnimation.GetComponent<Animator>();
        image = image.GetComponent<Image>();
    }

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
                        buttonOriginalPosition, new Color(0.5f, 0.5f, 0.5f), Color.white)); //BTN CD MOVE
                    skillButton.interactable = true; //BTN CD MOVE

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
            if (isReady)
            {
                CancelZeusSkill(); // when active -> deactivate
            }
            else
            {
                ActivateZeusSkill(); // when not active -> activate
                poseidonWave.CancelPoseidonSkill();
                heraStun.CancelHeraSkill();
            }
        }

        if (isReady)
        {
            PlacementPreview();

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return; // avoid clicks on ui-elements

                TriggerLightning();
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelZeusSkill();
            }
        }
    }

    public void ActivateZeusSkill() //set isReady on true, activate the preview and the pre-sound
    {
        if (Time.timeScale == 0) return;

        if (remainingCooldownTime <= 0 && GameManager.Instance.isInWave)
        {
            if (isReady)
            {
                CancelZeusSkill();
            }
            else
            {
                isReady = true;
                if (currentPreview == null)
                {
                    currentPreview = Instantiate(boltPreview);
                    PlayPreBoltSFX(preSkillSound);
                }

                poseidonWave.CancelPoseidonSkill();
                heraStun.CancelHeraSkill();
            }
        }
    }

    private void TriggerLightning()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        Vector2 rayOrigin = worldPosition;
        Vector2 rayDirection = (Vector2)worldPosition - rayOrigin;
        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, rayDirection, Mathf.Infinity, enemyLayer);

        if (hits.Length > 0)
        {
            // Finde den nächsten Gegner zur Maus
            RaycastHit2D closestHit = hits.OrderBy(h => Vector2.Distance(h.point, worldPosition)).First();

            if (closestHit.collider != null && closestHit.collider.CompareTag("Enemy"))
            {
                GameObject bolt = Instantiate(boltPrefab, new Vector3(closestHit.point.x, closestHit.point.y + 10, 0), Quaternion.identity);
                GameObject zeus = Instantiate(hitPS, worldPosition, Quaternion.identity);
                PlayBoltSFX(skillSound);
                Destroy(bolt, lightningDuration);

                closestHit.collider.GetComponent<EnemyManager>().TakeDamage(Mathf.RoundToInt(Random.Range(damageLowerLimit, damageUpperLimit)));

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
                StartCoroutine(MoveButton(buttonRect, targetPosition, Color.white, new Color(0.5f, 0.5f, 0.5f)));
                skillButton.interactable = false;

                if (_cameraShake == null)
                {
                    _cameraShake = FindFirstObjectByType<CameraShake>();
                }

                if (_cameraShake != null)
                {
                    StartCoroutine(_cameraShake.Shake(_cameraShakeDuration, _cameraShakeMagnitude));
                }
            }
        }
    }

    private void PlacementPreview()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;
        worldPosition.y += 10;

        if (currentPreview == null)
        {
            currentPreview = Instantiate(boltPreview);
        }

        currentPreview.transform.position = worldPosition;
    }

    public void CancelZeusSkill()
    {
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

        skillButton.interactable = true;
        UIManager.Instance.zeusSkillCooldown.text = remainingCooldownTime <= 0 ? "READY" : $"{remainingCooldownTime:F1}s";
    }

    private void PlayBoltSFX(AudioClip clip)
    {
        GameObject soundObject = new GameObject("BoltSound");
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();

        tempAudioSource.clip = clip;
        tempAudioSource.ignoreListenerPause = false;
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
        tempAudioSource.ignoreListenerPause = false;
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

        UIManager.Instance.zeusSkillCooldown.text = "READY";

        RectTransform buttonRect = skillButton.GetComponent<RectTransform>();
        StartCoroutine(MoveButton(buttonRect, buttonOriginalPosition, new Color(0.5f, 0.5f, 0.5f), Color.white));

        skillButton.interactable = true;

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
    }

    public void UpgradeBolt()
    {
        Debug.Log("ZeusBolt upgraded");
        damageLowerLimit += (damageLowerLimitUpgrade);
        damageUpperLimit += (damageUpperLimitUpgrade);
        cooldownTime -= cooldownReductionUpgrade; //OPTIONAL: Mathf.clamp um Cooldown bspw. auf 1/2 des urpsrgl. CDs zu beschränken
        //Multiplikator mit GameManager.Instance.zeusTower; nicht notwendig 
        //da Upgrade mit dem Platzieren/Upgraden eines Turmes jedes Mal aufgerufen wird
    }
}
