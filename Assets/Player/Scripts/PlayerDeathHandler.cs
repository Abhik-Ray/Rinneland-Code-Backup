using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private Image deathOverlay;
    [SerializeField] private float fadeDuration = 1.2f;
    [SerializeField] private float restartDelay = 0.5f;
    [SerializeField] private TMPro.TextMeshProUGUI youDiedText;
    [SerializeField] private AudioSource gameOverAudio;



    private bool isDead;

    public void HandleDeath()
    {
        if (isDead)
            return;

        // Stop ambient music
        var music = FindFirstObjectByType<MusicController>();
        if (music != null)
            music.StopMusic();

        // Play game over sound
        if (gameOverAudio != null)
            gameOverAudio.Play();

        isDead = true;
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        DisablePlayerInput();

        // IMPORTANT: ensure cursor is free
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float t = 0f;

        if (youDiedText != null)
        {
            youDiedText.transform.localScale = Vector3.one * 1.2f;
            youDiedText.color = new Color(
                youDiedText.color.r,
                youDiedText.color.g,
                youDiedText.color.b,
                0f
            );
        }

        // Fade using UN-SCALED time
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);

            deathOverlay.color = new Color(0, 0, 0, alpha);

            if (youDiedText != null)
            {
                youDiedText.color = new Color(
                    youDiedText.color.r,
                    youDiedText.color.g,
                    youDiedText.color.b,
                    alpha
                );

                youDiedText.transform.localScale =
                    Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, alpha);
            }

            yield return null;
        }

        // Hold screen (REAL TIME)
        yield return new WaitForSecondsRealtime(restartDelay);

        // Restore time before leaving scene (important!)
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }


    private void DisablePlayerInput()
    {
        // Disable ALL player control scripts here
        var controllers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var c in controllers)
        {
            if (c.gameObject.CompareTag("Player"))
                c.enabled = false;
        }
    }
}
