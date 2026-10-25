using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    public class GlobalBootstrap : MonoBehaviour
    {
        [SerializeField] private GlobalServicesRef globalServicesRef;
        
        private readonly DisposableCollector _disposableCollector = new DisposableCollector();
            
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            ISaveLoadService saveLoadService = new SaveLoadService();
            ISceneLoaderService sceneLoaderService = new SceneLoaderService();
            IMetaCurrencyWallet metaWallet = new MetaCurrencyWallet(0);
            ISaveDataManagementService saveDataManagementService = new SaveDataManagementService(saveLoadService, metaWallet);
            
            saveDataManagementService.Init();
            globalServicesRef.Init(sceneLoaderService, saveLoadService, metaWallet);
        }

        private void OnDestroy()
        {
            _disposableCollector.Dispose();
        }
    }
}