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
        
        private readonly int _maxHealth = 200;
        private readonly float _takeDamageDefaultTimer = 0.3f;
        
        private Collider2D _collider;

        private float _takeDamageTimer = 0f;
        private bool _isActing = false;
        
        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<Collider2D>();
            HealthComponent = new HealthComponent(_maxHealth);
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
            Cts.Cancel();
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

        public void Reset()
        {
            Cts.Cancel();
            Cts = new CancellationTokenSource();
            
            _collider.enabled = true;
            ResetStats();
        }

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


        /*private void CheckTargets()
        {
            if (_enemyDetector.Enemies.Count == 0)
            {
                _closestEnemy =  null;
                ShootComponent.SetTarget(_closestEnemy);
                return;
            }
            
            _closestEnemy = _enemyDetector.GetClosestEnemy();
            ShootComponent.SetTarget(_closestEnemy);
        }*/
    }
}