using UnityEngine;
using UnityEngine.InputSystem;

public class GlyphInputListener : MonoBehaviour
{
    [SerializeField] private GlyphManager glyphManager;
    [SerializeField] private InputActionAsset inputActions;

    private InputAction glyph1;
    private InputAction glyph2;
    private InputAction glyph3;
    private InputAction glyph4;

    private void Awake()
    {
        var glyphMap = inputActions.FindActionMap("Glyphs", true);

        glyph1 = glyphMap.FindAction("Glyph1", true);
        glyph2 = glyphMap.FindAction("Glyph2", true);
        glyph3 = glyphMap.FindAction("Glyph3", true);
        glyph4 = glyphMap.FindAction("Glyph4", true);
    }

    private void OnEnable()
    {
        glyph1.Enable();
        glyph2.Enable();
        glyph3.Enable();
        glyph4.Enable();

        glyph1.performed += OnGlyph1;
        glyph2.performed += OnGlyph2;
        glyph3.performed += OnGlyph3;
        glyph4.performed += OnGlyph4;
    }

    private void OnDisable()
    {
        glyph1.performed -= OnGlyph1;
        glyph2.performed -= OnGlyph2;
        glyph3.performed -= OnGlyph3;
        glyph4.performed -= OnGlyph4;

        glyph1.Disable();
        glyph2.Disable();
        glyph3.Disable();
        glyph4.Disable();
    }

    private void OnGlyph1(InputAction.CallbackContext _) =>
        glyphManager.AddGlyph(GlyphType.Triangle);

    private void OnGlyph2(InputAction.CallbackContext _) =>
        glyphManager.AddGlyph(GlyphType.Circle);

    private void OnGlyph3(InputAction.CallbackContext _) =>
        glyphManager.AddGlyph(GlyphType.Square);

    private void OnGlyph4(InputAction.CallbackContext _) =>
        glyphManager.AddGlyph(GlyphType.X);
}
