using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts
{
    public class BattleControlService : IBattleControlService
    {
        public event Action OnBattleFinished;
        public event Action OnBattleStarted;

        private readonly IWavesControlService _wavesControlService;
        private readonly Stronghold _stronghold;
        private readonly StrongholdHealthSO _strongholdHealthSO;
        private readonly IEnemySpawnService _enemySpawnService;
        private readonly IStructureConstructService _structureConstructService;
        private readonly ICoinsWallet _coinsWallet;
        private readonly IUpgradeStructureService _upgradeStructureService;

        private readonly IBattleInfoService _battleInfoService;

        private BattleState _state;

        public BattleControlService(
            IWavesControlService wavesControlService, 
            IEnemySpawnService enemySpawnService,
            IStructureConstructService structureConstructService,
            ICoinsWallet coinsWallet,
            IUpgradeStructureService upgradeStructureService,
            IBattleInfoService battleInfoService,
            Stronghold stronghold,
            StrongholdHealthSO strongholdHealthSO)
        {
            _wavesControlService = wavesControlService;
            _enemySpawnService = enemySpawnService;
            _structureConstructService = structureConstructService;
            _coinsWallet = coinsWallet;
            _upgradeStructureService = upgradeStructureService;
            _battleInfoService = battleInfoService;
            _stronghold = stronghold;
            _strongholdHealthSO = strongholdHealthSO;
        }

        public void Dispose()
        {
            _stronghold.OnDestroyed -= FinishBattle;
        }

        public void Init()
        {
            _stronghold.OnDestroyed += FinishBattle;
            _state = BattleState.NotStarted;
        }

        public void StartBattle()
        {
            Debug.Log("Battle Started");
            _stronghold.Init(_strongholdHealthSO);
            _state = BattleState.InProgress;
            _stronghold.StartActing();
            _wavesControlService.RunWavesLoopAsync().Forget();
        }

        public void RestartBattle()
        {
            ClearBattlefield();
            StartBattle();
            OnBattleStarted?.Invoke();
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
            _stronghold.Reset();
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