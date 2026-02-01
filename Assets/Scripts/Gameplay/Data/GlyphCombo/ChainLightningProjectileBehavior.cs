using UnityEngine;

[CreateAssetMenu(menuName = "Glyphs/Projectile Behaviors/Chain Lightning")]
public class ChainLightningProjectileBehavior : GlyphProjectileBehavior
{
    public override void OnSpawn(GameObject projectile)
    {
        var proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetChainLightningBehavior();
        }
    }
}
