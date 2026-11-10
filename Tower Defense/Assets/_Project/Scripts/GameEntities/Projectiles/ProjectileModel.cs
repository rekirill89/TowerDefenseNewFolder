using System;
using _Project.Scripts.Configs;

namespace _Project.Scripts.GameEntities
{
    public class ProjectileModel
    {
        public event Action OnHit;
        public event Action OnTargetLost;

        public bool IsFlying => _target != null;

        private readonly MoveComponent _moveComponent;
        private ITarget _target;
        private int _damage;

        public ProjectileModel(ProjectileConfig config, IBody2D body)
        {
            _moveComponent = new MoveComponent(body);
            _moveComponent.Init(config.Speed);
        }

        public void Launch(ITarget target, int damage)
        {
            Cancel();

            _target = target;
            _damage = damage;
            _target.OnDespawned += OnTargetDespawnedHandler;
        }

        public void FixedTick(float deltaTime)
        {
            if (!IsFlying)
                return;

            _moveComponent.MoveAndRotateTo(_target.Position, deltaTime);
        }

        public void Hit(IDamageable victim)
        {
            if (!IsFlying)
                return;

            // Release before dealing damage: a killing hit despawns the target,
            // and the projectile must not also treat that as "target lost".
            ReleaseTarget();
            victim.TakeDamage(_damage);
            OnHit?.Invoke();
        }

        public void Cancel()
        {
            if (IsFlying)
                ReleaseTarget();
        }

        private void OnTargetDespawnedHandler(ITarget target)
        {
            target.OnDespawned -= OnTargetDespawnedHandler;
            if (target != _target)
                return;

            _target = null;
            OnTargetLost?.Invoke();
        }

        private void ReleaseTarget()
        {
            _target.OnDespawned -= OnTargetDespawnedHandler;
            _target = null;
        }
    }
}
