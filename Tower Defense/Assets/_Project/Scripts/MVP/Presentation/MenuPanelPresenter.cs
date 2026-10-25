using System;
using _Project.Scripts.MVP;

namespace _Project.Scripts.MVP
{
    public class MenuPanelPresenter : IDisposable
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IMetaCurrencyWallet _metaCurrencyWallet;
        private readonly MenuPanelView _menuPanelView;

        public MenuPanelPresenter(
            MenuPanelView menuPanelView,
            IMetaCurrencyWallet metaCurrencyWallet,
            ISceneLoaderService sceneLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;
            _metaCurrencyWallet = metaCurrencyWallet;
            _menuPanelView = menuPanelView;
        }

        public void Dispose()
        {
            _menuPanelView.StartButton.OnClick -= OnStartButtonClickedHandler;
            _metaCurrencyWallet.OnBalanceChanged -= OnMetaBalanceChangedHandler;
        }

        public void Init()
        {
            _menuPanelView.StartButton.OnClick += OnStartButtonClickedHandler;
            _metaCurrencyWallet.OnBalanceChanged += OnMetaBalanceChangedHandler;
            
            _menuPanelView.SetBalanceText(_metaCurrencyWallet.CurrentBalance);
        }

        private void OnMetaBalanceChangedHandler(int value)
        {
            _menuPanelView.SetBalanceText(value);
        }

        private void OnStartButtonClickedHandler()
        {
            _sceneLoaderService.LoadBattleScene();
        }

    }
}