using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "StrongholdConfig", menuName = "Configs/StrongholdConfig")]
    public class StrongholdConfig : TowerConfig
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public float DamageCooldown { get; private set; }
    }
}
