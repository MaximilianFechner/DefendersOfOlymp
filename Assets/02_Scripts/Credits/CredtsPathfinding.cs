using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Runtime.CompilerServices;

public class CredtsPathfinding : MonoBehaviour
{
    public Transform[] rectangleCorners;
    public float speed = 2f;

    private int currentCornerIndex = 0;

    private void Update()
    {
        if (rectangleCorners.Length < 4)
        {
            Debug.LogError("Das Rechteck benötigt genau 4 Ecken!");
            return;
        }

        Transform targetCorner = rectangleCorners[currentCornerIndex];

        transform.position = Vector3.MoveTowards(transform.position, targetCorner.position, speed * Time.deltaTime);

        FlipBasedOnMovement(targetCorner.position - transform.position);

        if (Vector3.Distance(transform.position, targetCorner.position) < 0.1f)
        {
            currentCornerIndex = (currentCornerIndex + 1) % rectangleCorners.Length;
        }
    }

    private void FlipBasedOnMovement(Vector3 movementDirection)
    {
        if (movementDirection.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
