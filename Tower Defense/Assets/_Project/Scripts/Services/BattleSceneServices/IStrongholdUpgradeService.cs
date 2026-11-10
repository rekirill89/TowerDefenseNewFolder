using System;
using _Project.Scripts.GameEntities;

namespace _Project.Scripts.Services
{
    public interface IStrongholdUpgradeService
    {
        event Action OnUpgraded;

        void Upgrade(StrongholdUpgradeType upgradeType);
        void SetStronghold(StrongholdModel stronghold);
        void ResetUpgradeStats();
    }
}