using System;
using _Project.Scripts.GameEntities;

namespace _Project.Scripts.Services
{
    public interface IBattleControlService : IDisposable
    {
        event Action OnBattleFinished;
        event Action OnBattleStarted;
        event Action<Stronghold> OnStrongholdCreated;

        void Init();
        void StartBattle();
        void RestartBattle();
    }
}