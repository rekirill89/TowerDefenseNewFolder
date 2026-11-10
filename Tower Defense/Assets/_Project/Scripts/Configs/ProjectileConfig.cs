using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Configs/ProjectileConfig")]
    public class ProjectileConfig : GameEntityConfig
    {
        [field: SerializeField] public float Speed {get; private set;}
    }
}
