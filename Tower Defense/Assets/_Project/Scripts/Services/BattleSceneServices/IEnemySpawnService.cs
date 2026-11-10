using System;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public interface IEnemySpawnService : IDisposable
    {
        event Action OnEnemyKilled;

        void SetEnemiesDestination(Vector2 destination);
        void SpawnEnemy(Vector2 position = default);
        void StopAllEnemies();
        void RemoveAllEnemies();
    }
}
