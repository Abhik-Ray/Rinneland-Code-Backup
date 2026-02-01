using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class GlyphFadeIn : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.2f;

    private Renderer rend;
    private MaterialPropertyBlock block;
    private Color baseColor;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();

        rend.GetPropertyBlock(block);
        // baseColor = rend.sharedMaterial.color;

    }

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);

            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(1f);
    }

    private void SetAlpha(float a)
    {
        Color c = baseColor;
        c.a = a;

        block.SetColor("_BaseColor", c);
        rend.SetPropertyBlock(block);
    }
}