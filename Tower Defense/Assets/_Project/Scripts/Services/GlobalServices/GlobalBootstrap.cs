using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts.Services
{
    public class GlobalBootstrap : IInitializable
    {
        private readonly ISaveDataManagementService _saveDataManagementService;

        public GlobalBootstrap(ISaveDataManagementService saveDataManagementService)
        {
            _saveDataManagementService = saveDataManagementService;
        }
        
        public void Initialize()
        {
            _saveDataManagementService.Init();
        }
    }
}