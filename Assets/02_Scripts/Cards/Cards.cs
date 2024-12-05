using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/New Card")]
public class Cards : ScriptableObject
{
    public string TowerName; // Name der Karte
    public Sprite CardSprite; // Sprite der Karte
    public GameObject TowerPrefab; // Prefab des Turms
}
