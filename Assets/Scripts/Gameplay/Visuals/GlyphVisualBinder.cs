using UnityEngine;

public class GlyphVisualBinder : MonoBehaviour
{
    [SerializeField] private GlyphManager glyphManager;
    [SerializeField] private GlyphCrossView glyphCrossView;

    private void OnEnable()
    {
        glyphManager.OnGlyphAdded += glyphCrossView.OnGlyphAdded;
        glyphManager.OnGlyphsCleared += glyphCrossView.ClearAllGlyphs;
    }

    private void OnDisable()
    {
        glyphManager.OnGlyphAdded -= glyphCrossView.OnGlyphAdded;
        glyphManager.OnGlyphsCleared -= glyphCrossView.ClearAllGlyphs;
    }
}
