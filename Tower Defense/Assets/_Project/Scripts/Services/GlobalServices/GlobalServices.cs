namespace _Project.Scripts
{
    public class GlobalServices
    {
        public IMetaCurrencyWallet MetaWallet { get; private set; }
        public ISceneLoaderService SceneLoaderService { get; private set; }
        public ISaveLoadService SaveLoadService { get; private set; }

        public GlobalServices(
            ISceneLoaderService sceneLoaderService, 
            ISaveLoadService saveLoadService,
            IMetaCurrencyWallet metaWallet)
        {
            SceneLoaderService = sceneLoaderService;
            SaveLoadService = saveLoadService;
            MetaWallet = metaWallet;
        }
    }
}