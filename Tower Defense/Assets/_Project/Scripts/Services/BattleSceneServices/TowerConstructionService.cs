using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using _Project.Scripts.Configs;
using _Project.Scripts.GameEntities;

namespace _Project.Scripts.Services
{
    public class TowerConstructionService : ITowerConstructionService
    {
        public event Action OnTowerConstructed;

        private readonly IEntityFactory _entityFactory;
        private readonly ConfigProvider _configProvider;
        private readonly ICoinsWallet _coinsWallet;
        private readonly List<BaseTower> _constructedTowers = new();
        private readonly Dictionary<TowerType, (TowerConfig Config, Func<Vector2, BaseTower> Create)> _towers = new();

        public TowerConstructionService(
            IEntityFactory entityFactory,
            ConfigProvider configProvider,
            ICoinsWallet coinsWallet)
        {
            _entityFactory = entityFactory;
            _configProvider = configProvider;
            _coinsWallet = coinsWallet;

            RegisterTower<Archer>(TowerType.Archer);
            RegisterTower<Cannon>(TowerType.Cannon);
        }

        public void TryConstructTower(TowerType towerType, Vector2 position)
        {
            if (!_towers.TryGetValue(towerType, out var tower))
            {
                Debug.LogError($"No tower registered for {towerType}");
                return;
            }

            if(!_coinsWallet.TrySpend(tower.Config.ConstructCost))
                return;

            _constructedTowers.Add(tower.Create(position));
            OnTowerConstructed?.Invoke();
        }

        public void StopAllTowers()
        {
            foreach (var tower in _constructedTowers)
            {
                tower.Model.Stop();
            }
        }

        public void RemoveAllTowers()
        {
            foreach (var tower in _constructedTowers)
            {
                Object.Destroy(tower.gameObject);
            }
            _constructedTowers.Clear();
        }

        private void RegisterTower<TTower>(TowerType towerType) where TTower : BaseTower
        {
            var config = _configProvider.GetEntityConfig<TTower, TowerConfig>();
            _towers.Add(towerType, (config, position => _entityFactory.Create<TTower>(position)));
        }
    }
}
