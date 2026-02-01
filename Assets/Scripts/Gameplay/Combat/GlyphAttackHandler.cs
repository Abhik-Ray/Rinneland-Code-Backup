using UnityEngine;
using UnityEngine.InputSystem;

public class GlyphAttackHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private GlyphManager glyphManager;
    [SerializeField] private GlyphComboResolver comboResolver;
    [SerializeField] private Transform firePoint;
    [SerializeField] private AudioClip spellCastSFX;


    private InputAction attackAction;

    private void Awake()
    {
        var playerMap = inputActions.FindActionMap("Player", true);
        attackAction = playerMap.FindAction("Attack", true);
    }

    private void OnEnable()
    {
        attackAction.Enable();
        attackAction.performed += OnAttack;
    }

    private void OnDisable()
    {
        attackAction.performed -= OnAttack;
        attackAction.Disable();
    }

    private void OnAttack(InputAction.CallbackContext _)
    {
        var combo = comboResolver.Resolve(glyphManager.GlyphQueue);

        if (combo)
            FireProjectile(combo);

        glyphManager.ClearGlyphs();
    }

    private void FireProjectile(GlyphComboData combo)
    {
        var projectileGO =
            PoolRegistry_Player.PlayerProjectilePool.Get();
        
        // combo.behavior.OnSpawn(projectileGO);

        projectileGO.transform.position = firePoint.position;
        projectileGO.transform.rotation = firePoint.rotation;

        var projectile = projectileGO.GetComponent<Projectile>();

        projectile.SetPool(PoolRegistry_Player.PlayerProjectilePool);
        projectile.SetOwner(transform);

        // 🔴 IMPORTANT: release immediately
        projectile.Release(firePoint.forward);

        AudioSource.PlayClipAtPoint(
            spellCastSFX,
            firePoint.position,
            0.8f
        );

        Debug.Log($"Fired projectile for combo: {combo.comboName}");
    }

}
