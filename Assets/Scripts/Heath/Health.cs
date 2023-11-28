using Assets.Scripts;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public delegate void HealthChangeHandler(int currentHealth, int maxHealth);
    public delegate void OnHealthZeroHandler();
    public event HealthChangeHandler OnHealthChanged;
    public event OnHealthZeroHandler OnHealthZero;

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int HealthValue { get => currentHealth; set => currentHealth = value; }

    public void TakeDamage(int amount) {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, MaxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0) {
            OnHealthZero?.Invoke();
        }
    }

    public void Initialize(int currentHealth, int maxHealth) {
        this.maxHealth = maxHealth;
        this.currentHealth = currentHealth;
    }
}
