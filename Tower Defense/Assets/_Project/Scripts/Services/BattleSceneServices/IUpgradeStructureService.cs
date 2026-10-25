using System;

namespace _Project.Scripts
{
    public interface IUpgradeStructureService
    {
        event Action OnStructUpgraded;

        void UpgradeStruct(UpgradeStructureEnum upgradeEnum);
        void ResetUpgradeStats();
    }
}