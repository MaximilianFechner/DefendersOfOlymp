using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverManager : MonoBehaviour
{

    public GameObject[] torchLights;
    public GameObject[] torchFires;
    public GameObject gameOverPanel;
    public GameObject fog;

    public Image gameOver;
    public Button tryAgain;
    public Button mainMenu;
    public Button quit;

    public GameObject[] spawnPoints;
    public GameObject[] goEnemies;

    // fog target position local 2150, 0, 0
    // fog start position local -1600, 0, 0
    private Vector3 startPosition = new Vector3(-2400f, 0f, 0f);
    private Vector3 targetPosition = new Vector3(1050f, 0f, 0f);
    public float moveSpeed = 1f; // fog movespeed

    private Coroutine moveFogCoroutine;
    private Coroutine spawnHordeCoroutine;
    private Coroutine changeTorchColorsAndScaleLightsCoroutine;
    private Coroutine changeTorchColorsAndScaleFiresCoroutine;

    void Start()
    {
        gameOverPanel.SetActive(false);
        fog.SetActive(false);

        //GameObject torchContainer = GameObject.Find("TorchContainer");
        //if (torchContainer != null)
        //{
        //    DontDestroyOnLoad(torchContainer);
        //}
        //else
        //{
        //    Debug.LogWarning("TorchContainer not found in the scene!");
        //}
    }

    public IEnumerator MoveFog()
    {
        gameOverPanel.SetActive(true);
        fog.SetActive(true);

        fog.transform.localPosition = startPosition;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (fog.transform.localPosition != targetPosition)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            fog.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            yield return null;
        }
    }

    public void TriggerGameOver()
    {
        StartCoroutine(MoveFog());
        StartCoroutine(SpawnHorde());
        StartCoroutine(ChangeTorchColorsAndScale(torchLights, true));
        StartCoroutine(ChangeTorchColorsAndScale(torchFires, false));
        StartCoroutine(FadeInUI());
    }

    private IEnumerator SpawnHorde()
    {
        while (GameManager.Instance.isGameOver)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SpawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, goEnemies.Length);
        int randomSpawnerIndex = Random.Range(0, spawnPoints.Length);
        GameObject enemyPrefab = goEnemies[randomEnemyIndex];
        Instantiate(enemyPrefab, spawnPoints[randomSpawnerIndex].transform.position, Quaternion.identity);
    }

    private IEnumerator ChangeTorchColorsAndScale(GameObject[] torches, bool shouldScale)
    {
        int phaseSize = 4;
        int index = 0;

        while (index < torches.Length)
        {
            yield return new WaitForSeconds(1.25f);

            int batchSize = Mathf.Min(phaseSize, torches.Length - index);
            float delay = Mathf.Lerp(0.4f, 0.7f, (float)index / torches.Length);

            for (int i = 0; i < batchSize; i++)
            {
                GameObject torch = torches[index + i];
                StartCoroutine(ChangeTorchColorAndScale(torch, shouldScale));
            }

            yield return new WaitForSeconds(delay);

            if (phaseSize == 4)
            {
                phaseSize = 2;
            }

            index += batchSize;
        }
    }

    private IEnumerator ChangeTorchColorAndScale(GameObject torch, bool shouldScale)
    {
        SpriteRenderer torchRenderer = torch.GetComponent<SpriteRenderer>();
        torchRenderer.color = Color.red;

        if (shouldScale)
        {
            Vector3 originalScale = torch.transform.localScale;
            torch.transform.localScale = originalScale * 2f;
        }

        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator FadeInUI()
    {
        StartCoroutine(FadeImage(gameOver, 25));
        StartCoroutine(FadeButton(tryAgain, 10f));
        StartCoroutine(FadeButton(mainMenu, 10f));
        StartCoroutine(FadeButton(quit, 10f));
        yield return null;
    }

    private IEnumerator FadeImage(Image img, float duration)
    {
        Color color = img.color;
        float startAlpha = 0f;
        float endAlpha = color.a;

        img.color = new Color(color.r, color.g, color.b, startAlpha);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            img.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }
    }

    private IEnumerator FadeButton(Button button, float duration)
    {
        Image buttonImage = button.GetComponent<Image>();
        Text buttonText = button.GetComponentInChildren<Text>();

        if (buttonImage != null)
        {
            StartCoroutine(FadeImage(buttonImage, duration));
        }
        if (buttonText != null)
        {
            StartCoroutine(FadeText(buttonText, duration));
        }

        yield return null;
    }

    private IEnumerator FadeText(Text txt, float duration)
    {
        Color color = txt.color;
        float startAlpha = 0f;
        float endAlpha = color.a;

        txt.color = new Color(color.r, color.g, color.b, startAlpha);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            txt.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }
    }

    public void StopGameOverEffects()
    {
        if (moveFogCoroutine != null)
        {
            StopCoroutine(moveFogCoroutine);
        }
        if (spawnHordeCoroutine != null)
        {
            StopCoroutine(spawnHordeCoroutine);
        }
        if (changeTorchColorsAndScaleLightsCoroutine != null)
        {
            StopCoroutine(changeTorchColorsAndScaleLightsCoroutine);
        }
        if (changeTorchColorsAndScaleFiresCoroutine != null)
        {
            StopCoroutine(changeTorchColorsAndScaleFiresCoroutine);
        }

        fog.SetActive(false);
        gameOverPanel.SetActive(false);

        ResetTorchValues();
    }

    private void ResetTorchValues()
    {
        foreach (GameObject torch in torchLights)
        {
            if (torch != null)
            {
                SpriteRenderer torchRenderer = torch.GetComponent<SpriteRenderer>();
                if (torchRenderer != null)
                {
                    torchRenderer.color = new Color(255f / 255f, 143f / 255f, 40f / 255f); // Standardfarbe
                }
                torch.transform.localScale = new Vector3(3f, 3f, 3f);
            }
        }

        foreach (GameObject fire in torchFires)
        {
            if (fire != null)
            {
                SpriteRenderer fireRenderer = fire.GetComponent<SpriteRenderer>();
                if (fireRenderer != null)
                {
                    fireRenderer.color = Color.white;
                }
                fire.transform.localScale = new Vector3(0.2f, 0.15f, 1f); // Standardgröße
            }
        }
    }
}