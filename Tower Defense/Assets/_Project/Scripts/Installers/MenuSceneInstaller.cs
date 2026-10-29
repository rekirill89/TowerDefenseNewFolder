using _Project.Scripts.MVP;
using UnityEngine;
using Zenject;

namespace _Project.Scripts
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