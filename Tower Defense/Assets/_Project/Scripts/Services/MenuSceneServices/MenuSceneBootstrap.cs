using System;
using _Project.Scripts.MVP;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts
{
    public class MenuSceneBootstrap : IInitializable
    {
        private readonly MenuPanelPresenter _menuPanelPresenter;
        
        public MenuSceneBootstrap(MenuPanelPresenter menuPanelPresenter)
        {
            _menuPanelPresenter = menuPanelPresenter;
        }
        
        public void Initialize()
        {
            _menuPanelPresenter.Init();
        }
    }
}