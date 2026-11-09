using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    public class Stronghold : BaseTower, IDamageable
    {
        public event Action OnDestroyed;
        public HealthComponent HealthComponent {  get; private set; }
        public override Type ConfigType => typeof(StrongholdConfig);

        private float _takeDamageDefaultTimer;
        private float _takeDamageTimer;
        private bool _isActing = false;
        
        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            HealthComponent = new HealthComponent();
            HealthComponent.OnHealthZero += OnHealthZeroHandler;
        }

        protected override void Update()
        {
            if (!_isActing)
                return;
            
            CheckTargets();
        }

        private void FixedUpdate()
        {
            _takeDamageTimer = Math.Clamp(_takeDamageTimer - Time.fixedDeltaTime, 0, _takeDamageDefaultTimer);
        }

        protected override void OnDestroy()
        {
            HealthComponent.OnHealthZero -= OnHealthZeroHandler;
            base.OnDestroy();
        }
        
        public void StartActing()
        {
            _isActing = true;
            ShootComponent.AttackAsync(Cts).Forget();
        }

        public void TakeDamage(int value)
        {
            if (_takeDamageTimer <= 0)
            {
                HealthComponent.ChangeHealthByValue(-value);
                _takeDamageTimer = _takeDamageDefaultTimer;
            }
        }

        public void ResetState()
        {
            Cts.Cancel();
            Cts = new CancellationTokenSource();
            
            _collider.enabled = true;
            ResetStats();
        }

        protected override void OnInit(TowerConfig config)
        {
            base.OnInit(config);

            var strongholdConfig = (StrongholdConfig)config;
            _takeDamageDefaultTimer = strongholdConfig.TakeDamageTimer;
            HealthComponent.Init(strongholdConfig.MaxHealth);
        }

        protected override ObjectPool<Projectile> GetProjectilePool(IPoolProvider poolProvider) =>
            poolProvider.Get<Projectile, Arrow>();

        private void ResetStats()
        {
            HealthComponent.ResetHealth();
            ShootComponent.ResetStats();
            ShootComponent.RemoveAllProjectiles();
        }
        
        private void OnHealthZeroHandler()
        {
            _isActing = false;
            _collider.enabled = false;
            Cts.Cancel();
            OnDestroyed?.Invoke();
        }
    }
}
