using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using _Project.Scripts.Configs;
using _Project.Scripts.GameEntities;
namespace _Project.Scripts.Services
{
    public class StrongholdUpgradeService : IStrongholdUpgradeService
    {
        public event Action OnUpgraded;

        private readonly StrongholdUpgradeConfig _config;
        private readonly Dictionary<StrongholdUpgradeType, Action> _upgradeActions;
        private readonly ICoinsWallet _coinsWallet;
        private StrongholdModel _stronghold;

        private StrongholdUpgradeConfig _runtimeConfig;

        public StrongholdUpgradeService(ConfigProvider configProvider, ICoinsWallet coinsWallet)
        {
            _coinsWallet = coinsWallet;
            _config = configProvider.GetConfig<StrongholdUpgradeConfig>();
            _runtimeConfig = Object.Instantiate(_config);
            _upgradeActions = new Dictionary<StrongholdUpgradeType, Action>()
            {
                { StrongholdUpgradeType.Damage, () => TryUpgradeDamage() },
                { StrongholdUpgradeType.AttackRate, () => TryUpgradeAttackRate() },
                { StrongholdUpgradeType.Health, () => TryUpgradeHealth() }
            };
        }

        public void SetStronghold(StrongholdModel stronghold)
        {
            _stronghold = stronghold;
        }

        public void Upgrade(StrongholdUpgradeType upgradeType)
        {
            _upgradeActions[upgradeType]();
            OnUpgraded?.Invoke();
        }

        public void ResetUpgradeStats()
        {
            Object.Destroy(_runtimeConfig);
            _runtimeConfig = Object.Instantiate(_config);
        }

        private void TryUpgradeDamage()
        {
            if(!_coinsWallet.TrySpend(_runtimeConfig.DamageUpgradeUnit.CostPerLevel))
                return;
            _stronghold.UpgradeDamage(Mathf.RoundToInt(_runtimeConfig.DamageUpgradeUnit.ValuePerLevel));
            _runtimeConfig.DamageUpgradeUnit.NextLevel();
        }

        private void TryUpgradeAttackRate()
        {
            if(!_coinsWallet.TrySpend(_runtimeConfig.AttackRateUpgradeUnit.CostPerLevel))
                return;
            _stronghold.UpgradeAttackRate(Mathf.RoundToInt(_runtimeConfig.AttackRateUpgradeUnit.ValuePerLevel));
            _runtimeConfig.AttackRateUpgradeUnit.NextLevel();
        }

        private void TryUpgradeHealth()
        {
           if(!_coinsWallet.TrySpend(_runtimeConfig.HealthUpgradeUnit.CostPerLevel))
               return;
           _stronghold.IncreaseMaxHealth(Mathf.RoundToInt(_runtimeConfig.HealthUpgradeUnit.ValuePerLevel));
           _runtimeConfig.HealthUpgradeUnit.NextLevel();
        }
    }
}
