using UnityEngine;

[CreateAssetMenu(menuName = "Glyphs/Projectile Behaviors/Grenade")]
public class GrenadeProjectileBehavior : GlyphProjectileBehavior
{
    public override void OnSpawn(GameObject projectile)
    {
        var proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetGrenadeBehavior();
        }
    }
}
