using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private AudioClip hitSFX;

    private Transform owner;
    private ObjectPool pool;
    private bool isReleased;
    private float lifeTimer;
    private TrailRenderer trail;
    // Ice config
    private bool isIce;
    private float iceSlowMultiplier;
    private float iceSlowDuration;




    public void SetPool(ObjectPool poolRef)
    {
        pool = poolRef;
    }

    public void SetOwner(Transform ownerTransform)
    {
        owner = ownerTransform;
    }

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        trail = GetComponent<TrailRenderer>();
    }

    public void Attach(Transform socket)
    {
        isReleased = false;
        lifeTimer = 0f;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        rb.Sleep();

        if (trail != null)
        {
            trail.Clear();
            trail.emitting = false; // 🔴 important
        }

        transform.SetParent(socket);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Release(Vector3 direction)
    {
        if (isReleased)
            return;

        isReleased = true;

        // Detach immediately
        transform.SetParent(null, true);

        // Reset physics for this frame
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.Sleep();

        // Schedule real release next frame
        StartCoroutine(ReleaseNextFrame(direction));
    }

    private IEnumerator ReleaseNextFrame(Vector3 direction)
    {
        yield return null; // 🔴 CRITICAL

        transform.rotation = Quaternion.LookRotation(direction.normalized);

        rb.isKinematic = false;
        rb.WakeUp();

        rb.linearVelocity = direction.normalized * speed;

        if (trail != null)
        {
            trail.Clear();
            trail.emitting = true;
        }
    }



    private void Update()
    {
        if (!isReleased)
            return;

        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifetime)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        StopAllCoroutines(); // 🔴 important

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        rb.Sleep();

        // 🔴 FULL transform reset
        transform.SetParent(null);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Other Resets
        isIce = false;
        iceSlowMultiplier = 0f;
        iceSlowDuration = 0f;

        if (trail != null)
        {
            trail.Clear();
            trail.emitting = false;
        }

        isReleased = false;

        if (pool != null)
            pool.Return(gameObject);
        else
            Destroy(gameObject);
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Projectile collided with " + other.name);
        // Ignore the shooter
        if (other.transform == owner ||
            other.transform.root == owner)
            return;

        SpawnHitEffect(
            other.bounds.ClosestPoint(transform.position)
        );

        PlayHitSound(transform.position);

        // Player damage
        var playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            ReturnToPool();
            return;
        }

        // Enemy damage
        var enemyHealth = other.GetComponentInParent<EnemyHealth>();
        var enemyController = other.GetComponentInParent<EnemyController>();
        if (enemyController != null)
        {
            Debug.Log("Projectile hit enemy controller.");
            if (isIce)
            {
                Debug.Log("Applying ice slow to enemy.");
                enemyController.ApplySlow(iceSlowMultiplier, iceSlowDuration);
            }
        }


        if (enemyHealth != null)
        {
            Debug.Log("Projectile hit enemy for " + damage + " damage.");
            enemyHealth.TakeDamage(damage);
            ReturnToPool();
            return;
        }

        ReturnToPool(); // 🔴 SINGLE EXIT POINT
    }


    private void SpawnHitEffect(Vector3 position)
    {
        if (hitEffectPrefab == null)
            return;

        var effect = Instantiate(
            hitEffectPrefab,
            position,
            Quaternion.identity
        );

        Destroy(effect, 1f); // short lifetime is perfect for sparks
    }

    private void PlayHitSound(Vector3 position)
    {
        if (hitSFX == null)
            return;

        AudioSource.PlayClipAtPoint(hitSFX, position, 0.8f);
    }

    public void SetIceBehavior()
    {
        isIce = true;
    }

    public void SetGrenadeBehavior()
    {
        rb.useGravity = true;
        // StartCoroutine(ExplodeAfterDelay());
    }

    public void SetChainLightningBehavior()
    {
        // canChain = true;
    }

    public void ConfigureIce(float slowMultiplier, float duration)
    {
        isIce = true;
        iceSlowMultiplier = slowMultiplier;
        iceSlowDuration = duration;
    }

}
