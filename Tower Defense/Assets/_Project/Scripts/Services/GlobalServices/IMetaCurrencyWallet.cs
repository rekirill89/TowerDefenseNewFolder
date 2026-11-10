using System;

namespace _Project.Scripts.Services
{
    public interface IMetaCurrencyWallet
    {
        event Action<int> OnBalanceChanged;
        int Balance { get; }

        void SetBalance(int value);
        void Add(int value);
    }
}