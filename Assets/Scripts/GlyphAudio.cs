using UnityEngine;

public class GlyphAudio : MonoBehaviour
{
    [SerializeField] private AudioClip scribbleClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayScribble()
    {
        if (scribbleClip == null)
            return;

        audioSource.PlayOneShot(scribbleClip);
    }
}
