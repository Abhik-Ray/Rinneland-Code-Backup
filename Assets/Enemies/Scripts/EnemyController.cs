using UnityEngine;
using UnityEngine.AI;
using StarterAssets;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyStatsSO stats;
    private Transform player;

    private NavMeshAgent agent;
    private EnemyHealth health;
    private EnemyHealthBar healthBar;
    private ObjectPool projectilePool;
    [SerializeField] private float repathInterval = 0.3f;
    [SerializeField] private AudioClip spellCastSFX;
    private EnemyAudio enemyAudio;


    private float nextRepathTime;
    private bool isDead;


    private enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }

    private EnemyState state = EnemyState.Idle;

    [Header("Line of Sight")]
    [SerializeField] private Transform eyePoint;
    [SerializeField] private LayerMask lineOfSightMask;

    [Header("Combat")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private float fireCooldown = 1.5f;
    // [SerializeField] private float projectileSpeed = 12f;
    [Header("Combat")]
    [SerializeField] private Transform projectileSocket;
    private Animator animator;
    private float lastFireTime;
    private bool isAiming;
    private Projectile currentProjectile;
    private Transform playerAimTarget;

    private Coroutine slowRoutine;
    private float originalSpeed;
    [SerializeField] private GameObject iceAura;
    [SerializeField] private ParticleSystem iceParticles;





    private ObjectPool ProjectilePool
    {
        get
        {
            if (projectilePool == null)
            {
                projectilePool = PoolRegistry.EnemyProjectilePool;

                if (projectilePool == null)
                {
                    Debug.LogError("EnemyProjectilePool not ready!", this);
                }
            }
            return projectilePool;
        }
    }



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        // projectilePool = PoolRegistry.EnemyProjectilePool;
        animator = GetComponentInChildren<Animator>();
        enemyAudio = GetComponent<EnemyAudio>();


        agent.updateRotation = false;

        if (healthBar != null)
        {
            health.OnHealthChanged += healthBar.SetHealth;
        }

        health.OnDeath += OnDeath;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            var playerController = playerObject.GetComponent<FirstPersonController>();
            if (playerController != null)
            {
                playerAimTarget = playerController.AimTarget;
            }
        }

        agent.speed = stats.moveSpeed;
    }

    private void OnDeath()
    {
        if (isDead)
            return;

        isDead = true;

        // Stop AI & navigation
        agent.isStopped = true;
        agent.enabled = false;

        // Stop combat state
        isAiming = false;
        currentProjectile = null;

        // Trigger death animation
        animator.SetTrigger("Die");

        // Disable colliders
        foreach (var col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        // Disable health bar
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }
    }

    private void SpawnAttachedProjectile()
    {
        if (player == null)
            return;

        var projectileGO = ProjectilePool.Get();
        projectileGO.transform.position = projectileSocket.position;
        projectileGO.transform.rotation = projectileSocket.rotation;

        currentProjectile = projectileGO.GetComponent<Projectile>();
        currentProjectile.SetOwner(transform);
        currentProjectile.Attach(projectileSocket);
    }

    public void ReleaseProjectile()
    {
        if (currentProjectile == null || player == null)
            return;

        // Vector3 direction =
        //     (player.position - projectileSocket.position).normalized;
        Vector3 direction = (playerAimTarget.position - projectileSocket.position).normalized;

        currentProjectile.Release(direction);
        AudioSource.PlayClipAtPoint(
            spellCastSFX,
            projectileSocket.position,
            0.8f
        );


        currentProjectile = null;
        lastFireTime = Time.time;
        isAiming = false;

        animator.SetBool("IsAttacking", false);
    }

    private void ExitAttackState()
    {
        isAiming = false;
        animator.SetBool("IsAttacking", false);
    }


    private void Update()
    {
        if (isDead)
            return;

        if (player == null)
            return;

        if (animator.GetBool("IsAttacking"))
        {
            agent.isStopped = true;
            animator.SetBool("isMoving", false);

            FacePlayer();
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance > stats.detectionRadius)
        {
            state = EnemyState.Idle;
        }
        else if (distance > stats.attackRange)
        {
            state = EnemyState.Chase;
        }
        else
        {
            state = HasLineOfSight()
                ? EnemyState.Attack
                : EnemyState.Chase;
        }

        // Debug.Log($"State: {state}, Distance: {distance}");

        switch (state)
        {
            case EnemyState.Idle:
                agent.isStopped = true;
                animator.SetBool("isMoving", false);

                // ExitAttackState();

                break;

            case EnemyState.Chase:
                agent.isStopped = false;

                animator.SetBool("isMoving", true);

                FaceMovementDirection();

                if (enemyAudio)
                    enemyAudio.PlayRoar();

                // ExitAttackState();

                if (Time.time >= nextRepathTime)
                {
                    Vector3 targetPos = player.position;

                    if (NavMesh.SamplePosition(
                            targetPos,
                            out NavMeshHit hit,
                            2.0f,
                            NavMesh.AllAreas))
                    {
                        agent.SetDestination(hit.position);
                    }
                    nextRepathTime = Time.time + repathInterval;
                }
                break;

            case EnemyState.Attack:
                // Debug.Log("Attacking");
                agent.isStopped = true;

                animator.SetBool("isMoving", false);

                FacePlayer();
                TryAttack();

                break;

        }
        UpdateAnimatorMovement();
    }

    private void FaceMovementDirection()
    {
        Vector3 velocity = agent.velocity;
        velocity.y = 0f;

        if (velocity.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(velocity);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 10f
        );
    }


    private bool HasLineOfSight()
    {
        if (player == null)
            return false;

        Vector3 direction =
            (player.position - eyePoint.position).normalized;

        float distance =
            Vector3.Distance(eyePoint.position, player.position);

        if (Physics.Raycast(
            eyePoint.position,
            direction,
            out RaycastHit hit,
            distance,
            lineOfSightMask
        ))
        {
            return hit.transform.root.CompareTag("Player");
        }

        return false;
    }

    private void FireProjectile()
    {
        if (player == null)
            return;

        // Face the player before firing
        Vector3 flatLookDir = player.position - transform.position;
        flatLookDir.y = 0f;

        transform.rotation = Quaternion.LookRotation(flatLookDir);


        var projectile = projectilePool.Get();
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;


        projectile.GetComponent<Projectile>()
          .SetOwner(transform);

        // Give projectile velocity
        // var rb = projectile.GetComponent<Rigidbody>();
        // if (rb != null)
        // {
        //     Vector3 shootDir =
        //     (player.position - firePoint.position).normalized;

        //     rb.linearVelocity = shootDir * projectileSpeed;
        // }

        lastFireTime = Time.time;
        isAiming = false;
    }


    private void TryAttack()
    {
        if (animator.GetBool("IsAttacking"))
            return;

        if (Time.time < lastFireTime + fireCooldown)
        {
            // Debug.Log("On cooldown");
            return;
        }

        if (!isAiming)
        {
            isAiming = true;
            animator.SetBool("IsAttacking", true);
            SpawnAttachedProjectile();
        }
    }

    private void UpdateAnimatorMovement()
    {
        bool moving = agent.velocity.sqrMagnitude > 0.1f;
        animator.SetBool("isMoving", moving);
    }

    private void FacePlayer()
    {
        if (player == null)
            return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f; // keep upright

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 10f
        );
    }

    public void ApplySlow(float multiplier, float duration)
    {
        if (slowRoutine != null)
            StopCoroutine(slowRoutine);

        slowRoutine = StartCoroutine(SlowRoutine(multiplier, duration));
        ApplyIceVisual(true);
    }


    private IEnumerator SlowRoutine(float multiplier, float duration)
    {
        if (agent == null)
            yield break;

        iceParticles?.Play();

        float originalSpeed = agent.speed;
        agent.speed = Mathf.Max(0.1f, originalSpeed * multiplier);

        yield return new WaitForSeconds(duration);

        agent.speed = originalSpeed;

        iceParticles?.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        slowRoutine = null;
    }


    private void ApplyIceVisual(bool frozen)
    {

    }


}
