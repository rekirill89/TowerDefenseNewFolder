using System;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services
{
    public interface IWavesControlService : IDisposable
    {
        event Action<int, int> OnNewWaveStarted;
        event Action<int> OnTimerIncremented;

        UniTask RunWavesLoopAsync();
        void StopWaves();
        void Reset();
    }
}
