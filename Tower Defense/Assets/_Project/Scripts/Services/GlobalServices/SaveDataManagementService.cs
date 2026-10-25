using System;

namespace _Project.Scripts
{
    public class SaveDataManagementService : ISaveDataManagementService
    {
        private readonly ISaveLoadService _saveLoadService;
        private readonly IMetaCurrencyWallet _metaWallet;

        public SaveDataManagementService(ISaveLoadService saveLoadService, IMetaCurrencyWallet metaWallet)
        {
            _saveLoadService = saveLoadService;
            _metaWallet = metaWallet;
        }

        public void Init()
        {
            var tempData = _saveLoadService.Load(); 
            _metaWallet.SetCurrentBalance(tempData.MetaCurrencyBalance);
        }
    }
}