using System.Threading;
using UnityEngine;
using Zenject;

namespace _Project.Scripts
{
    public abstract class BaseTower : GameEntity<TowerConfig>
    {
        [SerializeField] private EnemyDetector _enemyDetector;
        public ShootComponent ShootComponent {get; private set;}
        protected CancellationTokenSource Cts = new CancellationTokenSource();
        private IPoolProvider _poolProvider;

        public void StopActing() => Cts.Cancel();

        [Inject]
        private void Construct(IPoolProvider poolProvider) => _poolProvider = poolProvider;

        protected virtual void Update()
        {
            CheckTargets();
        }

        protected virtual void OnDestroy()
        {
            Cts.Cancel();
        }

        protected abstract ObjectPool<Projectile> GetProjectilePool(IPoolProvider poolProvider);

        protected void CheckTargets()
        {
            ShootComponent.SetTarget(_enemyDetector.GetClosestEnemy());
        }

        protected override void OnInit(TowerConfig config)
        {
            ShootComponent = new ShootComponent(GetProjectilePool(_poolProvider), transform, config);
            _enemyDetector.Init(config.AttackRange);
        }
    }
}
