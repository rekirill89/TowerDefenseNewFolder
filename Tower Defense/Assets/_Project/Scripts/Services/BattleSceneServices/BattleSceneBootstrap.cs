using System;
using System.Collections.Generic;
using _Project.Scripts.MVP;
using UnityEngine;
using Zenject;

namespace _Project.Scripts
{
    public class BattleSceneBootstrap : IInitializable
    {
        private readonly IBattleControlService _battleControlService;
        private readonly IRewardService _rewardService;
        private readonly IBattleInfoService _battleInfoService;

        private readonly MediatorPresenter _mediatorPresenter;

        public BattleSceneBootstrap(
            IBattleControlService battleControlService, 
            IRewardService rewardService,
            MediatorPresenter mediatorPresenter,
            BattleInfoService battleInfoService)
        {
            _battleControlService = battleControlService;
            _rewardService = rewardService;
            _mediatorPresenter = mediatorPresenter;
            _battleInfoService = battleInfoService;
        }

        public void Initialize()
        {
            _battleControlService.Init();
            _rewardService.Init();
            _mediatorPresenter.Init();
            _battleInfoService.Init();
        }
    }
}
