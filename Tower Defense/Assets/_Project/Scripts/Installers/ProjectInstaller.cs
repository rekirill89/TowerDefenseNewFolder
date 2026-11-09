using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private List<BaseConfig> _configs;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SaveLoadService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveDataManagementService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MetaCurrencyWallet>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneLoaderService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GlobalBootstrap>().AsSingle();
            Container.BindInterfacesAndSelfTo<ConfigProvider>().AsSingle().WithArguments(_configs);
        }
    }
}
