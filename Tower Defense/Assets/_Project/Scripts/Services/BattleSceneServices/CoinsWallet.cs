using System;
using UnityEngine;
using _Project.Scripts.Configs;

namespace _Project.Scripts.Services
{
    public class CoinsWallet : ICoinsWallet
    {
        public event Action<int> OnBalanceChanged;
        public int Balance {get; private set;}

        private readonly int _startBalance = 0;

        public CoinsWallet(ConfigProvider configProvider)
        {
            _startBalance = configProvider.GetConfig<RewardsConfig>().StartCoins;
            Add(_startBalance);
        }
        
        public void Add(int coins)
        {
            if(coins < 0 )
            {
                Debug.LogWarning($"[Economy] Attempted to add invalid amount: {coins}");
                return;
            }
            Balance += coins;
            OnBalanceChanged?.Invoke(Balance);
        }

        public bool TrySpend(int cost)
        {
            if(cost < 0 )
            {
                Debug.LogWarning($"[Economy] Attempted to spend invalid amount: {cost}");
                return false;
            }

            if (cost > Balance)
            {
                Debug.Log($"[Economy] Insufficient money, balance:{Balance}/cost:{cost}");
                return false;
            }
            Balance -= cost;
            OnBalanceChanged?.Invoke(Balance);
            return true;
        }
        
        public void ResetBalance()
        {
            Balance = _startBalance;
        }
    }
}