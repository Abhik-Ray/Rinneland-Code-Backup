using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialPopupController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private float displayDuration = 3f;

    private CanvasGroup canvasGroup;
    private Coroutine currentRoutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
    }

    public void Show(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        tutorialText.text = message;
        currentRoutine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(displayDuration);
        canvasGroup.alpha = 0f;
    }
}
