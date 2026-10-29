using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class MetaCurrencyWallet : IMetaCurrencyWallet
    {
        public event Action<int> OnBalanceChanged;
        public int CurrentBalance { get; private set; }

        public MetaCurrencyWallet()
        {
            CurrentBalance = 0;
        }

        public void SetCurrentBalance(int value)
        {
            CurrentBalance = value;
            OnBalanceChanged?.Invoke(CurrentBalance);
        }

        public void Collect(int value)
        {
            if(value < 0 )
            {
                Debug.LogWarning($"[Economy] Attempted to collect invalid amount: {value}");
                return;
            }
            CurrentBalance += value;
            OnBalanceChanged?.Invoke(CurrentBalance);
        }
    }
}