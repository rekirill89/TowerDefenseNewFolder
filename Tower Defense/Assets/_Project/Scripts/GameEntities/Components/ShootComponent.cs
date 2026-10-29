using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts
{
    public class ShootComponent
    {
        private const float NO_ENEMIES_CHECK_TIMER = 0.1f;
        private readonly ProjectileSO _projectileSO;
        private readonly AttackStructureSO _attackStructureSO;
        private readonly Transform _rootTransform;
        private readonly ObjectPool<Projectile> _projectilePool;
        private readonly List<Projectile> _projectiles = new List<Projectile>(); 

        private Enemy _target; 
        private int _damage;
        private float _attackRateTimer;
        
        public ShootComponent(
            Projectile projectilePrefab, 
            Transform rootTransform,
            Enemy target, 
            ProjectileSO projectileSO,
            AttackStructureSO attackStructureSO)
        {
            _projectilePool = new ObjectPool<Projectile>(projectilePrefab);

            _rootTransform = rootTransform;
            _projectileSO = projectileSO;
            _attackStructureSO = attackStructureSO;
            _target = target;
            _attackRateTimer = _attackStructureSO.AttackTimer;
            _damage = _attackStructureSO.AttackDamage;
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
            _damage = _attackStructureSO.AttackDamage;
            _attackRateTimer = _attackStructureSO.AttackTimer;
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
            var projectile = _projectilePool.Get();
            projectile.Init(spawnPosition, _projectilePool.Return, _projectileSO, _damage);
            projectile.SetTarget(_target.transform);
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
