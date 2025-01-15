using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    [Header("Game Design Values")]
    [Tooltip("How many blood pools can exists in the level at the same time")]
    [Min(1)]
    public int maxBloodPools = 100;

    public Queue<GameObject> bloodQueue = new Queue<GameObject>();

    private void Update()
    {
        if (bloodQueue.Count <= maxBloodPools) return;

        GameObject oldestBlood = bloodQueue.Dequeue();
        BloodPools fadeOut = oldestBlood.GetComponent<BloodPools>();

        if (fadeOut != null)
        {
            fadeOut.StartFadeAndDestroy();
        }
        else
        {
            Destroy(oldestBlood);
        }
    }
}
