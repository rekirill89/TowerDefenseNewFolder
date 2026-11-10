using System;

namespace _Project.Scripts.Services
{
    public interface IBattleInfoService : IDisposable
    {
        void Init();
        BattleInfoData GetInfo();
        void Reset();
    }
}