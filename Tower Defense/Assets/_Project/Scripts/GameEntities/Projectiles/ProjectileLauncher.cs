using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Services;

namespace _Project.Scripts.GameEntities
{
    public class ProjectileLauncher : IProjectileLauncher
    {
        private readonly ObjectPool<Projectile> _pool;
        private readonly Transform _origin;
        private readonly List<Projectile> _projectiles = new();

        public ProjectileLauncher(ObjectPool<Projectile> pool, Transform origin)
        {
            _pool = pool;
            _origin = origin;
        }

        public void Launch(ITarget target, int damage)
        {
            var projectile = _pool.Get(_origin.position);
            projectile.Model.Launch(target, damage);
            projectile.OnDespawned += OnProjectileDespawnedHandler;
            _projectiles.Add(projectile);
        }

        public void DespawnAllProjectiles()
        {
            foreach (var projectile in _projectiles.ToArray())
            {
                if (projectile != null)
                    projectile.Despawn();
            }
            _projectiles.Clear();
        }

        private void OnProjectileDespawnedHandler(Projectile projectile)
        {
            projectile.OnDespawned -= OnProjectileDespawnedHandler;
            _projectiles.Remove(projectile);
        }
    }
}
