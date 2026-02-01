using System;
using System.Collections.Generic;
using UnityEngine;

public class GlyphManager : MonoBehaviour
{
    public const int MaxGlyphs = 4;

    [SerializeField] private float glyphManaCost = 10f;

    private readonly List<GlyphType> _glyphQueue = new();
    private PlayerMana playerMana;

    public IReadOnlyList<GlyphType> GlyphQueue => _glyphQueue;

    public event Action<GlyphType> OnGlyphAdded;
    public event Action OnGlyphsCleared;
    private GlyphAudio glyphAudio;
    private bool glyphTutorialShown;
    private bool introTutorialShown;



    private void Awake()
    {
        playerMana = FindFirstObjectByType<PlayerMana>();

        glyphAudio = FindFirstObjectByType<GlyphAudio>();

    }

    private void Start()
    {
        if (!introTutorialShown)
        {
            FindFirstObjectByType<TutorialPopupController>()?.Show("Press 1–4 to draw glyphs");
            introTutorialShown = true;
        }
    }

    public void AddGlyph(GlyphType glyph)
    {
        if (_glyphQueue.Count >= MaxGlyphs)
            return;

        if (playerMana == null)
        {
            Debug.LogWarning("PlayerMana not found");
            return;
        }

        // 🔴 MANA CHECK + CONSUME
        if (!playerMana.Consume(glyphManaCost))
        {
            Debug.Log("Not enough mana to add glyph");
            return;
        }

        if (glyphAudio != null)
            glyphAudio.PlayScribble();

        _glyphQueue.Add(glyph);
        OnGlyphAdded?.Invoke(glyph);

        if (!glyphTutorialShown)
        {
            var popup = FindFirstObjectByType<TutorialPopupController>();
            if (popup != null)
                popup.Show("Press Left Click to cast your spell");

            glyphTutorialShown = true;
        }


        Debug.Log($"Glyph added: {glyph} (Mana spent: {glyphManaCost})");
    }

    public void ClearGlyphs()
    {
        _glyphQueue.Clear();
        OnGlyphsCleared?.Invoke();

        Debug.Log("Glyphs cleared");
    }
}
