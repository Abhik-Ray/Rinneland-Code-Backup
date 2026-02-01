using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "GlyphCombo",
    menuName = "Glyphs/Glyph Combo"
)]
public class GlyphComboData : ScriptableObject
{
    public string comboName;

    [Tooltip("Order matters")]
    public List<GlyphType> sequence;

    public GameObject projectilePrefab;
    public GlyphProjectileBehavior behavior;

}
