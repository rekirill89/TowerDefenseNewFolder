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
    
    private readonly WavesConfig _wavesConfig;
    private readonly IEnemySpawnService _enemySpawnService;
    
    private int _totalEnemyAmountPerWave = 0;
    private float _spawnTimestep; 
    private float _waveCurrentTimer;
    private int _waveCurrentIndex;

    private bool _isWaveFinished = false;
    private bool _stopWaveFlag = false;
    
    private CancellationTokenSource _cts = new CancellationTokenSource();

    public WavesControlService(ConfigProvider configProvider, IEnemySpawnService enemySpawnService)
    {
        _wavesConfig = configProvider.GetConfig<WavesConfig>();
        _enemySpawnService = enemySpawnService;
        _spawnTimestep = _wavesConfig.SpawnTimestep;
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
            await UniTask.WaitForSeconds(_wavesConfig.BreakBetweenWavesTimer, cancellationToken: _cts.Token);
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
        _spawnTimestep = _wavesConfig.SpawnTimestep;
        _waveCurrentIndex = 0;
        _waveCurrentTimer = 0;
    }

    private async UniTask StartWaveAsync()
    {
        _isWaveFinished = false;
        StartWaveTimerAsync().Forget();

        _waveCurrentIndex++;
        _spawnTimestep -= (_spawnTimestep * _wavesConfig.DecreaseSpawnTimestepMultiplier);
        _totalEnemyAmountPerWave = (int)(_wavesConfig.WaveTimer / _spawnTimestep);
        OnNewWaveStarted?.Invoke(_waveCurrentIndex, _totalEnemyAmountPerWave);
        
        while (!_cts.IsCancellationRequested && !_stopWaveFlag)
        {
            var dirX = Random.Range(_wavesConfig.MinDirX, _wavesConfig.MaxDirX);
            var dirY = Random.Range(_wavesConfig.MinDirY, _wavesConfig.MaxDirY);
            var pos = new Vector2(dirX, dirY)
                .normalized * Random.Range(_wavesConfig.MinSpawnDistance, _wavesConfig.MaxSpawnDistance);
            
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
        
        while (_waveCurrentTimer < _wavesConfig.WaveTimer && !_cts.IsCancellationRequested)
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
