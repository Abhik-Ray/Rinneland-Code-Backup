using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    private EnemyController enemyController;

    private void Awake()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    // This MUST match the Animation Event name
    public void ReleaseProjectile()
    {
        if (enemyController != null)
        {
            enemyController.ReleaseProjectile();
        }
    }
}
