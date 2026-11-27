using System;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Configs;
using _Project.Scripts.GameEntities;

namespace _Project.Scripts.Services
{
    public class BattleControlService : IBattleControlService
    {
        public event Action OnBattleFinished;
        public event Action OnBattleStarted;
        public event Action<Stronghold> OnStrongholdCreated;

        private readonly IWavesControlService _wavesControlService;
        private readonly IEnemySpawnService _enemySpawnService;
        private readonly ITowerConstructionService _towerConstructionService;
        private readonly IEntityFactory _entityFactory;
        private readonly ICoinsWallet _coinsWallet;
        private readonly IStrongholdUpgradeService _strongholdUpgradeService;
        private readonly IBattleInfoService _battleInfoService;
        private readonly BattleConfig _battleConfig;
        private Stronghold _stronghold;
        private StrongholdModel _strongholdModel;

        private BattleState _state;

        public BattleControlService(
            IWavesControlService wavesControlService, 
            IEnemySpawnService enemySpawnService,
            ITowerConstructionService towerConstructionService,
            IEntityFactory entityFactory,
            ICoinsWallet coinsWallet,
            IStrongholdUpgradeService strongholdUpgradeService,
            IBattleInfoService battleInfoService,
            ConfigProvider configProvider)
        {
            _wavesControlService = wavesControlService;
            _enemySpawnService = enemySpawnService;
            _towerConstructionService = towerConstructionService;
            _entityFactory = entityFactory;
            _coinsWallet = coinsWallet;
            _strongholdUpgradeService = strongholdUpgradeService;
            _battleInfoService = battleInfoService;
            _battleConfig = configProvider.GetConfig<BattleConfig>();
        }

        public void Dispose()
        {
            if (_strongholdModel != null)
                _strongholdModel.OnDestroyed -= FinishBattle;
        }

        public void Init()
        {
            _state = BattleState.NotStarted;
        }

        public void StartBattle()
        {
            if (_stronghold == null)
                CreateStronghold();

            _strongholdModel.Activate();
            
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
            _strongholdModel = _stronghold.Model;
            _strongholdModel.OnDestroyed += FinishBattle;
            _enemySpawnService.SetEnemiesDestination(_stronghold.transform.position);
            _strongholdUpgradeService.SetStronghold(_strongholdModel);
            OnStrongholdCreated?.Invoke(_stronghold);
        }

        private void FinishBattle()
        {
            OnBattleFinished?.Invoke();
            _wavesControlService.StopWaves();
            _enemySpawnService.StopAllEnemies();
            _towerConstructionService.StopAllTowers();
            
            _state = BattleState.Finished;
        }

        private void ClearBattlefield()
        {
            _stronghold.ResetState();
            _enemySpawnService.RemoveAllEnemies();
            _towerConstructionService.RemoveAllTowers();
            _coinsWallet.ResetBalance();
            _strongholdUpgradeService.ResetUpgradeStats();
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
