using UnityEngine;

namespace _Project.Scripts
{
    [CreateAssetMenu(fileName = "TowerConfig", menuName = "Configs/TowerConfig")]
    public class TowerConfig : GameEntityConfig
    {
        [field: SerializeField] public float AttackTimer { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public int AttackDamage { get; private set; }
        [field: SerializeField] public int ConstructCost { get; private set; }
    }
}
