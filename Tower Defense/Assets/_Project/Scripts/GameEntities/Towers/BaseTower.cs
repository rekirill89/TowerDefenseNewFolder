using UnityEngine;
using Zenject;
using _Project.Scripts.Configs;
using _Project.Scripts.Services;

namespace _Project.Scripts.GameEntities
{
    public abstract class BaseTower : GameEntity<TowerConfig>
    {
        [SerializeField] private EnemyDetector _enemyDetector;
        public TowerModel Model { get; private set; }
        protected ProjectileLauncher Launcher { get; private set; }
        private IPoolProvider _poolProvider;

        [Inject]
        private void Construct(IPoolProvider poolProvider) => _poolProvider = poolProvider;

        protected override void OnInit(TowerConfig config)
        {
            _enemyDetector.Init(config.AttackRange);

            Launcher = new ProjectileLauncher(GetProjectilePool(_poolProvider), transform);
            Model = CreateModel(config);
        }

        protected virtual void Update()
        {
            Model.SetTarget(_enemyDetector.GetClosestEnemy()?.Model);
            Model.Tick(Time.deltaTime);
        }

        protected virtual void OnDestroy()
        {
            Launcher?.DespawnAllProjectiles();
        }

        protected virtual TowerModel CreateModel(TowerConfig config)
        {
            return new TowerModel(config, Launcher);
        }

        protected abstract ObjectPool<Projectile> GetProjectilePool(IPoolProvider poolProvider);
    }
}
