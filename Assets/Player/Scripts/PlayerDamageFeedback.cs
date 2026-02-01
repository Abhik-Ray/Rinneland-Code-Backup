using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerDamageFeedback : MonoBehaviour
{
    [SerializeField] private Image damageOverlay;
    [SerializeField] private float flashDuration = 0.10f;
    [SerializeField] private float maxAlpha = 0.2f;

    private Coroutine flashRoutine;

    public void PlayDamageFlash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash()
    {
        damageOverlay.color = new Color(1, 0, 0, maxAlpha);
        yield return new WaitForSeconds(flashDuration);
        damageOverlay.color = new Color(1, 0, 0, 0);
    }
}
