using System;

namespace _Project.Scripts.MVP
{
    public class UpgradeStructurePresenter : IDisposable
    {
        private readonly IUpgradeStructureService _upgradeStructureService;
        private readonly UpgradeStructurePanelView _upgradeStructurePanelView;
        private readonly ScreenInputHandler _screenInputHandler;

        public UpgradeStructurePresenter(
            UpgradeStructurePanelView upgradeStructurePanelView,
            IUpgradeStructureService upgradeStructureService,
            ScreenInputHandler screenInputHandler)
        {
           _upgradeStructurePanelView = upgradeStructurePanelView;
           _upgradeStructureService = upgradeStructureService;
           _screenInputHandler = screenInputHandler;
        }

        public void Dispose()
        {
            _upgradeStructurePanelView.OnUpgradeStructClicked -= OnUpgradeStructClickedHandler;
            _upgradeStructureService.OnStructUpgraded -= OnUpgradeStructHandler;
            _screenInputHandler.OnStrongholdClicked -= OnStrongholdClickedHandler;
        }

        public void Init()
        {
            _upgradeStructurePanelView.OnUpgradeStructClicked += OnUpgradeStructClickedHandler;
            _upgradeStructureService.OnStructUpgraded += OnUpgradeStructHandler;
            _screenInputHandler.OnStrongholdClicked += OnStrongholdClickedHandler;
        }

        private void OnStrongholdClickedHandler()
        {
            if(_upgradeStructurePanelView.isActiveAndEnabled)
                _upgradeStructurePanelView.Hide();
            else
                _upgradeStructurePanelView.Show();
        }

        private void OnUpgradeStructHandler()
        {
            _upgradeStructurePanelView.Hide();
        }

        private void OnUpgradeStructClickedHandler(UpgradeStructureEnum upgradeEnum)
        {
            _upgradeStructureService.UpgradeStruct(upgradeEnum);
        }
    }
}