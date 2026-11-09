using System;
using System.Collections.Generic;
using _Project.Scripts.MVP;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Scripts
{
    public class StructureConstructService : IStructureConstructService
    {
        public event Action OnStructureConstructed;

        private readonly IEntityFactory _entityFactory;
        private readonly ConfigProvider _configProvider;
        private readonly ICoinsWallet _coinsWallet;
        private readonly List<BaseTower> _constructedTowers = new();
        private readonly Dictionary<StructureEnum, (TowerConfig Config, Func<Vector2, BaseTower> Create)> _towers = new();

        public StructureConstructService(
            IEntityFactory entityFactory,
            ConfigProvider configProvider,
            ICoinsWallet coinsWallet)
        {
            _entityFactory = entityFactory;
            _configProvider = configProvider;
            _coinsWallet = coinsWallet;

            RegisterTower<Archer>(StructureEnum.Archer);
            RegisterTower<Cannon>(StructureEnum.Cannon);
        }

        public void TryConstructStructure(StructureEnum structureEnum, Vector2 pos)
        {
            if (!_towers.TryGetValue(structureEnum, out var tower))
            {
                Debug.LogError($"No tower registered for {structureEnum}");
                return;
            }

            if(!_coinsWallet.TrySpend(tower.Config.ConstructCost))
                return;

            _constructedTowers.Add(tower.Create(pos));
            OnStructureConstructed?.Invoke();
        }

        public void StopAllTowers()
        {
            foreach (var tower in _constructedTowers)
            {
                tower.StopActing();
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

        private void RegisterTower<TTower>(StructureEnum structureEnum) where TTower : BaseTower
        {
            var config = _configProvider.GetEntityConfig<TTower, TowerConfig>();
            _towers.Add(structureEnum, (config, pos => _entityFactory.Create<TTower>(pos)));
        }
    }
}
