using UnityEngine;

public class PickupFloat : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] private float floatAmplitude = 0.25f;
    [SerializeField] private float floatSpeed = 2f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 60f;

    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Floating motion
        float yOffset =
            Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        transform.position =
            startPosition + Vector3.up * yOffset;

        // Rotation
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
