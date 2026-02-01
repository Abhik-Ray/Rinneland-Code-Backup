using UnityEngine;

public class PoolRegistry : MonoBehaviour
{
    public static ObjectPool EnemyProjectilePool;

    private void Awake()
    {
        EnemyProjectilePool = GetComponent<ObjectPool>();
    }
}
