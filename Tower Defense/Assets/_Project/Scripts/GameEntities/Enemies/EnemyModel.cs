using System;
using UnityEngine;
using _Project.Scripts.Configs;

namespace _Project.Scripts.GameEntities
{
    public class EnemyModel : ITarget
    {
        public event Action OnAttacked;
        public event Action<EnemyModel> OnDied;
        public event Action<ITarget> OnDespawned;

        public HealthComponent Health { get; }
        public EnemyState State { get; private set; } = EnemyState.Idle;
        public Vector2 Destination { get; private set; }
        public Vector2 Position => _body.Position;

        private readonly IBody2D _body;
        private readonly MoveComponent _moveComponent;
        private readonly int _damage;
        private readonly float _attackInterval;

        private IDamageable _attackTarget;
        private float _attackCooldown;

        public EnemyModel(EnemyConfig config, IBody2D body)
        {
            _body = body;
            _damage = config.Damage;
            _attackInterval = config.AttackInterval;

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

        public void SetDestination(Vector2 destination)
        {
            Destination = destination;
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

            _attackCooldown = _attackInterval;
            _attackTarget.TakeDamage(_damage);
            OnAttacked?.Invoke();
        }

        public void FixedTick(float deltaTime)
        {
            if (State != EnemyState.Moving)
                return;

            _moveComponent.MoveTo(Destination, deltaTime);
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

            Health.ChangeHealth(-value);
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
