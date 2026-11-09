using System;
using UnityEngine;

namespace _Project.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Projectile : GameEntity<ProjectileConfig>, IPoolable<Projectile>
    {
        public event Action<Projectile> OnDestroyed;

        private MoveComponent _moveComponent;
        private Rigidbody2D _rb;
        private Transform _target;

        private int _damage;

        private Action<Projectile> _returnDelegate;

        public void SetReturnDelegate(Action<Projectile> returnDelegate) => _returnDelegate = returnDelegate;

        public void OnSpawned() { }

        private void Awake()
        {
            _rb =  GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _moveComponent.MoveAndRotateTo(_target.position);
        }

        private void LateUpdate()
        {
            if(!_target.gameObject.activeSelf)
                RemoveThisObject();
        }

        private void OnDisable()
        {
            transform.position = default;
        }

        public void Launch(Transform target, int damage)
        {
            _target = target;
            _damage = damage;
        }

        private void DealDamage(IDamageable target)
        {
            target.TakeDamage(_damage);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                DealDamage(enemy);
                RemoveThisObject();
            }
        }

        private void RemoveThisObject()
        {
            OnDestroyed?.Invoke(this);
            _returnDelegate?.Invoke(this);
        }

        protected override void OnInit(ProjectileConfig config)
        {
            _moveComponent = new MoveComponent(_rb);
            _moveComponent.Init(config.Speed);
        }
    }
}
