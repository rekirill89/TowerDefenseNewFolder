using System;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public class MetaCurrencyWallet : IMetaCurrencyWallet
    {
        public event Action<int> OnBalanceChanged;
        public int Balance { get; private set; }

        public MetaCurrencyWallet()
        {
            Balance = 0;
        }

        public void SetBalance(int value)
        {
            Balance = value;
            OnBalanceChanged?.Invoke(Balance);
        }

        public void Add(int value)
        {
            if(value < 0 )
            {
                Debug.LogWarning($"[Economy] Attempted to add invalid amount: {value}");
                return;
            }
            Balance += value;
            OnBalanceChanged?.Invoke(Balance);
        }
    }
}