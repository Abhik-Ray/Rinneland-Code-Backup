using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 30f;
    private EnemyAudio enemyAudio;


    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(HealthPercent);
        enemyAudio = GetComponent<EnemyAudio>();
    }

    public void TakeDamage(float amount)
    {
        Debug.Log($"{gameObject.name} took {amount} damage.");       
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if(enemyAudio)
            enemyAudio.PlayHurt();

        OnHealthChanged?.Invoke(HealthPercent);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        if (enemyAudio)
            enemyAudio.PlayDeath();
    }

    public float HealthPercent => currentHealth / maxHealth;
}
