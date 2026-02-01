using UnityEngine;

public class SpellBookBob : MonoBehaviour
{
    [Header("Bobbing")]
    [SerializeField] private float bobAmplitude = 0.015f;
    [SerializeField] private float bobSpeed = 1.5f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 15f;

    private Vector3 startLocalPos;

    private void Awake()
    {
        startLocalPos = transform.localPosition;
    }

    private void Update()
    {
        float bob =
            Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;

        transform.localPosition =
            startLocalPos + Vector3.up * bob;

        transform.Rotate(
            Vector3.up,
            rotationSpeed * Time.deltaTime,
            Space.Self
        );
    }
}
