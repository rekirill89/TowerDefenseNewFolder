using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class StrongholdVisual : MonoBehaviour
    {
        [SerializeField] private Stronghold _stronghold;
        [SerializeField] private Image _image;

        private void Start()
        {
            _stronghold.HealthComponent.OnHealthChanged += OnHealthChangedHandler;
            OnHealthChangedHandler();
        }

        private void OnDestroy()
        {
            _stronghold.HealthComponent.OnHealthChanged -= OnHealthChangedHandler;
        }
        
        private void OnHealthChangedHandler()
        {
            float newValue = (float)_stronghold.HealthComponent.CurrentHealth / _stronghold.HealthComponent.MaxHealth;
            _image.fillAmount = Math.Clamp(newValue, 0, 1);
        }
    }
}
