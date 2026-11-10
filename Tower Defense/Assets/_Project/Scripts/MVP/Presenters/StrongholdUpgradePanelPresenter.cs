using System;
using _Project.Scripts.GameEntities;
using _Project.Scripts.Services;

namespace _Project.Scripts.MVP
{
    public class StrongholdUpgradePanelPresenter : IDisposable
    {
        private readonly IStrongholdUpgradeService _strongholdUpgradeService;
        private readonly StrongholdUpgradePanelView _upgradePanelView;
        private readonly ScreenInputHandler _screenInputHandler;

        public StrongholdUpgradePanelPresenter(
            StrongholdUpgradePanelView upgradePanelView,
            IStrongholdUpgradeService strongholdUpgradeService,
            ScreenInputHandler screenInputHandler)
        {
           _upgradePanelView = upgradePanelView;
           _strongholdUpgradeService = strongholdUpgradeService;
           _screenInputHandler = screenInputHandler;
        }

        public void Dispose()
        {
            _upgradePanelView.OnUpgradeClicked -= OnUpgradeClickedHandler;
            _strongholdUpgradeService.OnUpgraded -= OnStrongholdUpgradedHandler;
            _screenInputHandler.OnStrongholdClicked -= OnStrongholdClickedHandler;
        }

        public void Init()
        {
            _upgradePanelView.OnUpgradeClicked += OnUpgradeClickedHandler;
            _strongholdUpgradeService.OnUpgraded += OnStrongholdUpgradedHandler;
            _screenInputHandler.OnStrongholdClicked += OnStrongholdClickedHandler;
        }

        private void OnStrongholdClickedHandler()
        {
            _upgradePanelView.Toggle();
        }

        private void OnStrongholdUpgradedHandler()
        {
            _upgradePanelView.Hide();
        }

        private void OnUpgradeClickedHandler(StrongholdUpgradeType upgradeType)
        {
            _strongholdUpgradeService.Upgrade(upgradeType);
        }
    }
}