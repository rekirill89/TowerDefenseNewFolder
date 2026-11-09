using System;
using System.Collections.Generic;
using _Project.Scripts;
using UnityEngine;

public class EnemySpawnService : IEnemySpawnService
{
    public event EnemiesCountChanged OnEnemiesCountChanged;
    public event Action OnEnemyKilled;
    public event EnemySpawned OnEnemySpawned;

    private readonly List<Enemy> _enemies;
    private readonly ObjectPool<Enemy> _objectPool;
    private Vector2 _enemiesTarget;

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

    public void SetEnemiesTarget(Vector2 target)
    {
        _enemiesTarget = target;
    }

    public void SpawnEnemy(Vector2 position = default)
    {
        var enemy = _objectPool.Get(position);
        enemy.Model.SetTarget(_enemiesTarget);

        _enemies.Add(enemy);
        enemy.OnDied += OnEnemyDiedHandler;
        enemy.OnDespawned += OnEnemyDespawnedHandler;

        OnEnemiesCountChanged?.Invoke(_enemies.Count);
        OnEnemySpawned?.Invoke(enemy);
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
        OnEnemiesCountChanged?.Invoke(_enemies.Count);
    }

    private void Unsubscribe(Enemy enemy)
    {
        enemy.OnDied -= OnEnemyDiedHandler;
        enemy.OnDespawned -= OnEnemyDespawnedHandler;
    }
}
