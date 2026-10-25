using System;

namespace _Project.Scripts
{
    public interface IMetaCurrencyWallet
    {
        event Action<int> OnBalanceChanged;
        int CurrentBalance { get; }

        void SetCurrentBalance(int value);
        void Collect(int value);
    }
}