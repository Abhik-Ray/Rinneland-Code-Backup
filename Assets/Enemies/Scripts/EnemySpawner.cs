using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        foreach (var point in spawnPoints)
        {
            Instantiate(enemyPrefab, point.position, point.rotation);
        }
    }
}
