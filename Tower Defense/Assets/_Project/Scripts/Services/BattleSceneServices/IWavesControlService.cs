using System;
using Cysharp.Threading.Tasks;

public delegate void NewWaveStarted(int waveIndex, int totalEnemiesThisWave);

public interface IWavesControlService : IDisposable
{
    event NewWaveStarted OnNewWaveStarted;
    event Action<int> OnTimerIncremented;

    UniTask RunWavesLoopAsync();
    void StopWave();
    void Reset();
}