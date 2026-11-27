using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
using _Project.Scripts.Configs;

namespace _Project.Scripts.Services
{
    public class WavesControlService : IWavesControlService
    {
        public event Action<int, int> OnNewWaveStarted; 
        public event Action<int> OnTimerIncremented;
        
        private readonly WavesConfig _wavesConfig;
        private readonly IEnemySpawnService _enemySpawnService;
        
        private int _enemiesInCurrentWave = 0;
        private float _spawnInterval; 
        private float _waveElapsedTime;
        private int _currentWaveIndex;

        private bool _isWaveFinished = false;
        private bool _isWaveTimeOver = false;
        
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public WavesControlService(ConfigProvider configProvider, IEnemySpawnService enemySpawnService)
        {
            _wavesConfig = configProvider.GetConfig<WavesConfig>();
            _enemySpawnService = enemySpawnService;
            _spawnInterval = _wavesConfig.SpawnInterval;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public async UniTask RunWavesLoopAsync()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                RunWaveAsync().Forget();
                await UniTask.WaitUntil(() => _isWaveFinished, cancellationToken: _cancellationTokenSource.Token);
                await UniTask.WaitForSeconds(_wavesConfig.BreakBetweenWaves, cancellationToken: _cancellationTokenSource.Token);
            }    
        }


        public void StopWaves()
        {
            _cancellationTokenSource.Cancel();
        }

        public void Reset()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            _spawnInterval = _wavesConfig.SpawnInterval;
            _currentWaveIndex = 0;
            _waveElapsedTime = 0;
        }

        private async UniTask RunWaveAsync()
        {
            _isWaveFinished = false;
            RunWaveTimerAsync().Forget();

            _currentWaveIndex++;
            _spawnInterval -= (_spawnInterval * _wavesConfig.SpawnIntervalDecreaseFactor);
            _enemiesInCurrentWave = (int)(_wavesConfig.WaveDuration / _spawnInterval);
            OnNewWaveStarted?.Invoke(_currentWaveIndex, _enemiesInCurrentWave);
            
            while (!_cancellationTokenSource.IsCancellationRequested && !_isWaveTimeOver)
            {
                var directionX = Random.Range(_wavesConfig.MinDirectionX, _wavesConfig.MaxDirectionX);
                var directionY = Random.Range(_wavesConfig.MinDirectionY, _wavesConfig.MaxDirectionY);
                var position = new Vector2(directionX, directionY)
                    .normalized * Random.Range(_wavesConfig.MinSpawnDistance, _wavesConfig.MaxSpawnDistance);
                
                _enemySpawnService.SpawnEnemy(position);

                await UniTask.WaitForSeconds(_spawnInterval, cancellationToken: _cancellationTokenSource.Token);
            }

            _isWaveFinished = true;
        }

        private async UniTask RunWaveTimerAsync()
        {
            _isWaveTimeOver = false;
            _waveElapsedTime = 0;

            int currentSecond = 0;
            int lastReportedSecond = 0;
            
            while (_waveElapsedTime < _wavesConfig.WaveDuration && !_cancellationTokenSource.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: _cancellationTokenSource.Token);
                _waveElapsedTime += Time.deltaTime;
                currentSecond = (int)_waveElapsedTime;
                
                if (currentSecond >= lastReportedSecond)
                {
                    OnTimerIncremented?.Invoke(currentSecond);
                    lastReportedSecond = currentSecond;
                }
            }
            
            _isWaveTimeOver = true;
        }
    }
}
