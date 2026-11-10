using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "TowerConfig", menuName = "Configs/TowerConfig")]
    public class TowerConfig : GameEntityConfig
    {
        [field: SerializeField] public float AttackInterval { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public int ConstructCost { get; private set; }
    }
}
