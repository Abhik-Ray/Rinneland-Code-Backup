using UnityEngine;

public class PoolRegistry_Player : MonoBehaviour
{
    public static ObjectPool PlayerProjectilePool;

    private void Awake()
    {
        PlayerProjectilePool = GetComponent<ObjectPool>();
    }
}
