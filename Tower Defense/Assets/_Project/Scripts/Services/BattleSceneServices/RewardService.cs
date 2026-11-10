using System;
using UnityEngine;
using _Project.Scripts.Configs;

namespace _Project.Scripts.Services
{
    public class RewardService : IRewardService
    {

        private readonly IBattleControlService _battleControlService;
        private readonly IBattleInfoService _battleInfoService;
        private readonly IEnemySpawnService _enemySpawnService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ICoinsWallet _coinsWallet;
        private readonly IMetaCurrencyWallet _metaCurrencyWallet;
        private readonly RewardsConfig _rewardsConfig;

        public RewardService(
            IBattleControlService battleControlService,
            IBattleInfoService battleInfoService,
            ConfigProvider configProvider,
            IEnemySpawnService enemySpawnService, 
            ISaveLoadService saveLoadService,
            ICoinsWallet coinsWallet,
            IMetaCurrencyWallet metaCurrencyWallet)
        {
            _battleControlService = battleControlService;
            _battleInfoService = battleInfoService;
            _enemySpawnService = enemySpawnService;
            _saveLoadService = saveLoadService;
            _coinsWallet = coinsWallet;
            _metaCurrencyWallet = metaCurrencyWallet;
            _rewardsConfig = configProvider.GetConfig<RewardsConfig>();
        }

        public void Dispose()
        {
            _enemySpawnService.OnEnemyKilled -= OnEnemyKilledHandler;
            _battleControlService.OnBattleFinished -= OnBattleFinishedHandler;
        }

        public void Init()
        {
            _enemySpawnService.OnEnemyKilled += OnEnemyKilledHandler;
            _battleControlService.OnBattleFinished += OnBattleFinishedHandler;
        }

        private void OnBattleFinishedHandler()
        {
            var info = _battleInfoService.GetInfo();
            var value = _rewardsConfig.MetaCurrencyPerWave * info.WavesSurvived +
                        _rewardsConfig.MetaCurrencyPerEnemy * info.EnemiesKilled;
            _metaCurrencyWallet.Add(value);
            _saveLoadService.Save(new SaveData(_metaCurrencyWallet.Balance));
            Debug.Log("Collected: " + value + ", current balance: " + _metaCurrencyWallet.Balance);
        }

        private void OnEnemyKilledHandler()
        {
            _coinsWallet.Add(_rewardsConfig.CoinsPerEnemy);
        }
    }
}