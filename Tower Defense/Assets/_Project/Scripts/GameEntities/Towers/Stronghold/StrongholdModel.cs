using System;
using _Project.Scripts.Configs;

namespace _Project.Scripts.GameEntities
{
    public class StrongholdModel : TowerModel, IDamageable
    {
        public event Action<int, int> OnHealthChanged;
        public event Action OnDestroyed;

        public int CurrentHealth => _health.CurrentHealth;
        public int MaxHealth => _health.MaxHealth;
        private bool IsDestroyed { get; set; }

        private readonly HealthComponent _health;
        private readonly float _damageCooldownDuration;
        private float _damageCooldown;

        public StrongholdModel(StrongholdConfig config, IProjectileLauncher projectileLauncher) : base(config, projectileLauncher)
        {
            _damageCooldownDuration = config.DamageCooldown;

            _health = new HealthComponent();
            _health.Init(config.MaxHealth);
            _health.OnHealthChanged += OnHealthChangedHandler;
            _health.OnHealthZero += OnHealthZeroHandler;
        }

        public override void Tick(float deltaTime)
        {
            if (_damageCooldown > 0f)
                _damageCooldown -= deltaTime;

            base.Tick(deltaTime);
        }

        public void TakeDamage(int value)
        {
            if (IsDestroyed || _damageCooldown > 0f)
                return;

            _health.ChangeHealth(-value);
            _damageCooldown = _damageCooldownDuration;
        }

        public void UpgradeDamage(int extraDamage) => ShootComponent.UpgradeDamage(extraDamage);

        public void UpgradeAttackRate(int extraAttackRatePercentage) =>
            ShootComponent.UpgradeAttackRate(extraAttackRatePercentage);

        public void IncreaseMaxHealth(int extraHealth) => _health.IncreaseMaxHealth(extraHealth);

        public override void Reset()
        {
            base.Reset();
            _health.ResetHealth();
            IsDestroyed = false;
            _damageCooldown = 0f;
        }

        private void OnHealthChangedHandler() => OnHealthChanged?.Invoke(_health.CurrentHealth, _health.MaxHealth);

        private void OnHealthZeroHandler()
        {
            if (IsDestroyed)
                return;

            IsDestroyed = true;
            Stop();
            OnDestroyed?.Invoke();
        }
    }
}
