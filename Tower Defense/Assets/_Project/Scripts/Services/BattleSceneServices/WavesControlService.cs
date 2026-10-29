using System;
using System.Threading;
using _Project.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class WavesControlService : IWavesControlService
{
    public event NewWaveStarted OnNewWaveStarted; 
    public event Action<int> OnTimerIncremented;
    
    private readonly WavesSO _wavesSO;
    private readonly IEnemySpawnService _enemySpawnService;
    
    private int _totalEnemyAmountPerWave = 0;
    private float _spawnTimestep; 
    private float _waveCurrentTimer;
    private int _waveCurrentIndex;

    private bool _isWaveFinished = false;
    private bool _stopWaveFlag = false;
    
    private CancellationTokenSource _cts = new CancellationTokenSource();

    public WavesControlService(WavesSO wavesSO, IEnemySpawnService enemySpawnService)
    {
        _wavesSO = wavesSO;
        _enemySpawnService = enemySpawnService;
        _spawnTimestep = _wavesSO.SpawnTimestep;
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    public async UniTask RunWavesLoopAsync()
    {
        while (!_cts.IsCancellationRequested)
        {
            StartWaveAsync().Forget();
            await UniTask.WaitUntil(() => _isWaveFinished, cancellationToken: _cts.Token);
            await UniTask.WaitForSeconds(_wavesSO.BreakBetweenWavesTimer, cancellationToken: _cts.Token);
        }    
    }


    public void StopWave()
    {
        _cts.Cancel();
    }

    public void Reset()
    {
        _cts.Cancel();
        _cts = new CancellationTokenSource();
        _spawnTimestep = _wavesSO.SpawnTimestep;
        _waveCurrentIndex = 0;
        _waveCurrentTimer = 0;
    }

    private async UniTask StartWaveAsync()
    {
        _isWaveFinished = false;
        StartWaveTimerAsync().Forget();

        _waveCurrentIndex++;
        //_spawnTimestep -= (_wavesSO.SpawnTimestep * (_waveCurrentIndex * 0.1f));
        _spawnTimestep -= (_spawnTimestep * _wavesSO.DecreaseSpawnTimestepMultiplier);
        _totalEnemyAmountPerWave = (int)(_wavesSO.WaveTimer / _spawnTimestep);
        OnNewWaveStarted?.Invoke(_waveCurrentIndex, _totalEnemyAmountPerWave);
        
        while (!_cts.IsCancellationRequested && !_stopWaveFlag)
        {
            var dirX = Random.Range(_wavesSO.MinDirX, _wavesSO.MaxDirX);
            var dirY = Random.Range(_wavesSO.MinDirY, _wavesSO.MaxDirY);
            var pos = new Vector2(dirX, dirY)
                .normalized * Random.Range(_wavesSO.MinSpawnDistance, _wavesSO.MaxSpawnDistance);
            
            _enemySpawnService.SpawnEnemy(pos);

            await UniTask.WaitForSeconds(_spawnTimestep, cancellationToken: _cts.Token);
        }

        _isWaveFinished = true;
    }

    private async UniTask StartWaveTimerAsync()
    {
        _stopWaveFlag = false;
        _waveCurrentTimer = 0;

        int currentSecond = 0;
        int lastReportedSecond = 0;
        
        while (_waveCurrentTimer < _wavesSO.WaveTimer && !_cts.IsCancellationRequested)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: _cts.Token);
            _waveCurrentTimer += Time.deltaTime;
            currentSecond = (int)_waveCurrentTimer;
            
            if (currentSecond >= lastReportedSecond)
            {
                OnTimerIncremented?.Invoke(currentSecond);
                lastReportedSecond = currentSecond;
            }
        }
        
        _stopWaveFlag = true;
    }
}
