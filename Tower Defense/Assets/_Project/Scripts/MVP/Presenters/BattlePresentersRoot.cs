using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using _Project.Scripts.GameEntities;
using _Project.Scripts.Services;

namespace _Project.Scripts.MVP
{
    public class BattlePresentersRoot : IDisposable
    {
        private readonly IInstantiator _instantiator;
        private readonly IBattleControlService _battleControlService;
        private readonly BattleStartPanelView _battleStartPanelView;
        private readonly InfoPanelView _infoPanelView;
        private readonly ConstructionPanelView _constructionPanelView;
        private readonly EndGamePanelView _endGamePanelView;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public BattlePresentersRoot(
            IInstantiator instantiator,
            IBattleControlService battleControlService,
            BattleStartPanelView battleStartPanelView,
            InfoPanelView infoPanelView,
            ConstructionPanelView constructionPanelView,
            EndGamePanelView endGamePanelView)
        {
            _instantiator = instantiator;
            _battleControlService = battleControlService;

            _battleStartPanelView = battleStartPanelView;
            _infoPanelView = infoPanelView;
            _constructionPanelView =  constructionPanelView;
            _endGamePanelView = endGamePanelView;
        }

        public void Dispose()
        {
            _battleControlService.OnStrongholdCreated -= OnStrongholdCreatedHandler;
            _disposables.Dispose();
        }

        public void Init()
        {
            var infoPanelPresenter = _instantiator.Instantiate<InfoPanelPresenter>(new object[] { _infoPanelView });
            var battleStartPanelPresenter = _instantiator.Instantiate<BattleStartPanelPresenter>(new object[] { _battleStartPanelView });
            var constructionPanelPresenter = _instantiator.Instantiate<ConstructionPanelPresenter>(new object[] { _constructionPanelView });
            var endGamePanelPresenter = _instantiator.Instantiate<EndGamePanelPresenter>(new object[] { _endGamePanelView });

            infoPanelPresenter.Init();
            battleStartPanelPresenter.Init();
            constructionPanelPresenter.Init();
            endGamePanelPresenter.Init();

            _disposables.Add(new List<IDisposable>()
            {
                infoPanelPresenter,
                battleStartPanelPresenter,
                constructionPanelPresenter,
                endGamePanelPresenter
            });

            _battleControlService.OnStrongholdCreated += OnStrongholdCreatedHandler;
        }

        private void OnStrongholdCreatedHandler(Stronghold stronghold)
        {
            if (!stronghold.TryGetComponent(out StrongholdUI strongholdUI))
            {
                Debug.LogError($"{stronghold.name} has no {nameof(StrongholdUI)}", stronghold);
                return;
            }

            var strongholdUpgradePanelPresenter = _instantiator.Instantiate<StrongholdUpgradePanelPresenter>(
                new object[] { strongholdUI.UpgradePanel });
            strongholdUpgradePanelPresenter.Init();

            _disposables.Add(strongholdUpgradePanelPresenter);
        }
    }
}
