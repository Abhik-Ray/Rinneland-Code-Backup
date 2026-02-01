using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [Header("Footstep Clips")]
    [SerializeField] private AudioClip[] footstepClips;

    [Header("Settings")]
    [SerializeField] private float stepInterval = 0.45f;
    [SerializeField] private float moveThreshold = 0.1f;

    private AudioSource audioSource;
    private CharacterController controller;

    private float stepTimer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!IsGrounded())
            return;

        if (controller.velocity.magnitude < moveThreshold)
            return;

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            PlayFootstep();
            stepTimer = stepInterval;
        }
    }

    private bool IsGrounded()
    {
        return controller != null && controller.isGrounded;
    }

    private void PlayFootstep()
    {
        if (footstepClips.Length == 0)
            return;

        int index = Random.Range(0, footstepClips.Length);
        audioSource.PlayOneShot(footstepClips[index]);
    }
}
