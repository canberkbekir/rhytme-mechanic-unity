using System;
using UnityEngine;

namespace Characters.Base
{
    public abstract class Character : MonoBehaviour
    { 
        public event Action OnDeathEvent;
        public event Action<float> OnHealEvent;
        public event Action<float> OnDamageEvent;
        
        [Header("Character Stats")] 
        public float maxHealth;
        public float currentHealth;

        protected virtual void Start()
        {
            currentHealth = maxHealth; // Initialize health to max
        }

        public virtual void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth < 0)
                currentHealth = 0;

            OnDamageEvent?.Invoke(damage);
            Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");
            CheckIfDead();
        }

        public virtual void Heal(float amount)
        {
            currentHealth += amount;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            OnHealEvent?.Invoke(amount);
            Debug.Log($"{gameObject.name} healed {amount}. Current health: {currentHealth}");
        }

        public bool IsAlive()
        {
            return currentHealth > 0;
        }

        protected virtual void CheckIfDead()
        {
            if (!IsAlive())
            {
                Debug.Log($"{gameObject.name} has died.");
                OnDeath();
            }
        }

        protected virtual void OnDeath()
        {
            OnDeathEvent?.Invoke();
        }
    }
}