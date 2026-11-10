using System;
using UnityEngine;
using _Project.Scripts.Configs;
using _Project.Scripts.Services;

namespace _Project.Scripts.GameEntities
{
    [RequireComponent(typeof(Collider2D))]
    public class Stronghold : BaseTower
    {
        [SerializeField] private StrongholdVisual _visual;

        public new StrongholdModel Model => (StrongholdModel)base.Model;
        public override Type ConfigType => typeof(StrongholdConfig);

        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        protected override void OnInit(TowerConfig config)
        {
            base.OnInit(config);
            _visual.Bind(Model);
        }

        protected override void OnDestroy()
        {
            if (base.Model != null)
                Model.OnDestroyed -= OnModelDestroyedHandler;

            base.OnDestroy();
        }

        public void ResetState()
        {
            Model.Reset();
            Launcher.DespawnAllProjectiles();
            _collider.enabled = true;
        }

        protected override TowerModel CreateModel(TowerConfig config)
        {
            var model = new StrongholdModel((StrongholdConfig)config, Launcher);
            model.OnDestroyed += OnModelDestroyedHandler;
            return model;
        }

        protected override ObjectPool<Projectile> GetProjectilePool(IPoolProvider poolProvider) =>
            poolProvider.Get<Projectile, Arrow>();

        private void OnModelDestroyedHandler()
        {
            _collider.enabled = false;
        }
    }
}
