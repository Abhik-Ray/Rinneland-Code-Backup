using UnityEngine;

public enum PickupType
{
    Health,
    Mana
}

public class Pickup : MonoBehaviour
{
    [SerializeField] private PickupType type;
    [SerializeField] private float amount = 25f;
    [SerializeField] private AudioClip pickupSFX;
    [SerializeField] private float pickupVolume = 0.8f;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (type == PickupType.Health)
        {
            var health = other.GetComponent<PlayerHealth>();
            if (health != null)
                health.Heal(amount);
        }
        else if (type == PickupType.Mana)
        {
            var mana = other.GetComponent<PlayerMana>();
            if (mana != null)
                mana.Restore(amount);
        }

        if (pickupSFX != null)
        {
            AudioSource.PlayClipAtPoint(
                pickupSFX,
                transform.position,
                pickupVolume
            );
        }

        Destroy(gameObject);
    }
}
