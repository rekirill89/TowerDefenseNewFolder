using System;

namespace _Project.Scripts.Services
{
    public class BattleInfoService : IBattleInfoService
    {
        private readonly IEnemySpawnService _enemySpawnService;
        private readonly IWavesControlService _wavesControlService;
        private BattleInfoData _battleInfoData;

        public BattleInfoService(IEnemySpawnService enemySpawnService, IWavesControlService wavesControlService)
        {
            _enemySpawnService = enemySpawnService;
            _wavesControlService = wavesControlService;
            _battleInfoData = new BattleInfoData();
        }

        public void Dispose()
        {
            _enemySpawnService.OnEnemyKilled -= OnEnemyKilledHandler;
            _wavesControlService.OnNewWaveStarted -= OnNewWaveStartedHandler;
        }

        public void Init()
        {
            _enemySpawnService.OnEnemyKilled += OnEnemyKilledHandler;
            _wavesControlService.OnNewWaveStarted += OnNewWaveStartedHandler;
        }

        public BattleInfoData GetInfo()
        {
            return _battleInfoData;
        }

        public void Reset()
        {
            _battleInfoData = new BattleInfoData();
        }

        private void OnNewWaveStartedHandler(int _, int __)
        {
            _battleInfoData.AddSurvivedWave();
        }

        private void OnEnemyKilledHandler()
        {
            _battleInfoData.AddKilledEnemy();
        }
    }

    public class BattleInfoData
    {
        public int EnemiesKilled {get; private set;}
        public int WavesSurvived {get; private set;}

        public void AddKilledEnemy()
        {
            EnemiesKilled++;
        }
        
        public void AddSurvivedWave()
        {
            WavesSurvived++;
        }
    }
}