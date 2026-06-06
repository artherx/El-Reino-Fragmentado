using System;
using UnityEngine;

namespace Shared
{
    public class HealthComponent : MonoBehaviour
    {
        public int CurrentHealth { get; private set; }
        public event Action OnDeath;
        public event Action<int> OnHealthChanged;

        public void Initialize(int maxHealth)
        {
            CurrentHealth = maxHealth;
            OnHealthChanged?.Invoke(CurrentHealth);
        }

        public void ApplyDamage(int amount)
        {
            if (CurrentHealth <= 0) return;

            CurrentHealth -= amount;
            OnHealthChanged?.Invoke(CurrentHealth);

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            OnDeath?.Invoke();
        }
    }
}