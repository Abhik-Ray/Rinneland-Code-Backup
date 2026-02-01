using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] hurtClips;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip roarClip;

    [Header("Settings")]
    [SerializeField] private float roarCooldown = 5f;

    private AudioSource audioSource;
    private float lastRoarTime;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHurt()
    {
        if (hurtClips.Length == 0)
            return;

        int index = Random.Range(0, hurtClips.Length);
        audioSource.PlayOneShot(hurtClips[index]);
    }

    public void PlayDeath()
    {
        if (deathClip == null)
            return;

        audioSource.PlayOneShot(deathClip);
    }

    public void PlayRoar()
    {
        if (roarClip == null)
            return;

        if (Time.time < lastRoarTime + roarCooldown)
            return;

        audioSource.PlayOneShot(roarClip);
        lastRoarTime = Time.time;
    }
}
