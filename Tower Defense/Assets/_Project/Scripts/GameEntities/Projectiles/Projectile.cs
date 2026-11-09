using System;
using UnityEngine;

namespace _Project.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Projectile : GameEntity<ProjectileConfig>, IPoolable<Projectile>
    {
        public event Action<Projectile> OnDestroyed;
        public ProjectileModel Model { get; private set; }

        private Action<Projectile> _returnDelegate;

        public void SetReturnDelegate(Action<Projectile> returnDelegate) => _returnDelegate = returnDelegate;

        public void OnSpawned() { }

        private void FixedUpdate()
        {
            Model.FixedTick(Time.fixedDeltaTime);
        }

        private void OnDisable()
        {
            transform.position = default;
        }

        private void OnDestroy()
        {
            Model.OnRemove -= RemoveThisObject;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                Model.ReachTarget(enemy.Model);
            }
        }

        private void RemoveThisObject()
        {
            OnDestroyed?.Invoke(this);
            _returnDelegate?.Invoke(this);
        }

        protected override void OnInit(ProjectileConfig config)
        {
            Model = new ProjectileModel(config, new Rigidbody2DBody(GetComponent<Rigidbody2D>()));
            Model.OnRemove += RemoveThisObject;
        }
    }
}
