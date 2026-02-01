using UnityEngine;

public class PooledHitEffect : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        ps.Clear(true);
        ps.Play(true);
    }
}
