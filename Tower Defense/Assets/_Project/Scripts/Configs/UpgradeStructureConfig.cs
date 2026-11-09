using System;
using _Project.Scripts;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeStructureConfig", menuName = "Configs/UpgradeStructureConfig")]
public class UpgradeStructureConfig : BaseConfig
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
    [field:SerializeField] public float MultiplierP { get; private set; }
    [field:SerializeField] public int CostPerLevel { get; private set; }
    [field:SerializeField] public float CostMultiplierP { get; private set; }
    
    public void NextLevel()
    {
        ValuePerLevel += ValuePerLevel * (MultiplierP / 100);
        CostPerLevel += (int)(CostPerLevel * (CostMultiplierP / 100));
    }
}
