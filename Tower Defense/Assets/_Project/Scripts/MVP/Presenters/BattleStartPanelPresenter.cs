using System;
using _Project.Scripts.Services;

namespace _Project.Scripts.MVP
{
    public class BattleStartPanelPresenter : IDisposable
    {
        private readonly IBattleControlService _battleControlService;
        private readonly BattleStartPanelView _battleStartPanelView;
        
        public BattleStartPanelPresenter(BattleStartPanelView battleStartPanelView, IBattleControlService battleControlService)
        {
            _battleStartPanelView = battleStartPanelView;
            _battleControlService = battleControlService;
        }

        public void Dispose()
        {
            _battleStartPanelView.StartButton.OnClick -= OnStartButtonClickedHandler;
        }

        public void Init()
        {
            _battleStartPanelView.StartButton.OnClick += OnStartButtonClickedHandler;
        }

        private void OnStartButtonClickedHandler()
        {
            _battleControlService.StartBattle();
            _battleStartPanelView.Hide();
        }
    }
}