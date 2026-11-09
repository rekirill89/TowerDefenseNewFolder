using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class ProjectileModel
    {
        public event Action OnRemove;
        private readonly MoveComponent _moveComponent;
        private EnemyModel _enemyModel;

        private float _currentDeltaTime;
        private int _damage;
        private bool _isReachedFlag = false;

        public ProjectileModel(ProjectileConfig config, IBody2D body2D)
        {
            _moveComponent = new MoveComponent(body2D);
            _moveComponent.Init(config.Speed);
        }

        public void FixedTick(float deltaTime)
        {
            _moveComponent.MoveAndRotateTo(_enemyModel.Position, deltaTime);
        }

        public void Launch(EnemyModel enemyModel, int damage)
        {
            _isReachedFlag = false;
            _enemyModel = enemyModel;
            _enemyModel.OnDespawned += OnEnemyDespawnedHandler;
            _damage = damage;
        }

        public void ReachTarget(IDamageable enemyModel)
        {
            if (_isReachedFlag)
                return;
            _isReachedFlag = true;
            enemyModel.TakeDamage(_damage);
            _enemyModel.OnDespawned -= OnEnemyDespawnedHandler;
            OnRemove?.Invoke();
        }

        private void OnEnemyDespawnedHandler(EnemyModel obj)
        {
            _enemyModel.OnDied -= OnEnemyDespawnedHandler;
            if (!_isReachedFlag)
            {
                _isReachedFlag = true;
                OnRemove?.Invoke();
            }
        }
    }
}