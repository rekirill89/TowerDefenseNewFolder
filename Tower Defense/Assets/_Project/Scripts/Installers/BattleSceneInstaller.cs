using _Project.Scripts.MVP;
using UnityEngine;
using Zenject;

namespace _Project.Scripts
{
    public class BattleSceneInstaller : MonoInstaller
    {
        [Header("Prefabs")]
        [SerializeField] private Stronghold _stronghold;
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Archer _archerPrefab;
        [SerializeField] private Cannon _cannonPrefab ;
    
        [Header("Configs")]
        [SerializeField] private RewardsSO _rewardSO;
        [SerializeField] private UpgradeStructureSO _upgradeStrongholdSO;
        [SerializeField] private StrongholdHealthSO _strongholdHealthSO;
        [SerializeField] private EnemySO _enemySO;
        [SerializeField] private WavesSO _wavesSO;
    
        [Header("Panels")]
        [SerializeField] private InfoPanelView _infoPanelView;
        [SerializeField] private BattleStartPanelView _battleStartPanelView;
        [SerializeField] private ChoseActionPanelView _choseActionPanelView;
        [SerializeField] private UpgradeStructurePanelView _upgradeStructurePanelView;
        [SerializeField] private EndGamePanelView _endGamePanelView;
    
        [Header("Other scene objects")]
        [SerializeField] private ScreenInputHandler _screenInputHandler;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MediatorPresenter>().AsSingle().WithArguments(
                _battleStartPanelView,
                _infoPanelView,
                _choseActionPanelView,
                _upgradeStructurePanelView,
                _endGamePanelView,
                _screenInputHandler);
            Container.BindInterfacesAndSelfTo<CoinsWallet>().AsSingle().WithArguments(_rewardSO);
            Container.BindInterfacesAndSelfTo<RewardService>().AsSingle().WithArguments(_rewardSO);
            Container.BindInterfacesAndSelfTo<EnemySpawnService>().AsSingle()
                .WithArguments(_enemyPrefab, _stronghold, _enemySO);
            Container.BindInterfacesAndSelfTo<WavesControlService>().AsSingle().WithArguments(_wavesSO);
            Container.BindInterfacesAndSelfTo<BattleControlService>().AsSingle()
                .WithArguments(_stronghold, _strongholdHealthSO);
            Container.BindInterfacesAndSelfTo<BattleInfoService>().AsSingle();
            Container.BindInterfacesAndSelfTo<StructureConstructService>().AsSingle()
                .WithArguments(_archerPrefab, _cannonPrefab); 
            Container.BindInterfacesAndSelfTo<UpgradeStructureService>().AsSingle()
                .WithArguments(_stronghold, _upgradeStrongholdSO); 
            Container.BindInterfacesAndSelfTo<BattleSceneBootstrap>().AsSingle();
        }
    }
}