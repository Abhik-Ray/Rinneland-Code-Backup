using System.Collections.Generic;
using UnityEngine;

public class GlyphCrossView : MonoBehaviour
{
    [Header("Anchors")]
    [SerializeField] private Transform triangleAnchor;
    [SerializeField] private Transform circleAnchor;
    [SerializeField] private Transform squareAnchor;
    [SerializeField] private Transform xAnchor;

    [Header("Prefab")]
    [SerializeField] private GlyphView glyphViewPrefab;

    [Header("Visual Data")]
    [SerializeField] private List<GlyphVisualData> visualDataList;

    private Dictionary<GlyphType, GlyphVisualData> visualLookup;

    private Dictionary<GlyphType, List<GlyphView>> activeGlyphs;

    [SerializeField] private float stackOffset = 0.02f;

    private void Awake()
    {
        visualLookup = new Dictionary<GlyphType, GlyphVisualData>();
        activeGlyphs = new Dictionary<GlyphType, List<GlyphView>>();

        foreach (var data in visualDataList)
        {
            visualLookup[data.glyphType] = data;
            activeGlyphs[data.glyphType] = new List<GlyphView>();
        }
    }

    public void SpawnGlyph(GlyphType glyph)
    {
        var anchor = GetAnchor(glyph);
        var visualData = visualLookup[glyph];

        var view = Instantiate(glyphViewPrefab, anchor);

        int stackIndex = activeGlyphs[glyph].Count;

        view.transform.localPosition = new Vector3(
            0f,
            0f,
            stackIndex * stackOffset
        );

        view.transform.localRotation = Quaternion.identity;
        view.Initialize(visualData.mesh, visualData.material);

        activeGlyphs[glyph].Add(view);
    }


    private Transform GetAnchor(GlyphType glyph)
    {
        return glyph switch
        {
            GlyphType.Triangle => triangleAnchor,
            GlyphType.Circle => circleAnchor,
            GlyphType.Square => squareAnchor,
            GlyphType.X => xAnchor,
            _ => null
        };
    }

    public void OnGlyphAdded(GlyphType glyph)
    {
        SpawnGlyph(glyph);
    }

    public void ClearAllGlyphs()
    {
        foreach (var kvp in activeGlyphs)
        {
            foreach (var glyphView in kvp.Value)
            {
                Destroy(glyphView.gameObject);
            }

            kvp.Value.Clear();
        }
    }

}
