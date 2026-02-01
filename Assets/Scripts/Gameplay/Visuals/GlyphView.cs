// using UnityEngine;
// using System.Collections;

// public class GlyphView : MonoBehaviour
// {
//     [SerializeField] private MeshFilter meshFilter;
//     [SerializeField] private MeshRenderer meshRenderer;
//     [SerializeField] private float fadeDuration = 0.2f;

//     private MaterialPropertyBlock block;
//     private Color baseColor;
//     private Coroutine fadeRoutine;

//     private void Awake()
//     {
//         block = new MaterialPropertyBlock();
//     }

//     public void Initialize(Mesh mesh, Material material)
//     {
//         meshFilter.mesh = mesh;
//         meshRenderer.material = material;

//         // Cache base color AFTER material is assigned
//         baseColor = material.color;

//         // Start invisible
//         SetAlpha(0f);

//         // Restart fade if reused from pool
//         if (fadeRoutine != null)
//             StopCoroutine(fadeRoutine);

//         fadeRoutine = StartCoroutine(FadeIn());
//     }

//     private IEnumerator FadeIn()
//     {
//         float t = 0f;

//         while (t < fadeDuration)
//         {
//             t += Time.deltaTime;
//             float alpha = Mathf.SmoothStep(0f, 1f, t / fadeDuration);
//             SetAlpha(alpha);
//             yield return null;
//         }

//         SetAlpha(1f);
//     }

//     private void SetAlpha(float alpha)
//     {
//         Color c = baseColor;
//         c.a = alpha;

//         block.SetColor("_BaseColor", c);
//         meshRenderer.SetPropertyBlock(block);
//     }
// }

using UnityEngine;
using System.Collections;

public class GlyphView : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float fadeDuration = 0.2f;

    private MaterialPropertyBlock block;
    private Color baseColor;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        block = new MaterialPropertyBlock();
    }

    public void Initialize(Mesh mesh, Material material)
    {
        meshFilter.mesh = mesh;
        meshRenderer.material = material;

        // Cache base color AFTER material is assigned
        baseColor = material.color;

        // Start invisible
        SetAlpha(0f);

        // Restart fade if reused from pool
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.SmoothStep(0f, 1f, t / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(1f);
    }

    private void SetAlpha(float alpha)
    {
        Color c = baseColor;
        c.a = alpha;

        block.SetColor("_BaseColor", c);
        meshRenderer.SetPropertyBlock(block);
    }
}
