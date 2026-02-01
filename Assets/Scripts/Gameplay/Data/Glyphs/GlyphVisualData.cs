using UnityEngine;

[CreateAssetMenu(
    fileName = "GlyphVisualData",
    menuName = "Glyphs/Glyph Visual Data"
)]
public class GlyphVisualData : ScriptableObject
{
    public GlyphType glyphType;
    public Mesh mesh;
    public Material material;
}
