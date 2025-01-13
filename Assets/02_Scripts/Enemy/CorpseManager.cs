using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CorpseManager : MonoBehaviour
{
    [Header("Game Design Values")]
    [Tooltip("How many corpses of dead enemies laying in the level at the same time")]
    [Min(1)]
    public int maxCorpses = 3;

    public Queue<GameObject> corpseQueue = new Queue<GameObject>();

    private void Update()
    {
        if (corpseQueue.Count <= maxCorpses) return;

        GameObject oldestCorpse = corpseQueue.Dequeue();
        CorpseEnemy fadeOut = oldestCorpse.GetComponent<CorpseEnemy>();

        if (fadeOut != null)
        {
            fadeOut.StartFadeAndDestroy();
        }
        else
        {
            Destroy(oldestCorpse);
        }
    }
}

