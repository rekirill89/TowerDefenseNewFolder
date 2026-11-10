using Zenject;
using _Project.Scripts.MVP;

namespace _Project.Scripts.Services
{
    public class BattleSceneBootstrap : IInitializable
    {
        private readonly IBattleControlService _battleControlService;
        private readonly IRewardService _rewardService;
        private readonly IBattleInfoService _battleInfoService;

        private readonly BattlePresentersRoot _presentersRoot;

        public BattleSceneBootstrap(
            IBattleControlService battleControlService,
            IRewardService rewardService,
            BattlePresentersRoot presentersRoot,
            BattleInfoService battleInfoService)
        {
            _battleControlService = battleControlService;
            _rewardService = rewardService;
            _presentersRoot = presentersRoot;
            _battleInfoService = battleInfoService;
        }

        public void Initialize()
        {
            _battleControlService.Init();
            _rewardService.Init();
            _presentersRoot.Init();
            _battleInfoService.Init();
        }
    }
}
