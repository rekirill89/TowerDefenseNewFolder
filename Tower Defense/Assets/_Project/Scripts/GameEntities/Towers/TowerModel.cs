using _Project.Scripts.Configs;

namespace _Project.Scripts.GameEntities
{
    public class TowerModel
    {
        protected ShootComponent ShootComponent { get; }
        private bool _isActive;
        private ITarget _target;

        public TowerModel(TowerConfig config, IProjectileLauncher projectileLauncher)
        {
            ShootComponent = new ShootComponent(config, projectileLauncher);
        }

        public void Activate() => _isActive = true;

        public void Stop() => _isActive = false;

        public void SetTarget(ITarget target) => _target = target;

        public virtual void Tick(float deltaTime)
        {
            if (!_isActive)
                return;

            ShootComponent.Tick(_target, deltaTime);
        }

        public virtual void Reset()
        {
            _isActive = false;
            _target = null;
            ShootComponent.ResetStats();
        }
    }
}
