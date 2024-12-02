using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "Scriptable Objects/ProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    public GameObject projectilePrefab;

    public float movementSpeed;
    public float damage;
    public float aoeRadius;
    public float slowValue;
}
