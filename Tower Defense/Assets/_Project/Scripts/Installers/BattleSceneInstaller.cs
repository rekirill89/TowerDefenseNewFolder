using UnityEngine;
using Zenject;
using _Project.Scripts.GameEntities;
using _Project.Scripts.MVP;
using _Project.Scripts.Services;

namespace _Project.Scripts.Installers
{
    public class BattleSceneInstaller : MonoInstaller
    {
        [Header("Panels")]
        [SerializeField] private InfoPanelView _infoPanelView;
        [SerializeField] private BattleStartPanelView _battleStartPanelView;
        [SerializeField] private ConstructionPanelView _constructionPanelView;
        [SerializeField] private EndGamePanelView _endGamePanelView;

        [Header("Other scene objects")]
        [SerializeField] private ScreenInputHandler _screenInputHandler;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EntityFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<PoolProvider>().AsSingle();
            Container.BindInstance(_screenInputHandler);
            Container.BindInterfacesAndSelfTo<BattlePresentersRoot>().AsSingle().WithArguments(
                _battleStartPanelView,
                _infoPanelView,
                _constructionPanelView,
                _endGamePanelView);
            Container.BindInterfacesAndSelfTo<CoinsWallet>().AsSingle();
            Container.BindInterfacesAndSelfTo<RewardService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawnService>().AsSingle();
            Container.BindInterfacesAndSelfTo<WavesControlService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleControlService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleInfoService>().AsSingle();
            Container.BindInterfacesAndSelfTo<TowerConstructionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<StrongholdUpgradeService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleSceneBootstrap>().AsSingle();
        }
    }
}
