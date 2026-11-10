using System;

namespace _Project.Scripts.Services
{
    public interface ICoinsWallet
    {
        event Action<int> OnBalanceChanged;
        int Balance { get; }

        void Add(int coins);
        bool TrySpend(int cost);
        void ResetBalance();
    }
}