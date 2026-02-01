using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] painClips;
    [SerializeField] private AudioClip deathClip;

    [Header("Settings")]
    [SerializeField] private float painCooldown = 0.4f;

    private AudioSource audioSource;
    private float lastPainTime;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPain()
    {
        if (painClips.Length == 0)
            return;

        if (Time.time < lastPainTime + painCooldown)
            return;

        int index = Random.Range(0, painClips.Length);
        audioSource.PlayOneShot(painClips[index]);

        lastPainTime = Time.time;
    }

    public void PlayDeath()
    {
        if (deathClip == null)
            return;

        audioSource.PlayOneShot(deathClip);
    }
}
