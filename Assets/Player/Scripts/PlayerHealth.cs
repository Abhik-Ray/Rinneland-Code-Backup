using UnityEngine;
using System;

[RequireComponent(typeof(PlayerAudio))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public float Current => currentHealth;
    public float Max => maxHealth;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;
    private PlayerAudio playerAudio;
    private bool combatTutorialShown;



    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        playerAudio = GetComponent<PlayerAudio>();
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0f)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (!combatTutorialShown)
        {
            var popup = FindFirstObjectByType<TutorialPopupController>();
            if (popup != null)
                popup.Show("Avoid enemy attacks!");

            combatTutorialShown = true;
        }


        var damageFeedback = FindFirstObjectByType<PlayerDamageFeedback>();
        if (damageFeedback != null)
        {
            damageFeedback.PlayDamageFlash();
        }

        if (playerAudio != null && currentHealth > 0f)
        {
            playerAudio.PlayPain();
        }

        if (currentHealth <= 0f)
        {
            OnDeath?.Invoke();

            if (currentHealth <= 0f)
            {
                if (playerAudio)
                    playerAudio.PlayDeath();

                OnDeath?.Invoke();
            }

            var deathHandler = FindFirstObjectByType<PlayerDeathHandler>();
            if (deathHandler != null)
            {
                deathHandler.HandleDeath();
            }
        }

    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
