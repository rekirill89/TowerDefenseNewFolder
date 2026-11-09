using System;
using UnityEngine;

namespace _Project.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : GameEntity<EnemyConfig>, IDamageable, IPoolable<Enemy>
    {
        public event Action<Enemy> OnDied;
        public event Action<Enemy> OnDespawned;
        [SerializeField] private EnemyVisual _visual;
        public EnemyModel Model { get; private set; }

        private Action<Enemy> _returnDelegate;

        public void SetReturnDelegate(Action<Enemy> returnDelegate) => _returnDelegate = returnDelegate;

        private void Update() => Model.Tick(Time.deltaTime);

        private void FixedUpdate() => Model.FixedTick(Time.fixedDeltaTime);

        private void OnDisable()
        {
            transform.position = Vector2.zero;
        }

        private void OnDestroy()
        {
            if (Model != null)
                Model.OnDied -= OnModelDiedHandler;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Stronghold stronghold))
                Model.ReachTarget(stronghold);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Stronghold _))
                Model.LoseTarget();
        }

        protected override void OnInit(EnemyConfig config)
        {
            Model = new EnemyModel(config, new Rigidbody2DBody(GetComponent<Rigidbody2D>()));
            Model.OnDied += OnModelDiedHandler;
            _visual.Bind(Model);
        }

        public void OnSpawned() => Model.Spawn();

        public void TakeDamage(int value) => Model.TakeDamage(value);

        public void Despawn()
        {
            if (!gameObject.activeSelf)
                return;

            Model.Despawn();
            OnDespawned?.Invoke(this);
            _returnDelegate?.Invoke(this);
        }

        private void OnModelDiedHandler(EnemyModel model)
        {
            OnDied?.Invoke(this);
            Despawn();
        }
    }
}
