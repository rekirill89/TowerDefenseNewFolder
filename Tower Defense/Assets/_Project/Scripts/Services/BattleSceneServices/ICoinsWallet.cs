namespace _Project.Scripts
{
    public delegate void CoinsChanged(int coins);

    public interface ICoinsWallet
    {
        event CoinsChanged OnCoinsChanged;
        int CoinsBalance { get; }

        void Collect(int coins);
        bool TrySpend(int cost);
        void ResetBalance();
    }
}