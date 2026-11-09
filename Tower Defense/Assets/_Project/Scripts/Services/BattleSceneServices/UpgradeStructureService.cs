using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;
namespace _Project.Scripts
{
    public class UpgradeStructureService : IUpgradeStructureService
    {
        public event Action OnStructUpgraded;

        private readonly UpgradeStructureConfig _strongholdUpgradeConfig;
        private readonly Dictionary<UpgradeStructureEnum, Action> _dic;
        private readonly ICoinsWallet _coinsWallet;
        private Stronghold _stronghold;

        private UpgradeStructureConfig _strongholdUpgradeConfigCopy;

        public UpgradeStructureService(ConfigProvider configProvider, ICoinsWallet coinsWallet)
        {
            _coinsWallet = coinsWallet;
            _strongholdUpgradeConfig = configProvider.GetConfig<UpgradeStructureConfig>();
            _strongholdUpgradeConfigCopy = Object.Instantiate(_strongholdUpgradeConfig);
            _dic = new Dictionary<UpgradeStructureEnum, Action>()
            {
                { UpgradeStructureEnum.Damage, () => TryUpgradeDamage() },
                { UpgradeStructureEnum.AttackRate, () => TryUpgradeAttackRate() },
                { UpgradeStructureEnum.Health, () => TryUpgradeHealth() }
            };
        }

        public void SetStronghold(Stronghold stronghold)
        {
            _stronghold = stronghold;
        }

        public void UpgradeStruct(UpgradeStructureEnum upgradeEnum)
        {
            _dic[upgradeEnum]();
            OnStructUpgraded?.Invoke();
        }

        public void ResetUpgradeStats()
        {
            Object.Destroy(_strongholdUpgradeConfigCopy);
            _strongholdUpgradeConfigCopy = Object.Instantiate(_strongholdUpgradeConfig);
        }

        private void TryUpgradeDamage()
        {
            if(!_coinsWallet.TrySpend(_strongholdUpgradeConfigCopy.DamageUpgradeUnit.CostPerLevel))
                return;
            _stronghold.ShootComponent.UpgradeDamage((int)_strongholdUpgradeConfigCopy.DamageUpgradeUnit.ValuePerLevel);
            _strongholdUpgradeConfigCopy.DamageUpgradeUnit.NextLevel();
        }

        private void TryUpgradeAttackRate()
        {
            if(!_coinsWallet.TrySpend(_strongholdUpgradeConfigCopy.AttackRateUpgradeUnit.CostPerLevel))
                return;
            _stronghold.ShootComponent.UpgradeAttackRate((int)_strongholdUpgradeConfigCopy.AttackRateUpgradeUnit.ValuePerLevel);
            _strongholdUpgradeConfigCopy.AttackRateUpgradeUnit.NextLevel();
        }

        private void TryUpgradeHealth()
        {
           if(!_coinsWallet.TrySpend(_strongholdUpgradeConfigCopy.HealthUpgradeUnit.CostPerLevel))
               return;
           _stronghold.HealthComponent.IncreaseHealth((int)_strongholdUpgradeConfigCopy.HealthUpgradeUnit.ValuePerLevel);
           _strongholdUpgradeConfigCopy.HealthUpgradeUnit.NextLevel();
        }
    }

    public enum UpgradeStructureEnum
    {
        Damage,
        AttackRate,
        Health
    }
}
