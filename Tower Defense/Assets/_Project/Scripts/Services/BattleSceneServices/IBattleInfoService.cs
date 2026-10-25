using System;

namespace _Project.Scripts
{
    public interface IBattleInfoService : IDisposable
    {
        void Init();
        BattleInfoData GetInfo();
        void Reset();
    }
}