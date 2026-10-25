using System;

namespace _Project.Scripts
{
    public delegate void RewardGained(RewardEnum rewardEnum, int amount);

    public interface IRewardService : IDisposable
    {
        event RewardGained OnRewardGained;

        void Init();
    }
}