using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.MVP
{
    public class MediatorPresenter : IDisposable
    {
        private readonly IInstantiator _instantiator;
        private readonly IBattleControlService _battleControlService;
        private readonly BattleStartPanelView _battleStartPanelView;
        private readonly InfoPanelView _infoPanelView;
        private readonly ChoseActionPanelView _choseActionPanelView;
        private readonly EndGamePanelView _endGamePanelView;

        private readonly DisposableCollector _disposableCollector = new DisposableCollector();

        public MediatorPresenter(
            IInstantiator instantiator,
            IBattleControlService battleControlService,
            BattleStartPanelView battleStartPanelView,
            InfoPanelView infoPanelView,
            ChoseActionPanelView choseActionPanelView,
            EndGamePanelView endGamePanelView)
        {
            _instantiator = instantiator;
            _battleControlService = battleControlService;

            _battleStartPanelView = battleStartPanelView;
            _infoPanelView = infoPanelView;
            _choseActionPanelView =  choseActionPanelView;
            _endGamePanelView = endGamePanelView;
        }

        public void Dispose()
        {
            _battleControlService.OnStrongholdCreated -= OnStrongholdCreatedHandler;
            _disposableCollector.Dispose();
        }

        public void Init()
        {
            var infoPanelPresenter = _instantiator.Instantiate<InfoPanelPresenter>(new object[] { _infoPanelView });
            var battleControllerPresenter = _instantiator.Instantiate<BattleControllerPresenter>(new object[] { _battleStartPanelView });
            var choseActionPresenter = _instantiator.Instantiate<ChoseActionPresenter>(new object[] { _choseActionPanelView });
            var endGamePresenter = _instantiator.Instantiate<EndGamePresenter>(new object[] { _endGamePanelView });

            infoPanelPresenter.Init();
            battleControllerPresenter.Init();
            choseActionPresenter.Init();
            endGamePresenter.Init();

            _disposableCollector.Register(new List<IDisposable>()
            {
                infoPanelPresenter,
                battleControllerPresenter,
                choseActionPresenter,
                endGamePresenter
            });

            _battleControlService.OnStrongholdCreated += OnStrongholdCreatedHandler;
        }

        private void OnStrongholdCreatedHandler(Stronghold stronghold)
        {
            if (!stronghold.TryGetComponent(out StrongholdViews strongholdViews))
            {
                Debug.LogError($"{stronghold.name} has no {nameof(StrongholdViews)}", stronghold);
                return;
            }

            var upgradeStructurePresenter = _instantiator.Instantiate<UpgradeStructurePresenter>(
                new object[] { strongholdViews.UpgradePanel });
            upgradeStructurePresenter.Init();

            _disposableCollector.Register(upgradeStructurePresenter);
        }
    }
}
