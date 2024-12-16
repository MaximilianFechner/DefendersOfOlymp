using UnityEngine;
using System.Collections.Generic;

public class PlayerLife : MonoBehaviour
{
    public GameObject heartPrefab;

    private List<GameObject> hearts = new List<GameObject>();

    private void Start()
    {
        InitializeLives(GameManager.Instance.ReturnLives());
    }

    public void InitializeLives(int lives)
    {
        ClearHearts();
        for (int i = 0; i < lives; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, transform);
            hearts.Add(newHeart);
        }
    }
    public void UpdateLives(int currentLives)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < currentLives);
        }
    }

    private void ClearHearts()
    {
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();
    }
}
