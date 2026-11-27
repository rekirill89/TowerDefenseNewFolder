using System.Collections.Generic;
using UnityEngine;
using Zenject;
using _Project.Scripts.Configs;
using _Project.Scripts.Services;

namespace _Project.Scripts.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private List<BaseConfig> _configs;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputActions>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveLoadService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveDataManagementService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MetaCurrencyWallet>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneLoaderService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GlobalBootstrap>().AsSingle();
            Container.BindInterfacesAndSelfTo<ConfigProvider>().AsSingle().WithArguments(_configs);
        }
    }
}
