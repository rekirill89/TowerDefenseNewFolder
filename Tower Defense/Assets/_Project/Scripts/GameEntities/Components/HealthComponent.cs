using System;
using UnityEngine;

namespace _Project.Scripts.GameEntities
{
    public class HealthComponent
    {
        public event Action OnHealthChanged;
        public event Action OnHealthZero;
    
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }
        private  int _initialMaxHealth;

        public void Init(int maxHealth)
        {
            _initialMaxHealth = maxHealth;
            MaxHealth = _initialMaxHealth;
            CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke();
        }
    
        public void ChangeHealth(int delta)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + delta, 0, MaxHealth);
            OnHealthChanged?.Invoke();
       
            if(CurrentHealth <= 0)
                OnHealthZero?.Invoke();
        }


        public void IncreaseMaxHealth(int value)
        {
            float healthRatio = (float)CurrentHealth / (float)MaxHealth;
            MaxHealth += value;
            CurrentHealth = (int)(MaxHealth * healthRatio);
            OnHealthChanged?.Invoke();
        }

        public void ResetHealth()
        {
            MaxHealth = _initialMaxHealth;
            CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke();
            OnHealthChanged?.Invoke();
        }
    }
}
