using System;
using _Project.Scripts.Configs;

namespace _Project.Scripts.GameEntities
{
    public class ShootComponent
    {

        public int Damage { get; private set; }
        public float AttackInterval { get; private set; }

        private readonly TowerConfig _towerConfig;
        private readonly IProjectileLauncher _projectileLauncher;
        private float _cooldown;

        public ShootComponent(TowerConfig towerConfig, IProjectileLauncher projectileLauncher)
        {
            _towerConfig = towerConfig;
            _projectileLauncher = projectileLauncher;
            ResetStats();
        }

        public void Tick(ITarget target, float deltaTime)
        {
            if (_cooldown > 0f)
                _cooldown -= deltaTime;

            if (target == null || _cooldown > 0f)
                return;

            _cooldown = AttackInterval;
            _projectileLauncher.Launch(target, Damage);
        }

        public void UpgradeDamage(int extraDamage)
        {
            Damage += extraDamage;
        }

        public void UpgradeAttackRate(int extraAttackRatePercentage)
        {
            AttackInterval -= AttackInterval * (extraAttackRatePercentage / 100f);
        }

        public void ResetStats()
        {
            Damage = _towerConfig.Damage;
            AttackInterval = _towerConfig.AttackInterval;
            _cooldown = 0f;
        }
    }
}
