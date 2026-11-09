using _Project.Scripts.MVP;
using UnityEngine;
using Zenject;

namespace _Project.Scripts
{
    public class BattleSceneInstaller : MonoInstaller
    {
        [Header("Panels")]
        [SerializeField] private InfoPanelView _infoPanelView;
        [SerializeField] private BattleStartPanelView _battleStartPanelView;
        [SerializeField] private ChoseActionPanelView _choseActionPanelView;
        [SerializeField] private EndGamePanelView _endGamePanelView;

        [Header("Other scene objects")]
        [SerializeField] private ScreenInputHandler _screenInputHandler;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EntityFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<PoolProvider>().AsSingle();
            Container.BindInstance(_screenInputHandler);
            Container.BindInterfacesAndSelfTo<MediatorPresenter>().AsSingle().WithArguments(
                _battleStartPanelView,
                _infoPanelView,
                _choseActionPanelView,
                _endGamePanelView);
            Container.BindInterfacesAndSelfTo<CoinsWallet>().AsSingle();
            Container.BindInterfacesAndSelfTo<RewardService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawnService>().AsSingle();
            Container.BindInterfacesAndSelfTo<WavesControlService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleControlService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleInfoService>().AsSingle();
            Container.BindInterfacesAndSelfTo<StructureConstructService>().AsSingle();
            Container.BindInterfacesAndSelfTo<UpgradeStructureService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleSceneBootstrap>().AsSingle();
        }
    }
}
