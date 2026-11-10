using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "RewardsConfig", menuName = "Configs/RewardsConfig")]
    public class RewardsConfig : BaseConfig
    {
        [field:SerializeField] public int StartCoins {get; private set;}
        [field:SerializeField] public int CoinsPerEnemy {get; private set;}
        [field:SerializeField] public int MetaCurrencyPerEnemy {get; private set;}
        [field:SerializeField] public int MetaCurrencyPerWave {get; private set;}
    }
}
