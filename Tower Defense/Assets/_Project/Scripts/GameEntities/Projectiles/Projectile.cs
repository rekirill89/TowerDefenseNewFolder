using System;
using UnityEngine;
using _Project.Scripts.Configs;

namespace _Project.Scripts.GameEntities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Projectile : GameEntity<ProjectileConfig>, IPoolable<Projectile>
    {
        public event Action<Projectile> OnDespawned;
        public ProjectileModel Model { get; private set; }

        private Action<Projectile> _returnDelegate;

        public void SetReturnDelegate(Action<Projectile> returnDelegate) => _returnDelegate = returnDelegate;

        public void OnSpawned() { }

        protected override void OnInit(ProjectileConfig config)
        {
            Model = new ProjectileModel(config, new Rigidbody2DBody(GetComponent<Rigidbody2D>()));
            Model.OnHit += Despawn;
            Model.OnTargetLost += Despawn;
        }

        private void FixedUpdate() => Model.FixedTick(Time.fixedDeltaTime);

        private void OnDisable()
        {
            transform.position = default;
        }

        private void OnDestroy()
        {
            if (Model == null)
                return;

            Model.Cancel();
            Model.OnHit -= Despawn;
            Model.OnTargetLost -= Despawn;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Enemy enemy))
                Model.Hit(enemy.Model);
        }

        public void Despawn()
        {
            if (!gameObject.activeSelf)
                return;

            Model.Cancel();
            OnDespawned?.Invoke(this);
            _returnDelegate?.Invoke(this);
        }
    }
}
