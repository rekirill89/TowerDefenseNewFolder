using System;
using _Project.Scripts;
using UnityEngine;

public delegate void EnemiesCountChanged(int count);
public delegate void EnemySpawned(Enemy enemy);

public interface IEnemySpawnService : IDisposable
{
    event EnemiesCountChanged OnEnemiesCountChanged;
    event Action OnEnemyKilled;
    event EnemySpawned OnEnemySpawned;

    void SetEnemiesTarget(Vector2 target);
    void SpawnEnemy(Vector2 position = default);
    void StopAllEnemies();
    void RemoveAllEnemies();
}