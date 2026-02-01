using UnityEngine;

[CreateAssetMenu(menuName = "Glyphs/Projectile Behaviors/Ice")]
public class IceProjectileBehavior : GlyphProjectileBehavior
{
    [Header("Ice Settings")]
    public float slowMultiplier = 0.5f;
    public float slowDuration = 2.5f;

    public override void OnSpawn(GameObject projectile)
    {
        var proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.ConfigureIce(slowMultiplier, slowDuration);
        }
    }
}