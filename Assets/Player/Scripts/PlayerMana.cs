using UnityEngine;
using System;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] private float maxMana = 100f;
    private float currentMana;

    public float Current => currentMana;
    public float Max => maxMana;

    public event Action<float, float> OnManaChanged;

    private void Awake()
    {
        currentMana = maxMana;
        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    public bool HasEnough(float amount)
    {
        return currentMana >= amount;
    }

    public bool Consume(float amount)
    {
        if (currentMana < amount)
            return false;

        currentMana -= amount;
        OnManaChanged?.Invoke(currentMana, maxMana);
        return true;
    }

    public void Restore(float amount)
    {
        currentMana = Mathf.Min(maxMana, currentMana + amount);
        OnManaChanged?.Invoke(currentMana, maxMana);
    }
}
