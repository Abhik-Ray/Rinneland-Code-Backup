using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour
{
    [Header("Bars")]
    [SerializeField] private Image healthFill;
    [SerializeField] private Image manaFill;

    private PlayerHealth playerHealth;
    private PlayerMana playerMana;

    private void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        playerMana = FindFirstObjectByType<PlayerMana>();

        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealth;
            UpdateHealth(playerHealth.Current, playerHealth.Max);
        }

        if (playerMana != null)
        {
            playerMana.OnManaChanged += UpdateMana;
            UpdateMana(playerMana.Current, playerMana.Max);
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateHealth;

        if (playerMana != null)
            playerMana.OnManaChanged -= UpdateMana;
    }

    private void UpdateHealth(float current, float max)
    {
        healthFill.fillAmount = current / max;
    }

    private void UpdateMana(float current, float max)
    {
        manaFill.fillAmount = current / max;
    }
}
