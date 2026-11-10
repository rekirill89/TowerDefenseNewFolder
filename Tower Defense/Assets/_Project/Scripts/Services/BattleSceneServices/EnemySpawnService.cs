using System;
using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.GameEntities;

namespace _Project.Scripts.Services
{
    public class EnemySpawnService : IEnemySpawnService
    {
        public event Action OnEnemyKilled;

        private readonly List<Enemy> _enemies;
        private readonly ObjectPool<Enemy> _objectPool;
        private Vector2 _enemiesDestination;

        public EnemySpawnService(IPoolProvider poolProvider)
        {
            _objectPool = poolProvider.Get<Enemy>();
            _enemies = new List<Enemy>();
        }

        public void Dispose()
        {
            foreach (var enemy in _enemies)
                Unsubscribe(enemy);
        }

        public void SetEnemiesDestination(Vector2 destination)
        {
            _enemiesDestination = destination;
        }

        public void SpawnEnemy(Vector2 position = default)
        {
            var enemy = _objectPool.Get(position);
            enemy.Model.SetDestination(_enemiesDestination);

            _enemies.Add(enemy);
            enemy.OnDied += OnEnemyDiedHandler;
            enemy.OnDespawned += OnEnemyDespawnedHandler;

        }

        public void StopAllEnemies()
        {
            foreach (var enemy in _enemies)
                enemy.Model.Stop();
        }

        public void RemoveAllEnemies()
        {
            foreach (var enemy in new List<Enemy>(_enemies))
                enemy.Despawn();
        }

        private void OnEnemyDiedHandler(Enemy enemy)
        {
            OnEnemyKilled?.Invoke();
        }

        private void OnEnemyDespawnedHandler(Enemy enemy)
        {
            Unsubscribe(enemy);
            _enemies.Remove(enemy);
        }

        private void Unsubscribe(Enemy enemy)
        {
            enemy.OnDied -= OnEnemyDiedHandler;
            enemy.OnDespawned -= OnEnemyDespawnedHandler;
        }
    }
}
