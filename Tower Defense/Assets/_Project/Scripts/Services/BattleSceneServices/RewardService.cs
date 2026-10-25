using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class RewardService : IRewardService
    {
        public event RewardGained OnRewardGained;

        private readonly IBattleControlService _battleControlService;
        private readonly IBattleInfoService _battleInfoService;
        private readonly IEnemySpawnService _enemySpawnService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ICoinsWallet _coinsWallet;
        private readonly IMetaCurrencyWallet _metaWallet;
        private RewardsSO _rewardSO;

        public RewardService(
            IBattleControlService battleControlService,
            IBattleInfoService battleInfoService,
            RewardsSO rewardsSO,
            IEnemySpawnService enemySpawnService, 
            ISaveLoadService saveLoadService,
            ICoinsWallet coinsWallet,
            IMetaCurrencyWallet metaWallet)
        {
            _battleControlService = battleControlService;
            _battleInfoService = battleInfoService;
            _enemySpawnService = enemySpawnService;
            _saveLoadService = saveLoadService;
            _coinsWallet = coinsWallet;
            _metaWallet = metaWallet;
            _rewardSO = rewardsSO;
        }

        public void Dispose()
        {
            _enemySpawnService.OnEnemySpawned -= OnEnemySpawnedHandler; 
            _battleControlService.OnBattleFinished -= OnBattleFinishedHandler;
        }

        public void Init()
        {
            _enemySpawnService.OnEnemySpawned += OnEnemySpawnedHandler;
            _battleControlService.OnBattleFinished += OnBattleFinishedHandler;
        }

        private void OnBattleFinishedHandler()
        {
            var info = _battleInfoService.GetInfo();
            var value = _rewardSO.MetaMoneyPerWave * info.WavesSurvivedAmount +
                        _rewardSO.MetaMoneyPerEnemy * info.EnemyKilledAmount;
            _metaWallet.Collect(value);
            _saveLoadService.Save(new SaveData(_metaWallet.CurrentBalance));
            Debug.Log("Collected: " + value + ", current balance: " + _metaWallet.CurrentBalance);
        }

        private void OnEnemySpawnedHandler(Enemy enemy)
        {
            enemy.OnKilled += OnEnemyKilledHandler;
        }

        private void OnEnemyKilledHandler(Enemy enemy)
        {
            _coinsWallet.Collect(_rewardSO.CoinsPerEnemy);
            enemy.OnKilled -= OnEnemyKilledHandler;
            OnRewardGained?.Invoke(RewardEnum.Coins, _rewardSO.CoinsPerEnemy);
        }
    }

    public enum RewardEnum
    {
        Coins
    }
}