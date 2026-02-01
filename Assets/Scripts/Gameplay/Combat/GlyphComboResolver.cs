using System.Collections.Generic;
using UnityEngine;

public class GlyphComboResolver : MonoBehaviour
{
    [SerializeField] private List<GlyphComboData> combos;

    public GlyphComboData Resolve(IReadOnlyList<GlyphType> glyphQueue)
    {
        GlyphComboData bestMatch = null;
        int bestLength = 0;

        foreach (var combo in combos)
        {
            if (combo.sequence.Count > glyphQueue.Count)
                continue;

            if (Matches(combo.sequence, glyphQueue))
            {
                if (combo.sequence.Count > bestLength)
                {
                    bestMatch = combo;
                    bestLength = combo.sequence.Count;
                }
            }
        }

        return bestMatch;
    }

    private bool Matches(
        List<GlyphType> combo,
        IReadOnlyList<GlyphType> queue)
    {
        // Compare from the START of the queue
        for (int i = 0; i < combo.Count; i++)
        {
            if (queue[i] != combo[i])
                return false;
        }

        return true;
    }
}
