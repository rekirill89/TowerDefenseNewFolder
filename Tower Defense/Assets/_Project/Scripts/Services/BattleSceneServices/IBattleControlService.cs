using System;

namespace _Project.Scripts
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