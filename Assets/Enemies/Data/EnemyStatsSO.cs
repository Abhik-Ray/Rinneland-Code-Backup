using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyStats",
    menuName = "Game/Enemies/Enemy Stats"
)]
public class EnemyStatsSO : ScriptableObject
{
    [Header("Health")]
    public float maxHealth = 30f;

    [Header("Detection")]
    public float detectionRadius = 12f;

    [Header("Movement")]
    public float moveSpeed = 3.5f;

    [Header("Combat (later)")]
    public float attackRange = 8f;
    public float fireCooldown = 1.4f;
}
