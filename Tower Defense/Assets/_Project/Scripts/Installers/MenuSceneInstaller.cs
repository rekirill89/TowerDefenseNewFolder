using UnityEngine;
using Zenject;
using _Project.Scripts.MVP;
using _Project.Scripts.Services;

namespace _Project.Scripts.Installers
{
    public class MenuSceneInstaller : MonoInstaller
    {
        [SerializeField] private MenuPanelView _menuPanelView;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MenuPanelPresenter>().AsSingle().WithArguments(_menuPanelView);
            Container.BindInterfacesAndSelfTo<MenuSceneBootstrap>().AsSingle();
        }
    }
}