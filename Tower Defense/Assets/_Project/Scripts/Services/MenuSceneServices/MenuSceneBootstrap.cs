using System;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using _Project.Scripts.MVP;

namespace _Project.Scripts.Services
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