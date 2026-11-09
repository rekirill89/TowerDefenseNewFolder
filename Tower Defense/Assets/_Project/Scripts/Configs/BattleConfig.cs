using UnityEngine;

namespace _Project.Scripts
{
    [CreateAssetMenu(fileName = "BattleConfig", menuName = "Configs/BattleConfig")]
    public class BattleConfig : BaseConfig
    {
        [field:SerializeField] public Vector2 StrongholdPosition { get; private set; } 
    }
}
