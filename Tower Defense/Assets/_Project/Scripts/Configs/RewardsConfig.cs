using UnityEngine;

namespace _Project.Scripts
{
    [CreateAssetMenu(fileName = "RewardsConfig", menuName = "Configs/RewardsConfig")]
    public class RewardsConfig : BaseConfig
    {
        [field:SerializeField] public int InitCoinsValue {get; private set;}
        [field:SerializeField] public int CoinsPerEnemy {get; private set;}
        [field:SerializeField] public int MetaMoneyPerEnemy {get; private set;}
        [field:SerializeField] public int MetaMoneyPerWave {get; private set;}
    }
}
