using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts
{
    public class ShootComponent
    {
        private const float NO_ENEMIES_CHECK_TIMER = 0.1f;
        private readonly TowerConfig _towerConfig;
        private readonly Transform _rootTransform;
        private readonly ObjectPool<Projectile> _projectilePool;
        private readonly List<Projectile> _projectiles = new List<Projectile>(); 

        private Enemy _target; 
        private int _damage;
        private float _attackRateTimer;
        
        public ShootComponent(
            ObjectPool<Projectile> projectilePool, 
            Transform rootTransform,
            TowerConfig towerConfig)
        {
            _projectilePool = projectilePool;
            _rootTransform = rootTransform;
            _towerConfig = towerConfig;
            _attackRateTimer = _towerConfig.AttackTimer;
            _damage = _towerConfig.AttackDamage;
        }
        
        public async UniTask AttackAsync(CancellationTokenSource cts)
        {
            while (!cts.IsCancellationRequested)
            {
                if (_target != null)
                {
                    Shoot(_rootTransform.position);
                    await UniTask.WaitForSeconds(_attackRateTimer, cancellationToken: cts.Token);
                }
                else
                    await UniTask.WaitForSeconds(NO_ENEMIES_CHECK_TIMER, cancellationToken: cts.Token);
            }
        }

        public void SetTarget(Enemy target)
        {
            _target = target;
        }

        public void UpgradeDamage(int extraDamage)
        {
            _damage += extraDamage;
        }
        
        public void UpgradeAttackRate(int extraAttackRatePercentage)
        {
            _attackRateTimer -= (_attackRateTimer * (extraAttackRatePercentage / 100f));
        }

        public void ResetStats()
        {
            _damage = _towerConfig.AttackDamage;
            _attackRateTimer = _towerConfig.AttackTimer;
        }

        public void RemoveAllProjectiles()
        {
            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                _projectiles[i].OnDestroyed -= OnProjectileDestroyedHandler;
                _projectiles.RemoveAt(i);
            }
        }

        private void Shoot(Vector2 spawnPosition)
        {
            var projectile = _projectilePool.Get(spawnPosition);
            projectile.Launch(_target.transform, _damage);
            projectile.OnDestroyed += OnProjectileDestroyedHandler;
            _projectiles.Add(projectile);
        }

        private void OnProjectileDestroyedHandler(Projectile projectile)
        {
            projectile.OnDestroyed -= OnProjectileDestroyedHandler;
            _projectiles.Remove(projectile);
        }
    }
}
