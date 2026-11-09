using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts
{
    public class BattleControlService : IBattleControlService
    {
        public event Action OnBattleFinished;
        public event Action OnBattleStarted;
        public event Action<Stronghold> OnStrongholdCreated;

        private readonly IWavesControlService _wavesControlService;
        private readonly IEnemySpawnService _enemySpawnService;
        private readonly IStructureConstructService _structureConstructService;
        private readonly IEntityFactory _entityFactory;
        private readonly ICoinsWallet _coinsWallet;
        private readonly IUpgradeStructureService _upgradeStructureService;
        private readonly IBattleInfoService _battleInfoService;
        private readonly BattleConfig _battleConfig;
        private Stronghold _stronghold;

        private BattleState _state;

        public BattleControlService(
            IWavesControlService wavesControlService, 
            IEnemySpawnService enemySpawnService,
            IStructureConstructService structureConstructService,
            IEntityFactory entityFactory,
            ICoinsWallet coinsWallet,
            IUpgradeStructureService upgradeStructureService,
            IBattleInfoService battleInfoService,
            ConfigProvider configProvider)
        {
            _wavesControlService = wavesControlService;
            _enemySpawnService = enemySpawnService;
            _structureConstructService = structureConstructService;
            _entityFactory = entityFactory;
            _coinsWallet = coinsWallet;
            _upgradeStructureService = upgradeStructureService;
            _battleInfoService = battleInfoService;
            _battleConfig = configProvider.GetConfig<BattleConfig>();
        }

        public void Dispose()
        {
            if (_stronghold != null)
                _stronghold.OnDestroyed -= FinishBattle;
        }

        public void Init()
        {
            _state = BattleState.NotStarted;
        }

        public void StartBattle()
        {
            Debug.Log("Battle Started");
            if (_stronghold == null)
                CreateStronghold();

            _stronghold.StartActing();
            
            _state = BattleState.InProgress;
            _wavesControlService.RunWavesLoopAsync().Forget();
        }

        public void RestartBattle()
        {
            ClearBattlefield();
            StartBattle();
            OnBattleStarted?.Invoke();
        }

        private void CreateStronghold()
        {
            _stronghold = _entityFactory.Create<Stronghold>(_battleConfig.StrongholdPosition);
            _stronghold.OnDestroyed += FinishBattle;
            _enemySpawnService.SetEnemiesTarget(_stronghold.transform.position);
            _upgradeStructureService.SetStronghold(_stronghold);
            OnStrongholdCreated?.Invoke(_stronghold);
        }

        private void FinishBattle()
        {
            Debug.Log("Battle Finished");
            OnBattleFinished?.Invoke();
            _wavesControlService.StopWave();
            _enemySpawnService.StopAllEnemies();
            _structureConstructService.StopAllTowers();
            
            _state = BattleState.Finished;
        }

        private void ClearBattlefield()
        {
            _stronghold.ResetState();
            _enemySpawnService.RemoveAllEnemies();
            _structureConstructService.RemoveAllTowers();
            _coinsWallet.ResetBalance();
            _upgradeStructureService.ResetUpgradeStats();
            _wavesControlService.Reset();
            _battleInfoService.Reset();
        }
    }

    public enum BattleState
    {
        NotStarted,
        InProgress,
        Finished
    }
}
