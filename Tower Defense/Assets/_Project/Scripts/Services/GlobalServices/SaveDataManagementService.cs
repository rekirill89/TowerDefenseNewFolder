using System;

namespace _Project.Scripts.Services
{
    public class SaveDataManagementService : ISaveDataManagementService
    {
        private readonly ISaveLoadService _saveLoadService;
        private readonly IMetaCurrencyWallet _metaCurrencyWallet;

        public SaveDataManagementService(ISaveLoadService saveLoadService, IMetaCurrencyWallet metaCurrencyWallet)
        {
            _saveLoadService = saveLoadService;
            _metaCurrencyWallet = metaCurrencyWallet;
        }

        public void Init()
        {
            var saveData = _saveLoadService.Load(); 
            _metaCurrencyWallet.SetBalance(saveData.MetaCurrencyBalance);
        }
    }
}