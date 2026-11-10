using System;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "StrongholdUpgradeConfig", menuName = "Configs/StrongholdUpgradeConfig")]
    public class StrongholdUpgradeConfig : BaseConfig
    {
        [field:Header("Upgrades values")]
        [field:SerializeField] public UpgradeUnit DamageUpgradeUnit { get; private set; }
        [field:SerializeField] public UpgradeUnit AttackRateUpgradeUnit { get; private set; }
        [field:SerializeField] public UpgradeUnit HealthUpgradeUnit { get; private set; }
    }

    [Serializable]
    public class UpgradeUnit
    {
        [field:SerializeField] public float ValuePerLevel { get; private set; }
        [field:SerializeField] public float ValueGrowthPercent { get; private set; }
        [field:SerializeField] public int CostPerLevel { get; private set; }
        [field:SerializeField] public float CostGrowthPercent { get; private set; }
        
        public void NextLevel()
        {
            ValuePerLevel += ValuePerLevel * (ValueGrowthPercent / 100);
            CostPerLevel += (int)(CostPerLevel * (CostGrowthPercent / 100));
        }
    }
}
