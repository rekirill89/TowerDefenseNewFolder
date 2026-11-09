using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class EnemyModel : IDamageable
    {
        public event Action OnAttacked;
        public event Action<EnemyModel> OnDied;
        public event Action<EnemyModel> OnDespawned;

        public HealthComponent Health { get; }
        public EnemyState State { get; private set; } = EnemyState.Idle;
        public Vector2 Target { get; private set; }
        public Vector2 Position => _body.Position;

        private readonly IBody2D _body;
        private readonly MoveComponent _moveComponent;
        private readonly int _damage;
        private readonly float _attackRate;

        private IDamageable _attackTarget;
        private float _attackCooldown;

        public EnemyModel(EnemyConfig config, IBody2D body)
        {
            _body = body;
            _damage = config.Damage;
            _attackRate = config.AttackRate;

            _moveComponent = new MoveComponent(body);
            _moveComponent.Init(config.Speed);

            Health = new HealthComponent();
            Health.Init(config.MaxHealth);
            Health.OnHealthZero += Die;
        }

        public void Spawn()
        {
            Health.ResetHealth();
            _attackTarget = null;
            State = EnemyState.Moving;
        }

        public void Despawn()
        {
            OnDespawned?.Invoke(this);
        }

        public void SetTarget(Vector2 target)
        {
            Target = target;
        }

        public void Stop()
        {
            if (State == EnemyState.Dead)
                return;

            _attackTarget = null;
            State = EnemyState.Idle;
        }

        public void Tick(float deltaTime)
        {
            if (State != EnemyState.Attacking)
                return;

            _attackCooldown -= deltaTime;
            if (_attackCooldown > 0f)
                return;

            _attackCooldown = _attackRate;
            _attackTarget.TakeDamage(_damage);
            OnAttacked?.Invoke();
        }

        public void FixedTick(float deltaTime)
        {
            if (State != EnemyState.Moving)
                return;

            _moveComponent.MoveTo(Target, deltaTime);
        }

        public void ReachTarget(IDamageable target)
        {
            if (State != EnemyState.Moving)
                return;

            _attackTarget = target;
            _attackCooldown = 0f;
            State = EnemyState.Attacking;
        }

        public void LoseTarget()
        {
            if (State != EnemyState.Attacking)
                return;

            _attackTarget = null;
            State = EnemyState.Idle;
        }

        public void TakeDamage(int value)
        {
            if (State == EnemyState.Dead)
                return;

            Health.ChangeHealthByValue(-value);
        }

        private void Die()
        {
            if (State == EnemyState.Dead)
                return;

            _attackTarget = null;
            State = EnemyState.Dead;
            OnDied?.Invoke(this);
        }
    }
}
