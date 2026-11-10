using System;
using _Project.Scripts.Services;

namespace _Project.Scripts.MVP
{
    public class EndGamePanelPresenter : IDisposable
    {
        private readonly IBattleControlService _battleControlService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly EndGamePanelView _endGamePanelView;

        public EndGamePanelPresenter(
            EndGamePanelView endGamePanelView,
            IBattleControlService battleControlService, 
            ISceneLoaderService sceneLoaderService)
        {
            _endGamePanelView = endGamePanelView;
            _battleControlService = battleControlService;
            _sceneLoaderService = sceneLoaderService;
        }

        public void Dispose()
        {
            _endGamePanelView.RestartButton.OnClick -= OnRestartButtonClickedHandler;
            _endGamePanelView.MenuButton.OnClick -= _sceneLoaderService.LoadMenuScene;
            
            _battleControlService.OnBattleFinished -= _endGamePanelView.Show;
            _battleControlService.OnBattleStarted -= _endGamePanelView.Hide;
        }

        public void Init()
        {
            _endGamePanelView.RestartButton.OnClick += OnRestartButtonClickedHandler;
            _endGamePanelView.MenuButton.OnClick += _sceneLoaderService.LoadMenuScene;

            _battleControlService.OnBattleFinished += _endGamePanelView.Show;
            _battleControlService.OnBattleStarted += _endGamePanelView.Hide;
        }

        private void OnRestartButtonClickedHandler()
        {
            _battleControlService.RestartBattle();
        }
    }
}