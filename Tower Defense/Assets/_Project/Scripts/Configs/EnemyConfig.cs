using UnityEngine;

namespace _Project.Scripts
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig")]
    public class EnemyConfig : GameEntityConfig
    {
        [field: SerializeField] public int MaxHealth { get; set; }
        [field: SerializeField] public int Damage { get; set; }
        [field: SerializeField] public float AttackRate { get; set; }
        [field: SerializeField] public float Speed { get; set; }
    }
}
