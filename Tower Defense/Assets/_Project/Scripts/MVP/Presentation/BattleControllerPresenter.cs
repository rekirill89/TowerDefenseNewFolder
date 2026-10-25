using System;
using _Project.Scripts.MVP;

namespace _Project.Scripts.MVP
{
    public class BattleControllerPresenter : IDisposable
    {
        private readonly IBattleControlService _battleControlService;
        private readonly BattleStartPanelView _battleStartPanelView;
        
        public BattleControllerPresenter(BattleStartPanelView battleStartPanelView, IBattleControlService battleControlService)
        {
            _battleStartPanelView = battleStartPanelView;
            _battleControlService = battleControlService;
        }

        public void Dispose()
        {
            _battleStartPanelView.StartButton.OnClick -= OnStartBattleButtonOnClickHandler;
        }

        public void Init()
        {
            _battleStartPanelView.StartButton.OnClick += OnStartBattleButtonOnClickHandler;
        }

        private void OnStartBattleButtonOnClickHandler()
        {
            _battleControlService.StartBattle();
            _battleStartPanelView.Hide();
        }
    }
}